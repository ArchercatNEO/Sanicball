using Godot;

namespace Sanicball.Ball;

/// <summary>
/// A UI overlay for balls inside a race
/// Builder will handle UI elements like speedometers and checkpoint/player markers
/// </summary>
[GodotClass]
//TODO: Implement speedometer
//TODO: Implement lap counter
//TODO: Implement checkpoint tracking
public partial class RaceUI : SubViewport
{
    public static RaceUI Create()
    {
        var prefab = GD.Load<PackedScene<RaceUI>>("res://scenes/Z00-Shared/RaceUI.tscn");
        
        var self = prefab.Instantiate();
        
        return self;
    }

    public void AddTracker(ObjectMarker marker)
    {
        AddChild(marker);
    }
}
