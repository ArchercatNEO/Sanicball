using Godot;
using Sanicball.Scenes;

namespace Sanicball.Tools;

[Tool]
internal partial class NullExportWarnings : EditorPlugin
{
	NullExportChecker gizmo = new();

    public override void _EnterTree()
    {
        GD.Print(GetTree().CurrentScene is null);
        AddInspectorPlugin(gizmo);
    }

    public override void _ExitTree()
    {
        RemoveInspectorPlugin(gizmo);
    }
}

internal partial class NullExportChecker : EditorInspectorPlugin
{
    public override bool _CanHandle(GodotObject @object)
    {
        return @object.GetScript().VariantType != Variant.Type.Nil;
    }

    public override bool _ParseProperty(GodotObject @object, Variant.Type type, string name, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        if ((usageFlags & PropertyUsageFlags.ScriptVariable) > 0)
        {
            object? field = @object.Get(name);
            
            GD.Print(field);
        }
        return false;
    }
}
