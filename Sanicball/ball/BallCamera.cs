using Godot;

namespace Sanicball.Ball;

/// <summary>
/// <list type="bullet">
///     <listheader> Specification </listheader>
///     <item>
/// 		<term> Orientation quaternion </term>
/// 		<description> The quaternion that maps Vector3.Forward to the ball's linear velocity (normalized) </description>
/// 	</item>
///     <item>
/// 		<term> Position (global) </term>
/// 		<description> Ball's global position + Orientation * orbit vector </description>
/// 	</item>
///     <item>
/// 		<term> Rotation </term>
/// 		<description> The rotation that looks at the ball directly </description>
/// 	</item>
/// </list>
/// </summary>
public partial class BallCamera : Camera3D
{
    [Export] private SanicBall sanicBall = null!;
    
    private float _orbitHeight;
    [Export] public float OrbitHeight
    {
        get => _orbitHeight;
        private set
        {
            _orbitHeight = value;
            Position = Position with { Y = value };
        } 
    }

    private float _orbitRadius;
    [Export] public float OrbitRadius
    {
        get => _orbitRadius;
        private set
        {
            _orbitRadius = value;
            Position = Position with { Z = value };
        }
    }

    private float orbitAngle;

    public override void _Ready()
    {
        Position = new(0, OrbitHeight, OrbitRadius);
        orbitAngle = Position.AngleTo(Vector3.Forward) + Mathf.Pi / 2;
    }

    public override void _PhysicsProcess(double delta)
    {
        //Position updates
        Vector3 normalizedVelocity = sanicBall.LinearVelocity.Normalized() * OrbitRadius;
        //TODO Use ball's up instead of global up
        Vector3 rotationAxis = normalizedVelocity.Cross(Vector3.Up).Normalized();
        //Normalisation will fail if velocity and "up" are aligned
        if (rotationAxis.IsNormalized())
        {
            Vector3 orbitVector = normalizedVelocity.Rotated(rotationAxis, -orbitAngle);
            //TODO handle zooming in/out
            GlobalPosition = sanicBall.GlobalPosition + orbitVector;
        }

        //Rotation updates
        LookAt(sanicBall.Position);
    }
}