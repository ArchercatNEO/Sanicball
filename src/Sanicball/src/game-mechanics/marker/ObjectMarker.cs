using Godot;

namespace Sanicball.Ball;

[GodotClass]
public partial class ObjectMarker : Control
{
    private static readonly PackedScene prefab = GD.Load<PackedScene>(
        "res://src/game-mechanics/marker/ObjectMarker.tscn"
    );

    /// <summary>
    ///     Factory method
    /// </summary>
    /// <param name="origin">The node3D from where calculation will be done</param>
    /// <param name="target">The node3D from where calculation will be done</param>
    /// <param name="color">The node3D from where calculation will be done</param>
    /// <returns>The created object</returns>
    public static ObjectMarker Create(Camera3D origin, Node3D target, Color color)
    {
        var marker = prefab.Instantiate<ObjectMarker>();

        marker.origin = origin;
        origin.TreeExited += marker.QueueFree;

        marker.target = target;
        target.TreeExited += marker.QueueFree;

        marker.Modulate = color;
        return marker;
    }

    private Camera3D origin = null!;
    private Node3D target = null!;

    protected override void _Process(double delta)
    {
        var projectionMatrix = origin.GetCameraProjection();
        var thing = origin.Transform * target.Position;
        var projectedVector = projectionMatrix * thing;
        //Position = Position with { X = projectedVector.X, Y = projectedVector.Y };
    }
}
