using Godot;
using Sanicball.Characters;

namespace Sanicball.Ball;

/// <summary>
/// A ball controlled by the computer.
/// This kind of ball is not currently supported in online mode
/// </summary>
public class AiBall : ISanicController
{
    //Settings -> AI skill adds noise to the navigation
    //Respawn -> after a certain amount of time
    readonly NavigationAgent3D navigationAgent = new();

    SanicBall ball = null!;

    public void Initialise(SanicBall parent)
    {
        ball = parent;
        ball.AddChild(navigationAgent);
        parent.Name = $"{parent.character.Name} (AI)";
    }

    public void Process(double delta)
    {
    }
}