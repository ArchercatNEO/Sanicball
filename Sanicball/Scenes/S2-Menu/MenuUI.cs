using Godot;

namespace Sanicball.Scenes;

public partial class MenuUI : Control
{
    public override void _Ready()
    {
        //needs to spin
        Label pressAnyKey = GetNode<Label>("PressAnyKey");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey)
        {

        }
    }
}