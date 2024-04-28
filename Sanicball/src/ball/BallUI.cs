using System;
using Godot;
using Sanicball.GameMechanics;

namespace Sanicball.Ball;

/// <summary>
/// A UI overlay for balls inside a race
/// Contains things like the speed counter and the place counter
/// </summary>
public partial class BallUI : Control
{
    [Export] private SanicBall sanicBall = null!;
    [Export] private Label lapCounter = null!;

    private CheckpointReciever chachedReciever = null!;

    public override void _Ready()
    {
        return;
        ArgumentNullException.ThrowIfNull(sanicBall.checkpointReciever);
        
        chachedReciever = sanicBall.checkpointReciever;
        chachedReciever.NextLap += (sender, lap) =>
        {
            lapCounter.Text = $"Lap: {lap + 1}/3";
        };

        chachedReciever.RaceFinished += (sender, args) =>
        {
            lapCounter.AddThemeColorOverride("default_color", new(0, 0, 1));
        };
    }
}