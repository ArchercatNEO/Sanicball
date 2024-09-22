using Godot;

namespace Sanicball.Scenes;

public static class SceneTreeExtensions
{
    public static T ChangeSceneAsync<T>(this SceneTree sceneTree, PackedScene<T> scene) where T : Node
    {
        Node root = sceneTree.Root;
        
        Node current = sceneTree.CurrentScene;
        root.RemoveChild(current);
        current.QueueFree();

        T next = scene.Instantiate();
        root.AddChild(next);

        sceneTree.CurrentScene = next;
        return next;
    }

    public static T ChangeSceneAsync<T>(this SceneTree sceneTree, PackedScene scene) where T : Node
    {
        Node root = sceneTree.Root;
        
        Node current = sceneTree.CurrentScene;
        root.RemoveChild(current);
        current.QueueFree();

        T next = scene.Instantiate<T>();
        root.AddChild(next);

        sceneTree.CurrentScene = next;
        return next;
    }
}
