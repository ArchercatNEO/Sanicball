using Godot;

namespace Sanicball.Scenes;

/// <summary>
/// A resource to represent what a stage track is and does
/// </summary>
[GlobalClass]
public partial class TrackResource : Resource
{
    [Export] public required string name;
    [Export] public required PackedScene RaceScene;
    [Export] public required Texture2D flatOverlay;
    [Export] public required PackedScene solidOverlay;

    public static readonly TrackResource GreenHillZone = GD.Load<TrackResource>("res://src/scenes/Z01-GreenHillZone/GreenHillZone.tres");
}