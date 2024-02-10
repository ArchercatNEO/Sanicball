using Godot;
using Sanicball.Scenes;

namespace Sanicball.Plugins;

[Tool]
internal partial class NullExportWarnings : EditorPlugin
{
    NullExportChecker? gizmo = null;

    public override void _EnterTree()
    {
        gizmo = new();
        AddInspectorPlugin(gizmo);
    }

    public override void _ExitTree()
    {
        RemoveInspectorPlugin(gizmo);
        gizmo = null;
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
