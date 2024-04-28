
using Godot;

namespace Sanicball.Sound;

[GlobalClass]
[Icon("res://addons/Song/AudioPlusPlus.svg")]
public partial class Song : Resource
{
    [Export] public required string name;
    [Export] public required string credits;
    [Export] public required AudioStream stream;
}