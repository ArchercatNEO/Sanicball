using System;
using Godot;
using Sanicball.GameMechanics;

namespace Sanicball.Ball;

/// <summary>
/// A UI overlay for balls inside a race
/// Contains things like the speed counter and the place counter
/// </summary>
[GodotClass]
public partial class RaceUI : SubViewportContainer
{
    private static readonly PackedScene prefab = GD.Load<PackedScene>("res://src/ball/RaceUI.tscn");

    public static RaceUI Create(SanicBall sanicBall)
    {
        RaceUI instance = prefab.Instantiate<RaceUI>();

        instance.sanicBall = sanicBall;
        instance.SubViewport.AddChild(sanicBall);

        return instance;
    }

    [BindProperty] public Label SubViewport { get; set; }
    [BindProperty] public Label LapCounter { get; set; }

    private SanicBall sanicBall = null!;
    private CheckpointReciever chachedReciever = null!;

    protected override void _Ready()
    {
        return;
        ArgumentNullException.ThrowIfNull(sanicBall.checkpointReciever);

        chachedReciever = sanicBall.checkpointReciever;
        chachedReciever.NextLap += (sender, lap) =>
        {
            LapCounter.Text = $"Lap: {lap + 1}/3";
        };

        chachedReciever.RaceFinished += (sender, args) =>
        {
            LapCounter.AddThemeColorOverride(new StringName("default_color"), new(0, 0, 1));
        };
    }
}
