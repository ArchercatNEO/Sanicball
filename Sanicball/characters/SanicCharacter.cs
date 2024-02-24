using System;
using Godot;
using Sanicball.GameMechanics;

namespace Sanicball.Characters;

public partial class SanicCharacter : RigidBody3D, IRespawnable
{
    #region Editor-level dependencies
    //Useful metadata
    [Export] public required string name;
    [Export] public required string credits;

    //Colours and renderers
    [Export] public required Color color;
    [Export] public required Texture icon;
    [Export] public required Texture minimapIcon;

    //Overwrite physics stats (big, bee)
    [Export] public float rollSpeed = 100;
    [Export] public float airSpeed = 15;
    [Export] public float jumpHeight = 14;
    [Export] public float grip = 20;
    [Export] public float ballSize = 1;
    #endregion Editor-level dependencies

    #region Static data
    public const int MaxSpeed = 1000;
    public const int InputAcceleration = 50;

    private static readonly AudioStreamMP3 jump = GD.Load<AudioStreamMP3>("res://Ball/sfx/jump.mp3");
    private static readonly AudioStreamWav roll = GD.Load<AudioStreamWav>("res://Ball/sfx/rolling.wav");
    private static readonly AudioStreamWav speed = GD.Load<AudioStreamWav>("res://Ball/sfx/speednoise.wav");
    private static readonly AudioStreamWav brake = GD.Load<AudioStreamWav>("res://Ball/sfx/brake.wav");
    #endregion Static data

    public static SanicCharacter Create<TController>(PackedScene? character = null)
    {
        character ??= Unknown;
        SanicCharacter ball = character.Instantiate<SanicCharacter>();
        return ball;
    }

    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        if (state.LinearVelocity.Length() > MaxSpeed)
        {
            state.LinearVelocity = state.LinearVelocity.Normalized() * MaxSpeed;
        }
    }

    public event EventHandler OnRespawn = (sender, args) =>
    {
        GD.Print("Character respawning");
    };

    public void Respawn()
    {
    }
}