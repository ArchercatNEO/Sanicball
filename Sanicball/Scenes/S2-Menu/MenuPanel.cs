using Godot;

namespace Sanicball.Scenes;

public partial class MenuPanel : ColorRect
{
    public override void _Ready()
    {
        SceneTree tree = GetTree();
        GetNode<Button>("Content/Local").ButtonDown += () => LobbyManager.Activate(tree);
        GetNode<Button>("Content/Online").ButtonDown += () => LobbyManager.Activate(tree);
        GetNode<Button>("Content/Records").ButtonDown += () => GD.Print("Unimplemented");
        GetNode<Button>("Content/Settings").ButtonDown += () => GD.Print("Unimplemented");
        GetNode<Button>("Content/Controls").ButtonDown += () => GD.Print("Unimplemented");
        GetNode<Button>("Content/Credits").ButtonDown += () => GD.Print("Unimplemented");
        GetNode<Button>("Content/Quit").ButtonDown += () => tree.Quit();
    }
}