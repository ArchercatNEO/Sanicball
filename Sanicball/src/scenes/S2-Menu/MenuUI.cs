using Godot;

namespace Sanicball.Scenes;

public partial class MenuUI : Control
{
    private static readonly PackedScene Scene = GD.Load<PackedScene>("res://src/scenes/S2-Menu/menu.tscn");

    public static void Activate(SceneTree tree)
    {
        tree.ChangeSceneToPacked(Scene);
    }


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