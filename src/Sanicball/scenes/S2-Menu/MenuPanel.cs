using Godot;
using Serilog;

namespace Sanicball.Scenes;

[GodotClass]
public partial class MenuPanel : ColorRect
{
    protected override void _Ready()
    {
        var tree = GetTree();
        GetNode<Button>(new NodePath("Content/Local")).ButtonDown += () =>
            LobbyManager.Activate(tree, []);

        //TODO activate online mode
        GetNode<Button>(new NodePath("Content/Online")).ButtonDown += () =>
            LobbyManager.Activate(tree, []);

        //TODO store race records in player's account
        GetNode<Button>(new NodePath("Content/Records")).ButtonDown += () =>
            Log.Error("Unimplemented");

        //TODO store game settings in player's account and use them
        GetNode<Button>(new NodePath("Content/Settings")).ButtonDown += () =>
            Log.Error("Unimplemented");

        //TODO implement rebindable controls and store them in the player's account
        GetNode<Button>(new NodePath("Content/Controls")).ButtonDown += () =>
            Log.Error("Unimplemented");

        //TODO implement credits
        GetNode<Button>(new NodePath("Content/Credits")).ButtonDown += () =>
            Log.Error("Unimplemented");
        GetNode<Button>(new NodePath("Content/Quit")).ButtonDown += () => tree.Quit();
    }
}
