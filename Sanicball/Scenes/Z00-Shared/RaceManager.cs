using System.Collections.Generic;
using Godot;
using Sanicball.Account;
using Sanicball.Ball;
using Sanicball.GameMechanics;

namespace Sanicball.Scenes;

public partial class RaceManager : Node
{
    public static RaceManager? Instance { get; private set; }
    public override void _EnterTree() { Instance = this; }
    public override void _ExitTree() { Instance = null; }

    private static Dictionary<ControlType, SanicBall> raceBalls = [];
    
    public static void Activate(SceneTree tree, TrackResource raceTrack, Dictionary<ControlType, SanicBall> players)
    {
        raceBalls = players;

        foreach (var (control, player) in players)
        {
            Node parent = player.GetParent();
            parent.RemoveChild(player);
        }

        tree.ChangeSceneToPacked(raceTrack.RaceScene);
    }

    [Export] private HBoxContainer viewportManager = null!;
    [Export] private Checkpoint finishLine = null!;

    public override void _Ready()
    {
        float offset = 10;
        foreach (var (control, player) in raceBalls)
        {
            player.ActivateRace(finishLine);

            SubViewport screen = new();
            screen.AddChild(player);
            SubViewportContainer screenFitter = new()
            {
                LayoutMode = 2,
                SizeFlagsHorizontal = Control.SizeFlags.Fill | Control.SizeFlags.Expand,
                Stretch = true
            };
            screenFitter.AddChild(screen);
            viewportManager.AddChild(screenFitter);
            player.GlobalPosition = finishLine.GlobalPosition with { Y = finishLine.GlobalPosition.Y + offset};
            offset += 10;
        }
    }
}