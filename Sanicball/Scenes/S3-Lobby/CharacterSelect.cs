using System.Collections.Generic;
using Godot;
using Sanicball.Account;
using Sanicball.Ball;
using Sanicball.Characters;

namespace Sanicball.Scenes;

public partial class CharacterSelect : MarginContainer
{
    private static readonly PackedScene prefab = GD.Load<PackedScene>("res://Scenes/S3-Lobby/CharacterSelect.tscn");

    public static CharacterSelect Create(ControlType controlType)
    {
        CharacterSelect panel = prefab.Instantiate<CharacterSelect>();
        panel.controlType = controlType;
        panel.characterSelect ??= panel.GetNode<Control>("CharacterSelect");
        panel.characterName ??= panel.GetNode<Label>("CharacterSelect/CharacterName");
        panel.characterIcon ??= panel.GetNode<TextureRect>("CharacterSelect/CharacterIcon");
        return panel;
    }


    private static readonly SanicCharacter cancel = new() {
        Name = "Exit",
        Credits = "Doesn't matter",
        Material = new(),
        Color = new(1, 0, 0, 1),
        Icon = GD.Load<Texture2D>("res://SharedArt/Cancel.png"),
        MinimapIcon = GD.Load<Texture2D>("res://SharedArt/Cancel.png"),
    };

    private static readonly SanicCharacter[] characters = [cancel, .. SanicCharacter.All];
    private static readonly Dictionary<ControlType, SanicBall> players = [];

    [Export] private Control characterSelect = null!;
    [Export] private Label characterName = null!;
    [Export] private TextureRect characterIcon = null!;
    
    private ControlType controlType;
    private SanicBall? player;

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
            if (controlType.Confirmed())
            {
                characterSelect.Show();
                CharacterIndex = 0;

                players.Remove(controlType);
                if (player is not null)
                {
                    player.QueueFree();
                    player = null;
                }
            }
            return;
        }

        if (controlType.LeftPressed()) { CharacterIndex -= 1; }
        if (controlType.RightPressed()) { CharacterIndex += 1; }
        if (controlType.UpPressed()) { CharacterIndex -= 4; }
        if (controlType.DownPressed()) { CharacterIndex += 4; }

        if (controlType.Confirmed())
        {
            //cancel index
            if (CharacterIndex == 0)
            {                
                QueueFree();
                return;
            }

            characterSelect.Hide();

            player = SanicBall.Create(SelectedCharacter, new PlayerBall());
            LobbySpawner.Instance!.AddChild(player);
            player.Translate(new(0, 5, 0));
            player.ApplyImpulse(new(0, 10, 0));

            players[controlType] = player;

            GetViewport().SetInputAsHandled();
        }
    }
}