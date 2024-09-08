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
//TODO Implement lobby settings
[GodotClass]
public partial class LobbyManager : Node
{
    private static LobbyManager? Instance { get; set; }

    private static readonly PackedScene prefab = GD.Load<PackedScene>("res://scenes/S3-Lobby/Lobby.tscn");

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
    [BindProperty] private HBoxContainer characterSelectContainer = null!;
    [BindProperty] private Label countdownText = null!;
    [BindProperty] private Control pauseMenu = null!;

    //3d
    [BindProperty] private Node3D playerSpawner = null!;


    private int readyPlayers = 0;
    private readonly List<SanicBallDescriptor> players = [];
    private readonly Dictionary<ControlType, CharacterSelect> activePanels = [];

    protected override void _EnterTree()
    {
        Instance = this;
        Input.Singleton.JoyConnectionChanged += OnDeviceConnected;
    }

    protected override void _Ready()
    {
        pauseMenu.GetNode<Button>(new NodePath("VBoxContainer/Unpause")).Pressed += pauseMenu.Hide;
        Button ctxSensitive = pauseMenu.GetNode<Button>(new NodePath("VBoxContainer/Context"));
        //TODO settings
        ctxSensitive.Pressed += pauseMenu.Hide;
        ctxSensitive.Text = "Settings";
        pauseMenu.GetNode<Button>(new NodePath("VBoxContainer/Quit")).Pressed += () => MenuUI.Activate(GetTree());
        pauseMenu.Hide();
    }

    protected override void _ExitTree()
    {
        Input.Singleton.JoyConnectionChanged -= OnDeviceConnected;
        Instance = null;
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
