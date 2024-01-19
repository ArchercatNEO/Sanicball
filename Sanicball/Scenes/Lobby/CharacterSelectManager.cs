using Godot;

namespace Sanicball.Scenes;

public partial class CharacterSelectManager : Control
{
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (Input.IsKeyPressed(Key.D))
        {
            CharacterSelect panel = CharacterSelect.Create(Account.ControlType.Keyboard);
            AddChild(panel);
        }
    }
}