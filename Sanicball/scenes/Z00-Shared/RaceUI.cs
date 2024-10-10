using Godot;
using Sanicball.Characters;

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
    [BindProperty] BallCamera camera;

    private Character target = null!;
    
    public static RaceUI Create(Character character)
    {
        var prefab = GD.Load<PackedScene>("res://scenes/Z00-Shared/RaceUI.tscn");
        
        var self = prefab.Instantiate<RaceUI>();
        self.target = character;
        self.target.Camera = self.camera;

        self.camera.Ball = character;

        self.AddChild(character);
        
        return self;
    }

    public void AddTracker(ObjectMarker marker)
    {
        AddChild(marker);
    }
}
