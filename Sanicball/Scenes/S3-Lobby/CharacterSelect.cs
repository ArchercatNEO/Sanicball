using System;
using Godot;
using Sanicball.Account;
using Sanicball.Ball;
using Sanicball.Characters;

namespace Sanicball.Scenes;

public partial class CharacterSelect : Control
{
    private static readonly PackedScene prefab = GD.Load<PackedScene>("res://Scenes/S3-Lobby/CharacterSelect.tscn");

    public static CharacterSelect Create(ControlType controlType)
    {
        CharacterSelect panel = prefab.Instantiate<CharacterSelect>();
        panel.controlType = controlType;
        panel.characterName ??= panel.GetNode<Label>("CharacterSelect/CharacterName");
        panel.characterIcon ??= panel.GetNode<TextureRect>("CharacterSelect/CharacterIcon");
        return panel;
    }

    public event EventHandler<ConfirmationEvent>? OnPlayerConfirmed;
    
    [Export] private Label characterName = null!;
    [Export] private TextureRect characterIcon = null!;
    
    private SanicCharacter[] characters = [null!, .. SanicCharacter.All];
    private ControlType controlType;

    private int _characterIndex = 0;
    private int CharacterIndex
    {
        get => _characterIndex;
        set
        {
            _characterIndex = (value + 16) % 16;
            SanicCharacter character = SanicCharacter.All[_characterIndex];
            characterName.Text = character.Name;
            characterIcon.Texture = character.Icon;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (controlType.LeftPressed()) { CharacterIndex -= 1; }
        if (controlType.RightPressed()) { CharacterIndex += 1; }
        if (controlType.UpPressed()) { CharacterIndex -= 4; }
        if (controlType.DownPressed()) { CharacterIndex += 4; }

        if (controlType.Confirmed())
        {
            //cancel index
            if (_characterIndex == 0)
            {
                OnPlayerConfirmed?.Invoke(this, new());
                QueueFree();
                return;
            }

            GetNode<Control>("CharacterSelect").Hide();

            SanicBall player = SanicBall.Create(SanicCharacter.All[CharacterIndex], new PlayerBall());
            LobbySpawner.Instance!.AddChild(player);
            player.Translate(new(0, 5, 0));
            player.ApplyImpulse(new(0, 10, 0));
            
            OnPlayerConfirmed?.Invoke(this, new() { SelectedCharacter = SanicCharacter.All[CharacterIndex] });

            GetViewport().SetInputAsHandled();
        }
    }
}

public class ConfirmationEvent
{
    public SanicCharacter? SelectedCharacter { get; init; }
}