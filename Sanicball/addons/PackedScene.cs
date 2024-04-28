using System;

namespace Godot;

public class PackedScene<T>(PackedScene scene) where T : Node
{
    public T Instantiate()
    {
        return (T)scene.Instantiate();
    }

    public static explicit operator PackedScene<T>(PackedScene castedScene)
    {
        string owner = castedScene.GetState().GetNodeType(0);
        string wanted = typeof(T).Name;
        if (owner != wanted)
        {
            throw new InvalidCastException($"Object of type {owner} cannot be casted to type {wanted} (check type of {castedScene.ResourcePath})");
        }
        return new(castedScene);
    }
}