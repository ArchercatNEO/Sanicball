using System;
using System.Collections.Generic;
using Godot;
using Sanicball.Account;
using Sanicball.Characters;

namespace Sanicball.Scenes;

/// <summary>
/// The top level handler of the lobby scene,
/// this is the only place the amount of players should increase
/// </summary>
//TODO Implement lobby settings
[GodotClass]
public partial class LobbyManager : Node
{
    public static void Activate(SceneTree tree, List<Character> players)
    {
        GD.Print("Entering lobby");
        var prefab = GD.Load<PackedScene>("res://scenes/S3-Lobby/Lobby.tscn");
        var self = tree.ChangeSceneAsync<LobbyManager>(prefab, self => {
            self.players = players;
        });
    }

    //ui
    [BindProperty] private Label countdownText = null!;
    [BindProperty] private Control pauseMenu = null!;
    [BindProperty] private HBoxContainer characterSelectContainer = null!;

    //3d
    [BindProperty] private Node3D playerSpawner = null!;

    private int readyPlayers = 0;
    private List<Character> players = [];
    private readonly Dictionary<ControlType, CharacterSelect> activePanels = [];

    protected override void _EnterTree()
    {
        Input.Singleton.JoyConnectionChanged += OnDeviceConnected;
    }

    protected override void _Ready()
    {
        foreach (var player in players)
        {
            GD.Print("Adding players to LobbyManager");
            if (player.controller is PlayerController controller)
            {
                CharacterSelect panel = CharacterSelect.Create(controller.ControlType, playerSpawner, player);
                characterSelectContainer.AddChild(panel);
                activePanels.Add(controller.ControlType, panel);
                panel.OnReadyChanged += OnReadyChanged;
            }
            //TODO: if player is AI -> settings
        }

        //TODO: Find a way to be able to start from lobby with keyboard
        //and return from race as a character
        GD.Print("No keyboard player detected, adding one in");
        OnDeviceConnected((long)ControlType.Keyboard, true);

        GD.Print("Setting up callbacks");
        pauseMenu.GetNode<Button>(new NodePath("Unpause")).Pressed += pauseMenu.Hide;
        pauseMenu.GetNode<Button>(new NodePath("Quit")).Pressed += () => MenuUI.Activate(GetTree());
        pauseMenu.GetParent<Control>().Hide();
        
        //TODO: settings
        Button ctxSensitive = pauseMenu.GetNode<Button>(new NodePath("Context"));
        ctxSensitive.Pressed += pauseMenu.Hide;
        ctxSensitive.Text = "Settings";
    }

    protected override void _ExitTree()
    {
        Input.Singleton.JoyConnectionChanged -= OnDeviceConnected;
    }

    protected override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey inputEvent)
        {
            if (inputEvent.Keycode == Key.P)
            {
                pauseMenu.Show();
                pauseMenu.GetNode<Button>(new NodePath("VBoxContainer/Unpause")).GrabFocus();
                //TODO pause game
            }
        }
    }

    private void OnDeviceConnected(long device, bool connected)
    {
        ControlType controller = (ControlType)device;
        if (connected && !activePanels.ContainsKey(controller))
        {
            CharacterSelect panel = CharacterSelect.Create(controller, playerSpawner, null);
            characterSelectContainer.AddChild(panel);
            activePanels.Add(controller, panel);
            panel.OnReadyChanged += OnReadyChanged;
        }
        else
        {
            CharacterSelect panel = activePanels[controller];
            characterSelectContainer.RemoveChild(panel);
            activePanels.Remove(controller);
            panel.QueueFree();
        }
    }

    private void OnReadyChanged(object? sender, bool ready)
    {
        ArgumentNullException.ThrowIfNull(sender);
        CharacterSelect panel = (CharacterSelect)sender;

        if (ready)
        {
            readyPlayers++;
            players.Add(panel.Player!);
        }
        else
        {
            readyPlayers--;
            players.RemoveAll(ball => ball.controller is PlayerController player && player.ControlType == panel.controlType);
        }

        if (readyPlayers < players.Count)
        {
            countdownText.Text = $"{readyPlayers}/{players.Count} players ready: waiting for more players";
            return;
        }

        RaceOptions options = new()
        {
            Players = players,
            SelectedStage = GD.Load<TrackResource>("res://scenes/Z01-GreenHillZone/GreenHillZone.tres")
        };

        Tween tween = CreateTween();

        void SetCountdownText(float time) => countdownText.Text = $"{readyPlayers}/{players.Count} players ready: Match starting in {time} seconds";
        tween.TweenMethod(Callable.From<float>(SetCountdownText), 5, 0, 5);

        tween.TweenCallback(Callable.From(() => {
            foreach (var player in players)
            {
                playerSpawner.RemoveChild(player);
            }
            RaceManager.Activate(GetTree(), options);
        }));
    }
}
