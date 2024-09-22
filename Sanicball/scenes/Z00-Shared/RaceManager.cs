using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Sanicball.Ball;
using Sanicball.Characters;
using Sanicball.GameMechanics;

namespace Sanicball.Scenes;

//TODO: Implement AI Balls
//TODO: Implement end of match displays
//TODO: Implenent minimap
//TODO: Implement Race UI
//TODO: Improve splitscreen
[GodotClass]
public partial class RaceManager : Node
{
    public static void Activate(SceneTree tree, RaceOptions options)
    {
        var self = tree.ChangeSceneAsync<RaceManager>(options.SelectedStage.RaceScene);
        self.players = options.Players;
    }

    [BindProperty] private HBoxContainer viewportManager = null!;
    [BindProperty] private Checkpoint finishLine = null!;
    [BindProperty] private AiNode initialNode = null!;

    private List<Character> players = [];
    private int playersFinished = 0;

    protected override void _Ready()
    {
        float offset = 10;
        foreach (var character in players)
        {
            character.CheckpointPassed += null;
            
            RaceUI raceUI = RaceUI.Create();
            //TODO: Implement RaceUI
            
            //Subvieports don't expand to the size of parent UI containers
            //SubviewportContiner is needed to wrap the SubViewport
            SubViewportContainer expander = new();
            expander.AddChild(raceUI);
            viewportManager.AddChild(expander);
            
            character.GlobalPosition = finishLine.GlobalPosition with { Y = finishLine.GlobalPosition.Y + offset };
            offset += 10;
        }

        foreach (int count in Enumerable.Range(0, 5))
        {
            //TODO: Make ai balls configurable from lobby
            Character ai = new Asspio()
            {
                controller = new AiController()
                {
                    InitialNode = initialNode
                }
            };

            AddChild(ai);
            ai.CheckpointPassed += null;
            ai.GlobalPosition = finishLine.GlobalPosition with { Y = finishLine.GlobalPosition.Y + offset };
            offset += 10;
        }
    }

    private void OnPlayerFinishRace(object? sender, EventArgs e)
    {
        playersFinished++;
        if (playersFinished == players.Count)
        {
            LobbyManager.Activate(GetTree(), players);
        }
    }
}
