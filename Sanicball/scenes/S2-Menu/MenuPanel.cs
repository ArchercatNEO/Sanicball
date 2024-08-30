using Godot;

namespace Sanicball.Scenes;

public partial class MenuPanel : ColorRect
{
    public override void _Ready()
    {
        SceneTree tree = GetTree();
        GetNode<Button>("Content/Local").ButtonDown += () => LobbyManager.Activate(tree, []);
        
        //TODO activate online mode
        GetNode<Button>("Content/Online").ButtonDown += () => LobbyManager.Activate(tree, []);
        
        //TODO store race records in player's account
        GetNode<Button>("Content/Records").ButtonDown += () => GD.Print("Unimplemented");

        //TODO store game settings in player's account and use them
        GetNode<Button>("Content/Settings").ButtonDown += () => GD.Print("Unimplemented");

        //TODO implement rebindable controls and store them in the player's account
        GetNode<Button>("Content/Controls").ButtonDown += () => GD.Print("Unimplemented");

        //TODO implement credits
        GetNode<Button>("Content/Credits").ButtonDown += () => GD.Print("Unimplemented");
        GetNode<Button>("Content/Quit").ButtonDown += () => tree.Quit();
    }
}