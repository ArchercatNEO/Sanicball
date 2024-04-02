using System;
using Godot;
using Sanicball.Characters;
using Sanicball.GameMechanics;

namespace Sanicball.Ball;

/// <summary>
/// The true type of a Ball.
/// Contains its controller (ai/player/remote)
/// Contains its character and associated personality
/// </summary>
public partial class SanicBall : RigidBody3D
{
    public const int MaxSpeed = 1000;
    public const int InputAcceleration = 50;

    private static readonly AudioStreamMP3 jump = GD.Load<AudioStreamMP3>("res://ball/S1-Sound/jump.mp3");
    private static readonly AudioStreamWav roll = GD.Load<AudioStreamWav>("res://ball/S1-Sound/rolling.wav");
    private static readonly AudioStreamWav speed = GD.Load<AudioStreamWav>("res://ball/S1-Sound/speednoise.wav");
    private static readonly AudioStreamWav brake = GD.Load<AudioStreamWav>("res://ball/S1-Sound/brake.wav");

    private static readonly PackedScene prefab = GD.Load<PackedScene>("res://ball/Ball.tscn");

    public static SanicBall Create(SanicCharacter character, ISanicController controller)
    {
        SanicBall ball = prefab.Instantiate<SanicBall>();
        ball.controller = controller;
        ball.character = character;
        
        ball.Renderer ??= ball.GetNode<MeshInstance3D>("Renderer");
        ball.Renderer.MaterialOverride = character.Material;
        if (character.MeshOverride is not null)
        {
            ball.Renderer.Mesh = character.MeshOverride;
        }
        
        ball.Collider ??= ball.GetNode<CollisionShape3D>("Collider");
        if (character.CollisionOverride is not null)
        {
            ball.Collider.Shape = character.CollisionOverride;
        }

        return ball;
    }

    public event EventHandler<TriggerRespawn>? OnRespawn;

    [Export] private MeshInstance3D Renderer = null!;
    [Export] private CollisionShape3D Collider = null!;

    private ISanicController controller = new PlayerBall();
    private SanicCharacter character = SanicCharacter.Unknown;

    public override void _Ready()
    {
        controller.Initialise(this);
        
        SetCollisionMaskValue(TriggerRespawn.layer, true);
        BodyEntered += (body) =>
        {
            if (body is TriggerRespawn respawn)
            {
                OnRespawn?.Invoke(this, respawn);
            }
        };
    }

    public override void _Process(double delta)
    {
        controller.Process(delta);
    }


    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        if (state.LinearVelocity.Length() > MaxSpeed)
        {
            state.LinearVelocity = state.LinearVelocity.Normalized() * MaxSpeed;
        }
    }
}