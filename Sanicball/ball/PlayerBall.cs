using Godot;
using Sanicball.Account;
using Sanicball.GameMechanics;
using Sanicball.Scenes;

namespace Sanicball.Ball;

public partial class PlayerBall : AbstractBall, IBall, IRespawnable
{
    public static CSharpScript AsScript { get; } = GD.Load<CSharpScript>("res://Ball/PlayerBall.cs");

    public override void _Ready()
    {
        LobbyCamera.TrySubscribe(this);

    }

    public override void _Input(InputEvent @event)
    {
        Vector3 force = ControlType.Keyboard.NormalizedForce();
        force *= InputAcceleration;

        ApplyForce(force);
    }

    public void Respawn()
    {
        Position = new(0, 100, 0);
    }
}
