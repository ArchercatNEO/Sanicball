using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator;

[Generator]
public class HelloSourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        var nullableContainingTypes = context.Compilation.Assembly.Modules
            .Select(module => module.GlobalNamespace)
            .SelectMany(rootNamespace => rootNamespace.GetTypeMembers())
            .Where(type => type.GetMembers().Where(member => 
                member.GetAttributes().Any(attribute => attribute.AttributeClass!.Name == "ExportAttribute")
            ).Any());

        foreach (var nullableType in nullableContainingTypes)
        {
            StringBuilder configurationWarnings = new();

            configurationWarnings.Append($@"
                public partial class {nullableType.Name}
                {{
            ");
            
            configurationWarnings.Append("""
            public override string[] _GetConfigurationWarnings()
            {
                List<string> warnings = ["Bad"];
            """);
                

            configurationWarnings.Append(" return warnings.ToArray();} } ");
                
            


            context.AddSource(nullableType.Name + ".g.cs", configurationWarnings.ToString());
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required for this one
    }
}

[Generator]
public class NullableExportsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var project = context.AdditionalTextsProvider.Where(static file => {
            Console.WriteLine("hello");
            Console.WriteLine(file);
            return true;
        });

        context.RegisterSourceOutput(project, (source, context) => {
            StringBuilder builder = new();
            builder.Append("""
            namespace Sanicball;

            public class GodotActions
            {
                public static readonly world = "hello"
            }
            """);
            source.AddSource("Project.cs", builder.ToString());
            DiagnosticDescriptor error = new("SB0000", "No null required fields", "hello", "security", DiagnosticSeverity.Error, true);
            Diagnostic diagnostic = Diagnostic.Create(error, null);
            source.ReportDiagnostic(diagnostic);
        });
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class PreloadAttribute(string _path) : Attribute
{
}


[Generator]
public class PreloadGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.AdditionalTextsProvider
            .Where(file => true) //Extension is relevant
            .Select((file, token) => file); //Merge into importable file
        var syntax = context.SyntaxProvider
            .ForAttributeWithMetadataName("SourceGenerator.PreloadAttribute", SyntaxFilter, SyntaxTransformer)
            .Collect()
            .SelectMany((fields, token) => {
                Dictionary<string, ClassAndFields> sorted = [];
                foreach (var field in fields)
                {
                    string fullName = field.containingNamespace + field.className;
                    if (!sorted.ContainsKey(fullName))
                    {
                        sorted.Add(fullName, new()
                        {
                            containingNamespace = field.containingNamespace,
                            className = field.className,
                            fields = []
                        });
                    }

                    ClassAndFields parent = sorted[fullName];
                    parent.fields.Add(field);
                }
                return sorted.Values;
            });
        context.RegisterSourceOutput(syntax, (ctx, classData) => {
            StringBuilder builder = new();
            builder.Append($$"""
            namespace Sanicball.{{classData.containingNamespace}};

            public partial class {{classData.className}}
            {
                static {{classData.className}}()
                {
            """);

            foreach (var field in classData.fields)
            {
                builder.Append($"{field.fieldName} = new();");
            }

            builder.Append("""
                }
            }
            """);

            ctx.AddSource($"{classData.className}.g.cs", builder.ToString());
        });
    }

    private static bool SyntaxFilter(SyntaxNode node, CancellationToken _cancellationToken)
    {
        return true;
    }

    private static FieldDescriptor SyntaxTransformer(GeneratorAttributeSyntaxContext context, CancellationToken _cancellationToken)
    {
        return new(){
            containingNamespace = context.TargetSymbol.ContainingNamespace.Name,
            className = context.TargetSymbol.ContainingType.Name,
            fieldName = context.TargetSymbol.Name,
            path = (string)context.Attributes[0].ConstructorArguments[0].Value!
        };
    }
}

internal struct ClassAndFields
{
    public string containingNamespace;
    public string className;
    public List<FieldDescriptor> fields;
}

internal struct FieldDescriptor
{
    public string containingNamespace;
    public string className;

    public string fieldName;
    public string path;
}