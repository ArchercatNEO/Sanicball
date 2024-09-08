
using Godot;

namespace Sanicball.Sound;

//Icon = "res://addons/Song/AudioPlusPlus.svg"
public partial class Song : Resource
{
    [BindProperty] public string name;
    [BindProperty] public string credits;
    [BindProperty] public AudioStream stream;
}
