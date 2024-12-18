using Godot;
using Serilog;

namespace Sanicball.Scenes;

//TODO Sanicball translate and expand animation
//TODO Press any key hide + spin animation
[GodotClass]
public partial class MenuUI : Control
{
    public static void Activate(SceneTree tree)
    {
        Log.Information("Entering Menu");

        var scene = GD.Load<PackedScene>("res://scenes/S2-Menu/menu.tscn");
        tree.ChangeSceneToPacked(scene);
    }

    protected override void _Ready()
    {
        //needs to spin
        var pressAnyKey = GetNode<Label>(new NodePath("PressAnyKey"));
    }

    protected override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey) { }
    }
}
