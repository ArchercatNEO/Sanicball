using Godot;

namespace Sanicball.Scenes;

/// <summary>
/// A resource to represent what a stage track is and does
/// </summary>

public partial class TrackResource : Resource
{
    [BindProperty] public required string name;
    [BindProperty] public required PackedScene RaceScene;
    [BindProperty] public required Texture2D flatOverlay;
    [BindProperty] public required PackedScene solidOverlay;

    public static readonly TrackResource GreenHillZone = GD.Load<TrackResource>("res://scenes/Z01-GreenHillZone/GreenHillZone.tres");
}