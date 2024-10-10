#if TOOLS

using Godot;

namespace Sanicball.Plugins;

[GodotClass(Tool = true)]
internal partial class NullExportWarnings : EditorPlugin
{
    NullExportChecker? gizmo = null;

    protected override void _EnterTree()
    {
        gizmo = new();
        AddInspectorPlugin(gizmo);
    }

    protected override void _ExitTree()
    {
        RemoveInspectorPlugin(gizmo);
        gizmo = null;
    }
}

internal partial class NullExportChecker : EditorInspectorPlugin
{
    protected override bool _CanHandle(GodotObject @object)
    {
        return @object is INullProperties;
    }

    protected override bool _ParseProperty(GodotObject @object, Variant.Type type, string name, PropertyHint hintType, string hintString, PropertyUsageFlags usageFlags, bool wide)
    {
        if ((usageFlags & PropertyUsageFlags.ScriptVariable) > 0)
        {
            object? field = @object.Get(name);
            if (field is null)
            {
                Log.Error("Field {name} on {node} is null", name, @object)
            }
        }
        return false;
    }
}

public interface INullProperties
{
    //TODO add methods
}

#endif
