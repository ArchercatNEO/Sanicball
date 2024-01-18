using Godot;

namespace Sanicball.Scenes;

public partial class MenuPanel : ColorRect
{
    public override void _Ready()
    {
        GetNode<Button>("Content/Local").ButtonDown += () => GetTree().ChangeSceneToFile("res://Scenes/Lobby/Lobby.tscn");
        GetNode<Button>("Content/Online").ButtonDown += () => GD.Print("Unimplemented");
        GetNode<Button>("Content/Records").ButtonDown += () => GD.Print("Unimplemented");
        GetNode<Button>("Content/Settings").ButtonDown += () => GD.Print("Unimplemented");
        GetNode<Button>("Content/Controls").ButtonDown += () => GD.Print("Unimplemented");
        GetNode<Button>("Content/Credits").ButtonDown += () => GD.Print("Unimplemented");
        GetNode<Button>("Content/Quit").ButtonDown += () => GetTree().Quit();
    }
}