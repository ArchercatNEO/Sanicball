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
    private SanicBall sanicBall = null!;
    public required  ControlType ControlType { get; init; }

    public void Initialise(SanicBall parent)
    {
        sanicBall = parent;
        sanicBall.ContactMonitor = true;
        sanicBall.MaxContactsReported = 3;

        LobbyCamera.Instance?.Subscribe(parent);
    }

    public void Process(double delta)
    {
        Vector3 force = ControlType.NormalizedForce();
        //TODO use lobby camera if omnicamera is not found
        if (sanicBall.Camera is not null)
        {
            //Rotate force so that Vector3.Forwards == Camera.Forwards
            force = Quaternion.FromEuler(sanicBall.Camera.Rotation) * force;
        }
        
        //Convert a linear force into the axis of rotation
        Vector3 torque = -force.Cross(sanicBall.UpOverride);
        
        torque *= sanicBall.character.AngularAcceleration;
        torque *= SanicBall.InputAcceleration;

        sanicBall.ApplyTorque(torque);
    }
}
