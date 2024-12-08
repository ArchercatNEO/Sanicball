using System;
using Godot;

namespace Sanicball.Utils;

public static class SceneTreeExtensions
{
    public static T ChangeSceneAsync<T>(this SceneTree sceneTree, PackedScene<T> scene)
        where T : Node
    {
        Node root = sceneTree.Root;

        var current = sceneTree.CurrentScene;
        root.RemoveChild(current);
        current.QueueFree();

        var next = scene.Instantiate();
        root.AddChild(next);

        sceneTree.CurrentScene = next;
        return next;
    }

    public static T ChangeSceneAsync<T>(
        this SceneTree sceneTree,
        PackedScene scene,
        Action<T>? preReady = null
    )
        where T : Node
    {
        Node root = sceneTree.Root;

        var current = sceneTree.CurrentScene;
        root.RemoveChild(current);
        current.QueueFree();

        var next = scene.Instantiate<T>();
        preReady?.Invoke(next);
        root.AddChild(next);

        sceneTree.CurrentScene = next;
        return next;
    }
}
