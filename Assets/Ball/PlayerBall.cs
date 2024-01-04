using Godot;
using Sanicball.Account;

namespace Sanicball.Ball;

public partial class PlayerBall : AbstractBall
{
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);


        Vector3 force = ControlType.Joystick1.NormalizedForce();
        force *= InputAcceleration;

        ApplyForce(force);
    }
}
