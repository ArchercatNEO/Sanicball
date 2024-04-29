using System;
using System.Collections.Generic;
using System.Linq;
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

    private static List<SanicBallDescriptor> players = [];

    public static void Activate(SceneTree tree, RaceOptions options)
    {
        players = options.Players;
        tree.ChangeSceneToPacked(options.SelectedStage.RaceScene);
    }

    [Export] private HBoxContainer viewportManager = null!;
    [Export] private Checkpoint finishLine = null!;

    private int playersFinished = 0;

    private void OnPlayerFinishRace(object? sender, EventArgs e)
    {
        playersFinished++;
        if (playersFinished == players.Count)
        {
            LobbyManager.Activate(GetTree(), players);
        }
    }

    public override void _Ready()
    {
        float offset = 10;
        foreach (var (character, controller) in players)
        {
            CheckpointReciever reciever = new(finishLine, 3);
            reciever.RaceFinished += OnPlayerFinishRace;
            
            SanicBall raceBall = SanicBall.CreateRace(character, controller, reciever);
            RaceUI raceUI = RaceUI.Create(raceBall);

            viewportManager.AddChild(raceUI);

            raceBall.GlobalPosition = finishLine.GlobalPosition with { Y = finishLine.GlobalPosition.Y + offset };
            offset += 10;
        }

        foreach (int count in Enumerable.Range(0, 5))
        {
            CheckpointReciever reciever = new(finishLine, 3);
            
            AiBall aiBall = new();
            
            SanicBall raceBall = SanicBall.CreateRace(SanicCharacter.Asspio, aiBall, reciever);
            
            AddChild(raceBall);
            
            raceBall.GlobalPosition = finishLine.GlobalPosition with { Y = finishLine.GlobalPosition.Y + offset };
            offset += 10;
        }
    }
}