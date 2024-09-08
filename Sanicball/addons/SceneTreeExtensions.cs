using Godot;

namespace Sanicball.Scenes;

public static class SceneTreeExtensions
{
    public static void ChangeSceneAsync(this SceneTree sceneTree, PackedScene scene)
    {
        Node root = sceneTree.Root;
        Node current = sceneTree.CurrentScene;

        root.RemoveChild(current);
        current.QueueFree();

        Node next = scene.Instantiate();
        root.AddChild(next);

        sceneTree.CurrentScene = next;
    }
}
