using Godot;

namespace Sanicball.Scenes;

/// <summary>
/// A resource to represent what a stage track is and does
/// </summary>
[GodotClass]
public partial class TrackResource : Resource
{
    [BindProperty] public string name;
    [BindProperty] public PackedScene RaceScene;
    [BindProperty] public Texture2D flatOverlay;
    [BindProperty] public PackedScene solidOverlay;

    public static readonly TrackResource GreenHillZone = GD.Load<TrackResource>("res://scenes/Z01-GreenHillZone/GreenHillZone.tres");
}
