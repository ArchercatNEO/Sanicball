using Godot;

namespace Sanicball.Assets;

//Icon = "res://assets/music/Song/AudioPlusPlus.svg"
[GodotClass]
public partial class Song : Resource
{
    [BindProperty] public string name;
    [BindProperty] public string credits;
    [BindProperty] public AudioStream stream;
}
