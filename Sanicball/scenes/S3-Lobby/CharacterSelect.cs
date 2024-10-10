using System;
using Godot;
using Sanicball.Account;
using Sanicball.Characters;

namespace Sanicball.Scenes;

/// <summary>
/// Manages most player input inside the lobby.
/// Handles setting ready, player joining and leaving
/// </summary>
//TODO Implement a preview grid to see what character is near
[GodotClass]
public partial class CharacterSelect : MarginContainer
{
    public static CharacterSelect Create(ControlType controlType, Node3D spawner, Character? character)
    {
        var prefab = GD.Load<PackedScene>("res://scenes/S3-Lobby/CharacterSelect.tscn");

        CharacterSelect self = prefab.Instantiate<CharacterSelect>();
        self.controlType = controlType;
        self.playerSpawner = spawner;
        
        self.controllerIcon.Texture = controlType.Icon();
        self.characterSelect.Hide();
        self.hotkeyLabel.Text = """
        [Arrow keys]: Select character,
        [Enter]: Confirm
        """;

        if (character is not null)
        {
            self.SpawnPlayer(character);
        }
        
        return self;
    }

    [BindProperty] private TextureRect background = null!;
    [BindProperty] private TextureRect controllerIcon = null!;
    [BindProperty] private Control characterSelect = null!;
    [BindProperty] private TextureRect characterIcon = null!;
    [BindProperty] private Label characterName = null!;
    [BindProperty] private Label hotkeyLabel = null!;

    private Node3D playerSpawner = null!;
    //TODO: refactor so these can be made private 
    public ControlType controlType;
    public Character? Player { get; private set; }

    //TODO: This will orphan a massive amount of nodes
    private readonly Character[] characters =
    [
        new Sanic(),
        new Asspio()
    ];

    public event EventHandler<bool>? OnReadyChanged;
    private bool ready = false;

    private int _characterIndex = 0;
    private int CharacterIndex
    {
        get => _characterIndex;
        set
        {
            int count = characters.Length;
            _characterIndex = (value + count) % count;
            
            if (_characterIndex == 0)
            {
                characterName.Text = "Cancel";
                characterIcon.Texture = GD.Load<Texture2D>("res://assets/art/Cancel.png");
            }
            else
            {
                Character selectedCharacter = characters[_characterIndex - 1];
                characterName.Text = (string)selectedCharacter.Name;
                characterIcon.Texture = selectedCharacter.Icon;
            }
        }
    }

    protected override void _Input(InputEvent @event)
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

                Player?.QueueFree();
                Player = null;
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

            Character selected = characters[CharacterIndex];
            SpawnPlayer(selected);
        }
    }

    private void SpawnPlayer(Character selected)
    {
        Player = selected;
        Player.controller = new PlayerController() { ControlType = controlType };
        
        playerSpawner.AddChild(Player);
        Player.Translate(new(0, 5, 0));
        Player.ApplyImpulse(new(0, 10, 0));
        LobbyCamera.Instance?.Subscribe(Player);
    }
}
