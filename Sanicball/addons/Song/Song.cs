
using Godot;

namespace Sanicball.Sound;

//Icon = "res://addons/Song/AudioPlusPlus.svg"
public partial class Song : Resource
{
    [BindProperty] public required string name;
    [BindProperty] public required string credits;
    [BindProperty] public required AudioStream stream;
}