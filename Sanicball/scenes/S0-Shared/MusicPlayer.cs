using Godot;
using Sanicball.Assets;

namespace Sanicball.Scenes;

[GodotClass]
public partial class MusicPlayer : AudioStreamPlayer
{
    private readonly Song[] songs =
    [
        GD.Load<Song>("res://assets/music/Dread.tres"),
        GD.Load<Song>("res://assets/music/ChariotsOfFire.tres")
    ];

    [BindProperty] public bool muted = false;

    private int songIndex;
    [BindProperty] public Label songName;

    protected override void _Ready()
    {
        if (muted) { return; }

        var first = songs[0];
        songName.Text = first.name;
        Stream = first.stream;
        Play();

        Finished += () =>
        {
            songIndex = (songIndex + 1) % songs.Length;

            var current = songs[songIndex];
            songName.Text = current.name;
            Stream = current.stream;
            Play();
        };
    }
}
