using Godot;
using Godot.Collections;
using System;

namespace Sanicball.Tools;

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
    public override string _GetVisibleName() => "Texture2D";

    public override string[] _GetRecognizedExtensions() => ["xcf"];
    public override string _GetResourceType() => nameof(Texture2D);

    public override Error _Import(string sourceFile, string savePath, Dictionary options, Array<string> platformVariants, Array<string> genFiles)
    {
		FileAccess file = FileAccess.Open(sourceFile, FileAccess.ModeFlags.Read);
		Texture2D image = new();
		ResourceSaver.Save(image, savePath + ".xcf");
		return Error.Ok;
    }
}
