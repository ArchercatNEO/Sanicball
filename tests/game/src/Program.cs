using Godot;
using Godot.Collections;

[GodotClass]
public partial class Ball : RigidBody3D
{
    public Ball()
    {
        ContactMonitor = true;
        MaxContactsReported = 3;
    }

    protected override void _Ready()
    {
        GD.Print("Hello world");
    }

    protected override void _PhysicsProcess(double delta)
    {
        GodotArray<Node3D> collisions = GetCollidingBodies();
    }
}
