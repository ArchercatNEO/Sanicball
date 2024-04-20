using Sanicball.Ball;

namespace Sanicball.Characters;

public interface ISanicController
{
    void Initialise(SanicBall parent);
    void Process(double delta);

    void ActivateLobby() { }
    void ActivateRace() { }
}