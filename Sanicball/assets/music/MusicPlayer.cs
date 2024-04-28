using Godot;

namespace Sanicball.Sound;

public partial class MusicPlayer : AudioStreamPlayer
{
    [Export] public required bool muted = false;
    [Export] public required Label songName;

    private readonly Song[] songs;
    private int songIndex = 0;

    private MusicPlayer()
    {
        songs = [
            GD.Load<Song>("res://assets/music/ChariotsOfFire.tres"),
            GD.Load<Song>("res://assets/music/Dread.tres")
        ];
    }

    public override void _Ready()
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
