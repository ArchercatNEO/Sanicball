using Godot;

namespace Sanicball.Scenes;

/// <summary>
/// A resource to represent what a stage track is and does
/// </summary>
[GlobalClass]
public partial class TrackResource : Resource
{
    [Export] public required string name;
    [Export] public required PackedScene prefab;
    [Export] public required Texture2D flatOverlay;
    [Export] public required PackedScene solidOverlay;

    public void Activate(SceneTree tree)
    {
        tree.ChangeSceneToPacked(prefab);
    }

    public static readonly TrackResource GreenHillZone = GD.Load<TrackResource>("res://Scenes/Z01-GreenHillZone/GreenHillZone.tres");
}