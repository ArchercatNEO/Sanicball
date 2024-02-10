using Godot;
using Godot.Collections;
using System;

namespace Sanicball.Plugins;

[Tool]
public partial class XcfImporter : EditorPlugin
{
    EditorImportPlugin? importer = null;

    public override void _EnterTree()
    {
        // Initialization of the plugin goes here.
        importer = new();
        AddImportPlugin(importer);
    }

    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
        RemoveImportPlugin(importer);
        importer = null;
    }
}

[Tool]
public partial class XcfConverter : EditorImportPlugin
{
    public override string _GetImporterName() => "GIMP file importer";
    public string GetImporterName() => "GIMP file importer";
    public string _get_importer_name() => "GIMP file importer";
    public override string _GetVisibleName() => "GIMP image file";

    public override string[] _GetRecognizedExtensions() => ["xcf"];
    public override string _GetResourceType() => nameof(Resource);
    public override string _GetSaveExtension() => "res";

    public override float _GetPriority() => 1.0f;
    public override int _GetPresetCount() => 0;
    public override int _GetImportOrder() => 0;

     public override bool _GetOptionVisibility(string path, StringName optionName, Dictionary options) => true;

    public override Error _Import(string sourceFile, string savePath, Dictionary options, Array<string> platformVariants, Array<string> genFiles)
    {
        FileAccess file = FileAccess.Open(sourceFile, FileAccess.ModeFlags.Read);
        Texture2D image = new();
        ResourceSaver.Save(image, savePath + ".res");
        return Error.Ok;
    }
}
