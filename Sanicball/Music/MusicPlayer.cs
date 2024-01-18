using Godot;

namespace Sanicball.Sound;

public partial class MusicPlayer : AudioStreamPlayer
{
    [Export] private Label songName = null!;
    
    private readonly Song[] songs;
    private int songIndex = 0;

    private MusicPlayer()
    {
        songs = [
            new("Chariots of fire", GD.Load<AudioStreamOggVorbis>("res://Music/ChariotsOfFire.ogg")),
            new("Dread", GD.Load<AudioStreamOggVorbis>("res://Music/Dread.ogg"))
        ];
    }

    public override void _Ready()
    {
        Song first = songs[0];
        songName.Text = first.Name;
        this.Stream = first.MP3;
        this.Play();

        this.Finished += () =>
        {
            this.songIndex = (this.songIndex + 1) % this.songs.Length;

            Song current = songs[songIndex];
            songName.Text = current.Name;
            this.Stream = current.MP3;
            this.Play();
        };
    }
}

public record class Song(string Name, AudioStreamOggVorbis MP3);