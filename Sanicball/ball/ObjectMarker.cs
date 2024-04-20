using Godot;

namespace Sanicball.Ball;


public partial class ObjectMarker : Control
{
    private static readonly PackedScene prefab = GD.Load<PackedScene>("res://ball/ObjectMarker.tscn");

    /// <summary>
    /// Factory method
    /// </summary>
    /// <param name="origin">The node3D from where calculation will be done</param>
    /// <param name="target">The node3D from where calculation will be done</param>
    /// <param name="color">The node3D from where calculation will be done</param>
    /// <returns>The created object</returns>
    public static ObjectMarker Create(Camera3D origin, Node3D target, Color color)
    {
        ObjectMarker marker = prefab.Instantiate<ObjectMarker>();

        marker.origin = origin;
        origin.TreeExited += marker.QueueFree;
        
        marker.target = target;
        target.TreeExited += marker.QueueFree;
        
        marker.Modulate = color;
        return marker;
    }

    private Camera3D origin = null!;
    private Node3D target = null!;

    public override void _Process(double delta)
    {
        Projection projectionMatrix = origin.GetCameraProjection();
        Vector3 thing = origin.Transform * target.Position;
        Vector3 projectedVector = projectionMatrix * thing;
        Position = Position with { X = projectedVector.X, Y = projectedVector.Y };
    }
}