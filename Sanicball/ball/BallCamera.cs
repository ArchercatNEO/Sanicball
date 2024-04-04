using Godot;

namespace Sanicball.Ball;

public partial class BallCamera : Camera3D
{
    private Vector3 lastRotation;

    private BallCamera()
    {
        lastRotation = Rotation;
    }

    public override void _PhysicsProcess(double delta)
    {
        //TODO use math to base rotation on the floor + speed
        Rotation = lastRotation;

        lastRotation = Rotation;
    }
}