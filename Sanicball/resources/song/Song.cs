
using Godot;

namespace Sanicball.Sound;

[GlobalClass]
[Icon("res://resources/song/AudioPlusPlus.svg")]
public partial class Song : Resource
{
    [Export] public required string name;
    [Export] public required string credits;
    [Export] public required AudioStream stream;
}