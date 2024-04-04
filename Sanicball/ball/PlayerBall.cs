using Godot;
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
        LobbyCamera.Instance?.Subscribe(parent);

        sanicBall.OnRespawn += (sender, body) =>
        {
            sanicBall.Position = new(0, 100, 0);
        };
    }

    public void Process(double delta)
    {
        Vector3 force = ControlType.NormalizedForce();
        force *= SanicBall.InputAcceleration;

        sanicBall.ApplyForce(force);
    }

    public void ActivateRace()
    {
        sanicBall.AddChild(new Camera3D());
    }
}
