using Godot;
using Sanicball.Assets;

namespace Sanicball.Scenes;

[GodotClass]
public partial class MusicPlayer : AudioStreamPlayer
{
    [BindProperty] public bool muted = false;
    [BindProperty] public Label songName;

    private int songIndex = 0;
    private readonly Song[] songs = [
        GD.Load<Song>("res://assets/music/Dread.tres"),
        GD.Load<Song>("res://assets/music/ChariotsOfFire.tres")
    ];

    protected override void _Ready()
    {
        if (muted) { return; }
        Song first = songs[0];
        songName.Text = first.name;
        this.Stream = first.stream;
        this.Play();

        this.Finished += () =>
        {
            this.songIndex = (this.songIndex + 1) % this.songs.Length;

            Song current = songs[songIndex];
            songName.Text = current.name;
            this.Stream = current.stream;
            this.Play();
        };
    }
}
