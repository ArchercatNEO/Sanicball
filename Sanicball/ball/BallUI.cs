using Godot;

namespace Sanicball.Ball;

/// <summary>
/// A UI overlay for balls inside a race
/// Contains things like the speed counter and the place counter
/// </summary>
public partial class BallUI : Control
{    
    [Export] private SanicBall sanicBall = null!;
    [Export] private Label lapCounter = null!;

    public override void _Ready()
    {
        sanicBall.CurrentLapChanged += (sender, lap) =>
        {
            lapCounter.Text = $"Lap: {lap + 1}/3";
        };
    }
}