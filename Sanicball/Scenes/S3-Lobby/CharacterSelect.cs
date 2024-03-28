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
        return panel;
    }

    private ControlType controlType;

    public event EventHandler<ConfirmationEvent>? OnPlayerConfirmed;

    public override void _Input(InputEvent @event)
    {
        if (!controlType.Confirmed()) { return; }
        
        SanicBall player = SanicBall.Create(SanicCharacter.Sanic, new PlayerBall());
        LobbySpawner.Instance!.AddChild(player);
        player.Translate(new(0, 5, 0));
        player.ApplyImpulse(new(0, 10, 0));
        
        OnPlayerConfirmed?.Invoke(this, new());

        GetViewport().SetInputAsHandled();

        QueueFree();
    }
}

public class ConfirmationEvent
{

}