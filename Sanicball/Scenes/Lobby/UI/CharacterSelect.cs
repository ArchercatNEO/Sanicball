using Godot;
using Sanicball.Account;

namespace Sanicball.Scenes;

public partial class CharacterSelect : Control
{
    private static readonly PackedScene prefab = GD.Load<PackedScene>("res://Scenes/Lobby/UI/CharacterSelect.tscn");

    public static CharacterSelect Create(ControlType controlType)
    {
        CharacterSelect panel = prefab.Instantiate<CharacterSelect>();
        panel.controlType = controlType;
        return panel;
    }

    private ControlType controlType;

    public override void _Input(InputEvent @event)
    {
        if (controlType.Confirmed())
        {
            LobbySpawner.SpawnBall();
            QueueFree();
        }
    }
}