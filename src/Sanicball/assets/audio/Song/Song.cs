using Godot;

namespace Sanicball.Assets;

//Icon = "res://assets/music/Song/AudioPlusPlus.svg"
[GodotClass(Tool = true)]
public partial class Song : Resource
{
    [BindProperty] public string credits = "";
    [BindProperty] public string name = "";
    [BindProperty] public AudioStream stream;

    public Song()
    {
        string path = ResourcePath;
        stream = GD.Load<AudioStream>($"{path}/stream.ogg");
    }
}
