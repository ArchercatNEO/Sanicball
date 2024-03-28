using Godot;
using Sanicball.Ball;
using Sanicball.Characters;

namespace Sanicball.Scenes;

public partial class LobbySpawner : StaticBody3D
{
    public static LobbySpawner? Instance { get; private set; }
    public override void _EnterTree() { Instance = this; }
    public override void _ExitTree() { Instance = null; }

    public SanicBall SpawnBall()
    {
        return new();
    }
}