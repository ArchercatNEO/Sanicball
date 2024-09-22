using System;

namespace Godot;

public class PackedScene<T>(PackedScene scene) where T : class /* Allows for interfaces */
{
    public T Instantiate()
    {
        return (T)(object)scene.Instantiate();
    }

    public static explicit operator PackedScene<T>(PackedScene castedScene)
    {
        StringName owner = castedScene.GetState().GetNodeType(0);
        StringName wanted = new(typeof(T).Name);
        if (owner != wanted)
        {
            throw new InvalidCastException($"Object of type {owner} cannot be casted to type {wanted} (check type of {castedScene.ResourcePath})");
        }
        return new(castedScene);
    }
}
