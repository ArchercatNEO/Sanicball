using Godot;
using Sanicball.Account;
using Sanicball.Characters;
using Sanicball.GameMechanics;
using Sanicball.Scenes;

namespace Sanicball.Ball;

public partial class PlayerBall : ISanicController
{
    //TODO LobbyCamera.TrySubscribe(this);

    public ForceRequest InputTransformer(InputEvent @event)
    {
        Vector3 force = ControlType.Keyboard.NormalizedForce();
        force *= SanicCharacter.InputAcceleration;

        return new(force);
    }
}
