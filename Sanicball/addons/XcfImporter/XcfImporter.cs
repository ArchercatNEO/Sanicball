#if TOOLS

using Godot;
using Godot.Collections;
using System;

namespace Sanicball.Plugins;

[GodotClass(Tool = true)]
public partial class XcfImporter : EditorPlugin
{
    XcfConverter? importer = new();

    protected override void _EnterTree()
    {
        // Initialization of the plugin goes here.
        AddImportPlugin(importer);
    }

    protected override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        RemoveImportPlugin(importer);
    }
}

public partial class XcfConverter : EditorImportPlugin
{
    protected override string _GetImporterName() => "GIMP file importer";
    protected override string _GetVisibleName() => "Texture2D";

    protected override string[] _GetRecognizedExtensions() => ["xcf"];
    protected override string _GetResourceType() => nameof(Texture2D);
    protected override string _GetSaveExtension() => "ctex";

    protected override float _GetPriority() => 1.0f;
    protected override int _GetPresetCount() => 0;
    protected override int _GetImportOrder() => 0;

    protected override Array<Dictionary> _GetImportOptions(string path, int presetIndex)
    {
        return [];
    }

    protected override bool _GetOptionVisibility(string path, StringName optionName, Dictionary options) => true;

    protected override Error _Import(string sourceFile, string savePath, Dictionary options, Array<string> platformVariants, Array<string> genFiles)
    {
        FileAccess file = FileAccess.Open(sourceFile, FileAccess.ModeFlags.Read);
        Texture2D image = new();
        ResourceSaver.Save(image, savePath + ".ctex");
        return Error.Ok;
    }
}

#endif
