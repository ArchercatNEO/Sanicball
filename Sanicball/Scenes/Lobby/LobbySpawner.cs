using Godot;
using Sanicball.Ball;
using Sanicball.Characters;

namespace Sanicball.Scenes;

public partial class LobbySpawner : StaticBody3D
{
    public override void _Ready()
    {
        base._Ready();

        PlayerBall ball = AbstractBall.Create<PlayerBall>(SanicCharacter.Sanic);
        AddChild(ball);
        ball.Translate(new(0, 5, 0));
        ball.ApplyImpulse(new(0, 10, 0));
    }
}