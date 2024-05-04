using Godot;
using Sanicball.Characters;

namespace Sanicball.Ball;

/// <summary>
/// A ball controlled by the computer.
/// This kind of ball is not currently supported in online mode
/// </summary>
public class AiBall(AiNode initialTarget) : ISanicController
{
    //Settings -> AI skill adds noise to the navigation
    //Respawn -> after a certain amount of time

    public AiNode? AiTarget { get; private set; } = initialTarget;
    
    SanicBall ball = null!;

    public void Initialise(SanicBall parent)
    {
        ball = parent;
        parent.Name = $"{parent.character.Name} (AI)";
    }

    public void Process(double delta)
    {
        Vector3 displacement = AiTarget.Position - ball.Position;
        displacement = displacement.Normalized();

        Vector3 torque = displacement.Cross(ball.UpOverride);

        torque *= ball.character.AngularAcceleration;

        ball.ApplyTorque(torque);
    }

    public void AiNodePassed(AiNode node)
    {
        if (AiTarget?.NextNode == node)
        {
            AiTarget = node;
        }
    }
}