using Godot;

namespace Sanicball.Scenes;

[GlobalClass]
public partial class TrackResource : Resource
{
    [Export] public required string name;
    [Export] public required PackedScene prefab;
    [Export] public required Texture2D flatOverlay;
    [Export] public required PackedScene solidOverlay;
}