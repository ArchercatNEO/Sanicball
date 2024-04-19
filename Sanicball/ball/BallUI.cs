using Godot;

namespace Sanicball.Ball;

/// <summary>
/// A UI overlay for balls inside a race
/// Contains things like the speed counter and the place counter
/// </summary>
public partial class BallUI : Control
{
    private static readonly PackedScene prefab = GD.Load<PackedScene>("res://ball/BallUI.tscn");
    
    public static BallUI Create(SanicBall sanicBall)
    {
        BallUI instance = prefab.Instantiate<BallUI>();
        sanicBall.CurrentLapChanged += (sender, lap) =>
        {
            instance.lapCounter.Text = $"Lap: {lap + 1}/3";
        };
        return instance;
    }
    
    [Export] private Label lapCounter = null!;
}