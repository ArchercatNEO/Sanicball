using System.IO;
using Godot;
using Serilog;

namespace Sanicball.Assets;

//Icon = "res://assets/music/Song/AudioPlusPlus.svg"
[GodotClass(Tool = true)]
public partial class Song : Resource
{
    private static FileSystemWatcher? watcher;

    static Song()
    {
        if (!Engine.Singleton.IsEditorHint())
        {
            return;
        }

        string root = ProjectSettings.Singleton.GlobalizePath("res://assets/audio/music");
        Log.Information(root);
        watcher = new FileSystemWatcher(root);
        watcher.EnableRaisingEvents = true;
        watcher.IncludeSubdirectories = true;

        watcher.Created += (sender, args) => Log.Information("Created");
        watcher.Deleted += (sender, args) => Log.Information("Deleted");
        watcher.Renamed += (sender, args) => Log.Information("Renamed");
        watcher.Error += (sender, args) => Log.Information("Error");
        watcher.Changed += (sender, args) =>
        {
            Log.Information("File path: {path} changed", args.FullPath);

            string dir = args.FullPath;
            if ((File.GetAttributes(args.FullPath) & FileAttributes.Directory) == 0)
            {
                dir = Path.GetDirectoryName(args.FullPath)!;
            }

            if (!File.Exists($"{dir}/index.tres"))
            {
                Log.Error("Index file at {dir} not found", dir);
                return;
            }

            if (!File.Exists($"{dir}/stream.ogg"))
            {
                Log.Error("Stream file at {dir} not found", dir);
                return;
            }

            Song song = GD.Load<Song>($"{dir}/index.tres");
            AudioStream stream = GD.Load<AudioStream>($"{dir}/stream.ogg");
            if (song.stream != stream)
            {
                song.stream = stream;
                ResourceSaver.Singleton.Save(song, $"{dir}/index.tres");
            }
        };
    }

    [BindProperty]
    public string name = "";

    [BindProperty]
    public string composer = "";

    [BindProperty]
    public string? origin;

    [BindProperty]
    public AudioStream stream;
}
