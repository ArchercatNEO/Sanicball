using System;
using Godot;
using Sanicball.Account;
using Sanicball.Ball;
using Sanicball.Characters;

namespace Sanicball.Scenes;

/// <summary>
/// Manages most player input inside the lobby.
/// Handles setting ready, player joining and leaving
/// </summary>
public partial class CharacterSelect : MarginContainer
{
    private static readonly PackedScene prefab = GD.Load<PackedScene>("res://Scenes/S3-Lobby/CharacterSelect.tscn");

    public static CharacterSelect Create(ControlType controlType, Node3D spawner)
    {
        CharacterSelect panel = prefab.Instantiate<CharacterSelect>();
        panel.playerSpawner = spawner;
        panel.controlType = controlType;
        panel.controllerIcon ??= panel.GetNode<TextureRect>("CharacterSelect");
        panel.controllerIcon.Texture = controlType.Icon();
        panel.characterSelect ??= panel.GetNode<Control>("CharacterSelect");
        panel.characterSelect.Hide();
        panel.characterIcon ??= panel.GetNode<TextureRect>("CharacterSelect/CharacterIcon");
        panel.characterName ??= panel.GetNode<Label>("CharacterSelect/CharacterName");
        panel.hotkeyLabel.Text = """
        [Arrow keys]: Select character,
        [Enter]: Confirm
        """;
        return panel;
    }

    private static readonly SanicCharacter cancel = new()
    {
        Name = "Exit",
        Credits = "Doesn't matter",
        Material = new(),
        Color = new(1, 0, 0, 1),
        Icon = GD.Load<Texture2D>("res://SharedArt/Cancel.png"),
        MinimapIcon = GD.Load<Texture2D>("res://SharedArt/Cancel.png"),
    };

    private static readonly SanicCharacter[] characters = [cancel, .. SanicCharacter.All];

    [Export] private TextureRect background = null!;
    [Export] private TextureRect controllerIcon = null!;
    [Export] private Control characterSelect = null!;
    [Export] private TextureRect characterIcon = null!;
    [Export] private Label characterName = null!;
    [Export] private Label hotkeyLabel = null!;
    private Node3D playerSpawner = null!;

    public SanicBall? Player { get; private set; }
    private ControlType controlType;

    private bool ready = false;
    public event EventHandler<bool>? OnReadyChanged;

    private int _characterIndex = 0;
    private int CharacterIndex
    {
        get => _characterIndex;
        set
        {
            _characterIndex = (value + 16) % 16;
            SelectedCharacter = characters[_characterIndex];
            characterName.Text = SelectedCharacter.Name;
            characterIcon.Texture = SelectedCharacter.Icon;
        }
    }

    private SanicCharacter SelectedCharacter = cancel;

    public override void _Input(InputEvent @event)
    {
        if (!characterSelect.Visible)
        {
            if (controlType.Ready(@event) && Player is not null)
            {
                ready = !ready;
                OnReadyChanged?.Invoke(this, ready);
                background.Modulate = ready ? new Color(0, 0, 1) : new Color(1, 1, 1);
            }

            if (controlType.Confirmed(@event))
            {
                characterSelect.Show();
                CharacterIndex = 0;

                if (Player is not null)
                {
                    Player.QueueFree();
                    Player = null;
                }
            }

            return;
        }

        if (controlType.LeftPressed(@event)) { CharacterIndex -= 1; }
        if (controlType.RightPressed(@event)) { CharacterIndex += 1; }
        if (controlType.UpPressed(@event)) { CharacterIndex -= 4; }
        if (controlType.DownPressed(@event)) { CharacterIndex += 4; }

        if (controlType.Confirmed(@event))
        {
            characterSelect.Hide();

            //cancel index
            if (CharacterIndex == 0) { return; }

            Player = SanicBall.CreateLobby(SelectedCharacter, new PlayerBall() { ControlType = controlType });
            playerSpawner.AddChild(Player);
            Player.Translate(new(0, 5, 0));
            Player.ApplyImpulse(new(0, 10, 0));
        }
    }
}