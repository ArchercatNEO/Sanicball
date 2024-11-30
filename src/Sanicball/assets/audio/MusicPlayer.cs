using System.IO;
using System.Linq;
using Godot;
using Sanicball.Assets;

namespace Sanicball.Assets;

[GodotClass]
public partial class MusicPlayer : AudioStreamPlayer
{
    public static MusicPlayer Instance { get; private set; } = null!;

    protected override void _Ready()
    {
        Instance = this;
    }

    public void PlaySong(Song song)
    {
        Stream = song.stream;
        Play();
    }

    public void PlayAlbum(string albumRoot)
    {
        string root = ProjectSettings.Singleton.GlobalizePath(albumRoot);
        string[] dirs = Directory.GetDirectories(root);
        Song[] songs = dirs.Select(path => GD.Load<Song>($"{path}/index.tres")).ToArray();

        int song = 0;

        var first = songs[0];
        Stream = first.stream;
        Play();

        Finished += () =>
        {
            song = (song + 1) % songs.Length;

            var current = songs[song];
            Stream = current.stream;
            Play();
        };
    }
}
