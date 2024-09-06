using Godot;
using Sanicball.Account;
using Sanicball.Characters;
using Sanicball.Scenes;

namespace Sanicball.Ball;

/// <summary>
/// A ball controlled by the local machine.
/// This kind of ball is the only one players have any control over
/// </summary>
public class PlayerBall : ISanicController
{
    /// <summary>
    /// The controller of this player, used to determine where the ball needs to turn towards
    /// </summary>
    /// <see cref="Account.ControlType"/>
    public  ControlType ControlType { get; init; }

    private SanicBall sanicBall = null!;

    public void Initialise(SanicBall parent)
    {
        sanicBall = parent;
    }

    public void Process(double delta)
    {
        Vector3 force = ControlType.NormalizedForce();

        //Rotate force so that Vector3.Forwards == Camera.Forwards
        force = Quaternion.FromEuler(sanicBall.Camera.Rotation) * force;

        //Convert a linear force into the axis of rotation
        Vector3 torque = -force.Cross(sanicBall.UpOverride);

        torque *= sanicBall.character.AngularAcceleration;

        sanicBall.ApplyTorque(torque);
    }
}
