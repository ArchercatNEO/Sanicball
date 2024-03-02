using Godot;

namespace Sanicball.Scenes;

public partial class MenuUI : Control
{
    public static readonly PackedScene Scene = GD.Load<PackedScene>("res://Scenes/S2-Menu/menu.tscn");
    
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