using Godot;
using Sanicball.Ball;
using Sanicball.Characters;

namespace Sanicball.Scenes;

public partial class LobbySpawner : StaticBody3D
{
    public static LobbySpawner? Instance { get; private set; }

    public static SanicCharacter? SpawnBall()
    {
        if (Instance is null)
        {
            return null;
        }

        SanicCharacter ball = SanicCharacter.Sanic.Instantiate<SanicCharacter>();
        Instance.AddChild(ball);
        ball.Translate(new(0, 5, 0));
        ball.ApplyImpulse(new(0, 10, 0));
        return ball;
    }

    public override void _Ready()
    {
        Instance = this;
    }

    public override void _ExitTree()
    {
        Instance = null;
    }

}