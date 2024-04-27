using Sanicball.Characters;

namespace Sanicball.Ball;

/// <summary>
/// Everything neccesary to reconstruct a SanicBall
/// </summary>
/// <param name="Character">The character that was selected</param>
/// <param name="Controller">The controller for the ball (player/ai/remote)</param>
public record SanicBallDescriptor(SanicCharacter Character, ISanicController Controller)
{

}