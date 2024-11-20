using Godot;
using Sanicball.Characters;

namespace Sanicball.Ball;

/// <summary>
///     <list type="bullet">
///         <listheader> Specification </listheader>
///         <item>
///             <term> Orientation quaternion </term>
///             <description> The quaternion that maps Vector3.Forward to the ball's linear velocity (normalized) </description>
///         </item>
///         <item>
///             <term> Position (global) </term>
///             <description> Ball's global position + Orientation * orbit vector </description>
///         </item>
///         <item>
///             <term> Rotation </term>
///             <description> The rotation that looks at the ball directly </description>
///         </item>
///     </list>
/// </summary>
[GodotClass]
public partial class BallCamera : Camera3D
{
    private float _orbitHeight;

    private float _orbitRadius;

    private float orbitAngle;
    private Vector3 previousOrbit;
    public Character Ball { get; set; }

    [BindProperty]
    public float OrbitHeight
    {
        get => _orbitHeight;
        private set
        {
            _orbitHeight = value;
            Position = Position with { Y = value };
        }
    }

    [BindProperty]
    public float OrbitRadius
    {
        get => _orbitRadius;
        private set
        {
            _orbitRadius = value;
            Position = Position with { Z = value };
        }
    }

    protected override void _Ready()
    {
        Position = new Vector3(0, OrbitHeight, OrbitRadius);
        orbitAngle = Position.AngleTo(Vector3.Forward) + Mathf.Pi / 2;
    }

    protected override void _PhysicsProcess(double delta)
    {
        //Position updates
        //TODO handle zooming in/out

        //Check if velocity and the up vector are not aligned so the cross product will work
        if (Ball.LinearVelocity.Dot(Ball.UpOverride) - Ball.LinearVelocity.Length() * Ball.UpOverride.Length() != 0)
        {
            var normalizedVelocity = Ball.LinearVelocity.Normalized() * OrbitRadius;
            var rotationAxis = normalizedVelocity.Cross(Ball.UpOverride).Normalized();
            if (!rotationAxis.IsNormalized())
            {
                return;
            }

            var orbitVector = normalizedVelocity.Rotated(rotationAxis, -orbitAngle);
            var offsetVector = previousOrbit.Lerp(orbitVector, (float)delta);
            previousOrbit = offsetVector;
            GlobalPosition = Ball.GlobalPosition + offsetVector;
        }


        //Rotation updates
        LookAt(Ball.Position);
    }
}
