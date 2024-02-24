using Godot;
using Sanicball.Account;
using Sanicball.Characters;
using Sanicball.GameMechanics;
using Sanicball.Scenes;

namespace Sanicball.Ball;

public partial class PlayerBall : ISanicController
{
    public void Initialise(SanicCharacter parent)
    {
        LobbyCamera.Instance?.Subscribe(parent);

        parent.OnRespawn += (sender, body) => {
            SanicCharacter character = (SanicCharacter)sender!;
            character.Position = new(0, 100, 0);
        };
    }

    //TODO 

    public ForceRequest InputTransformer(InputEvent @event)
    {
        Vector3 force = ControlType.Keyboard.NormalizedForce();
        force *= SanicCharacter.InputAcceleration;

        return new(force);
    }
}
