using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

/// <summary>
/// Turn an Action field into an online-mode message
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
public sealed class ServerRelayedAttribute : System.Attribute
{
    public static readonly Type Type = typeof(ServerRelayedAttribute);
    public static readonly AssemblyName name = new("MatchMessage");
    private static readonly ModuleBuilder builder;
    
    static ServerRelayedAttribute()
    {
        builder = AssemblyBuilder
            .DefineDynamicAssembly(name, AssemblyBuilderAccess.Run)
            .DefineDynamicModule(name.Name ?? "MatchMessage");
    }

    public static void ReplaceMethods<T>()
    {
        FieldInfo[] fields = typeof(T).GetFields();
        foreach (FieldInfo field in fields)
        {
            ServerRelayedAttribute? attribute = (ServerRelayedAttribute?) field.GetCustomAttribute(Type);
            if (attribute is null) continue;

            if (field.GetValue(null) is Delegate action)
            {
                MethodInfo method = action.Method;
                ParameterInfo[] args = method.GetParameters();
            
                attribute.CreateMessage(field, args);
            }
        }
    }

    private Type CreateMessage(FieldInfo actionField, ParameterInfo[] args)
    {
        Type actionType = actionField.FieldType;
        Type[] types = args.Select(arg => arg.ParameterType).ToArray();
        TypeBuilder message = builder.DefineType(actionField.Name + "Message", TypeAttributes.Public);


        // For a constructor, argument zero is a reference to the new
        // instance. Push it on the stack before calling the base
        // class constructor. Specify the default constructor of the
        // base class (Sanicball.Packet) by passing an empty array of
        // types (Type.EmptyTypes) to GetConstructor.
        ConstructorInfo? ci = typeof(Packet).GetConstructor(Type.EmptyTypes);
        ILGenerator ctor = message.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.HasThis,
            Type.EmptyTypes
        ).GetILGenerator();

        ctor.Emit(OpCodes.Ldarg_0);
        ctor.Emit(OpCodes.Call, ci!);
        ctor.Emit(OpCodes.Ret);

        ILGenerator constructor = message.DefineConstructor(
            MethodAttributes.Public,
            CallingConventions.HasThis,
            types
        ).GetILGenerator();

        constructor.Emit(OpCodes.Ldarg_0);
        constructor.Emit(OpCodes.Call, ci!);

        FieldBuilder consumeAction = message.DefineField(actionField.Name + "Internal", actionType, FieldAttributes.Static);
        MethodBuilder consumeBody = message.DefineMethod(
            "Consume",
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask,
            CallingConventions.HasThis,
            typeof(void),
            Type.EmptyTypes
        );
        ILGenerator consumeAssembly = consumeBody.GetILGenerator();

        consumeAssembly.Emit(OpCodes.Ldsfld, consumeAction);

        for (int i = 0; i < args.Length; i++)
        {
            ParameterInfo param = args[i];
            FieldBuilder field = message.DefineField(param.Name!, param.ParameterType, FieldAttributes.Public);
            
            constructor.Emit(OpCodes.Ldarg_0); // 'this'
            constructor.Emit(OpCodes.Ldarg_S, i + 1); //parameter of constructor
            constructor.Emit(OpCodes.Stfld, field);

            consumeAssembly.Emit(OpCodes.Ldarg_0);
            consumeAssembly.Emit(OpCodes.Ldfld, field);
        }
        
        consumeAssembly.Emit(OpCodes.Callvirt, actionType.GetMethod("Invoke")!);

        constructor.Emit(OpCodes.Ret);
        consumeAssembly.Emit(OpCodes.Ret);

        Type type = message.CreateType();
        ConstructorInfo def = type.GetConstructors()[1];
        FieldInfo a = type.GetFields()[0];
        MethodInfo method = type.GetMethod("Consume")!;
         
        /* Move the tagged method into a static action here */
        type.GetField(actionField.Name + "Internal", BindingFlags.NonPublic | BindingFlags.Static)?
            .SetValue(null, actionField.GetValue(null));

        /* Load a fake shell that does new().Send() into the tagged method */
        /* actionField.SetValue(null, (string name) => {
            Console.WriteLine("Relaying method: ");
            
            object? instance = def.Invoke([name]);
            method.Invoke(instance, null);
        }); */
        
        return type;
    }
}