using Godot;
using Godot.Collections;
using Sanicball.Account;
using Sanicball.Characters;
using Sanicball.Scenes;

namespace Sanicball.Ball;

/// <summary>
/// A ball controlled by the local machine.
/// This kind of ball is the only one players have any control over
/// </summary>
public partial class PlayerBall : ISanicController
{
    private SanicBall sanicBall = null!;
    public required  ControlType ControlType { get; init; }

    public void Initialise(SanicBall parent)
    {
        sanicBall = parent;
        sanicBall.ContactMonitor = true;
        sanicBall.MaxContactsReported = 3;

        LobbyCamera.Instance?.Subscribe(parent);

        sanicBall.OnRespawn += (sender, body) =>
        {
            sanicBall.Position = new(0, 100, 0);
        };
    }

    public void Process(double delta)
    {
        Array<Node3D> collisions = sanicBall.GetCollidingBodies();
        
        //TODO better floor check
        //? How can we detect a loop vs a wall?
        if (collisions.Count == 0) { return; }
        
        Vector3 force = ControlType.NormalizedForce();
        //TODO use lobby camera if omnicamera is not found
        if (sanicBall.Camera is not null)
        {
            force = Quaternion.FromEuler(sanicBall.Camera.Rotation) * force;
        }
        
        //TODO use floor collision to determine where "up" is 
        Vector3 up = Vector3.Up; 
        
        Vector3 torque = -force.Cross(up);
        torque *= SanicBall.InputAcceleration;

        sanicBall.ApplyTorque(torque);
    }
}
