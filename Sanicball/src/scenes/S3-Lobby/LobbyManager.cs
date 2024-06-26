using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Sanicball.Account;
using Sanicball.Ball;

namespace Sanicball.Scenes;

/// <summary>
/// The top level handler of the lobby scene,
/// this is the only place the amount of players should increase
/// </summary>
public partial class LobbyManager : Node
{
    private static LobbyManager? Instance { get; set; }

    private static readonly PackedScene prefab = GD.Load<PackedScene>("res://src/scenes/S3-Lobby/Lobby.tscn");

    public static void Activate(SceneTree tree, List<SanicBallDescriptor> players)
    {
        tree.ChangeSceneAsync(prefab);

        GD.Print("Activating scene");
        if (!players.Any(player => player.Controller is PlayerBall { ControlType: ControlType.Keyboard }))
        {
            GD.Print("No keyboard player detected, adding one in");
            Instance.OnDeviceConnected((long)ControlType.Keyboard, true);
        }
        
        Instance.players.AddRange(players);
        foreach (var descriptor in players)
        {
            if (descriptor.Controller is PlayerBall player)
            {
                CharacterSelect panel = CharacterSelect.CreatePlayer(player.ControlType, Instance.playerSpawner, descriptor.Character);
                Instance.characterSelectContainer.AddChild(panel);
                Instance.activePanels.Add(player.ControlType, panel);
                panel.OnReadyChanged += Instance.OnReadyChanged;
            }
        }
    }

    //ui
    [Export] private HBoxContainer characterSelectContainer = null!;
    [Export] private Label countdownText = null!;
    [Export] private Control pauseMenu = null!;
    
    //3d
    [Export] private Node3D playerSpawner = null!;


    private int readyPlayers = 0;
    private readonly List<SanicBallDescriptor> players = [];
    private readonly Dictionary<ControlType, CharacterSelect> activePanels = [];

    public override void _EnterTree()
    {
        Instance = this;
        Input.JoyConnectionChanged += OnDeviceConnected;
    }

    public override void _Ready()
    {
        pauseMenu.GetNode<Button>("VBoxContainer/Unpause").Pressed += pauseMenu.Hide;
        Button ctxSensitive = pauseMenu.GetNode<Button>("VBoxContainer/Context");
        //TODO settings
        ctxSensitive.Pressed += pauseMenu.Hide;
        ctxSensitive.Text = "Settings";
        pauseMenu.GetNode<Button>("VBoxContainer/Quit").Pressed += () => MenuUI.Activate(GetTree());
        pauseMenu.Hide();
    }

    public override void _ExitTree()
    {
        Input.JoyConnectionChanged -= OnDeviceConnected;
        Instance = null;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey inputEvent)
        {
            if (inputEvent.Keycode == Key.P)
            {
                pauseMenu.Show();
                pauseMenu.GetNode<Button>("VBoxContainer/Unpause").GrabFocus();
                //TODO pause game
            }
        }
    }

    private void OnDeviceConnected(long device, bool connected)
    {
        ControlType controller = (ControlType)device;
        if (connected && !activePanels.ContainsKey(controller))
        {
            CharacterSelect panel = CharacterSelect.Create(controller, playerSpawner);
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
            players.Add(new(panel.Player!.character, new PlayerBall() { ControlType = panel.controlType }));
        }
        else
        {
            readyPlayers--;
            players.RemoveAll(ball => ball.Controller is PlayerBall player && player.ControlType == panel.controlType);
        }

        if (readyPlayers < players.Count)
        {
            countdownText.Text = $"{readyPlayers}/{players.Count} players ready: waiting for more players";
            return;
        }
        
        RaceOptions options = new()
        {
            Players = players,
            SelectedStage = TrackResource.GreenHillZone
        };

        Tween tween = CreateTween();

        void SetCountdownText(float time) => countdownText.Text = $"{readyPlayers}/{players.Count} players ready: Match starting in {time} seconds";
        tween.TweenMethod(Callable.From<float>(SetCountdownText), 5, 0, 5);

        void startRaceCallback() => RaceManager.Activate(GetTree(), options);
        tween.TweenCallback(Callable.From(startRaceCallback));
    }
}