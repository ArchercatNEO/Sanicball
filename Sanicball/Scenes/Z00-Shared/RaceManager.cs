using System.Collections.Generic;
using Godot;
using Sanicball.Ball;
using Sanicball.Characters;
using Sanicball.GameMechanics;

namespace Sanicball.Scenes;

public partial class RaceManager : Node
{
    public static RaceManager? Instance { get; private set; }
    public override void _EnterTree() { Instance = this; }
    public override void _ExitTree() { Instance = null; }

    private static List<(SanicCharacter character, ISanicController controller)> players = [];

    public static void Activate(SceneTree tree, RaceOptions options)
    {
        players = options.Players;
        tree.ChangeSceneToPacked(options.SelectedStage.RaceScene);
    }

    [Export] private HBoxContainer viewportManager = null!;
    [Export] private Checkpoint finishLine = null!;

    public override void _Ready()
    {
        float offset = 10;
        foreach (var (character, controller) in players)
        {
            CheckpointReciever reciever = new(finishLine, 3);
            reciever.RaceFinished += (sender, e) => LobbyManager.Activate(GetTree());
            SanicBall raceBall = SanicBall.CreateRace(character, controller, reciever);

            Node viewportContainer = raceBall.GetNode("../..");
            viewportManager.AddChild(viewportContainer);

            raceBall.GlobalPosition = finishLine.GlobalPosition with { Y = finishLine.GlobalPosition.Y + offset };
            offset += 10;
        }
    }
}