using System;
using Godot;
using Godot.Collections;
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
    public const int MaxSpeed = 80;
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

    [Export] public Camera3D Camera { get; private set; } = null!;
    [Export] private MeshInstance3D Renderer = null!;
    [Export] private CollisionShape3D Collider = null!;

    /// <summary>
    /// The last recorded normal relative to the floor. Neccesary for things like loops to work properly
    /// </summary>
    public Vector3 UpOverride { get; private set; } = Vector3.Up;
    public bool IsGrounded { get; private set; } = false;

    /// <summary>
    /// Emmited when the ball passes through a checkpoint
    /// </summary>
    public event EventHandler<int>? CurrentLapChanged;
    private int _currentLap = 0;
    public int CurrentLap
    {
        get => _currentLap;
        set
        {
            _currentLap = value;
            CurrentLapChanged?.Invoke(this, value);
        }
    }
    

    private ISanicController controller = new PlayerBall() { ControlType = Account.ControlType.Keyboard };
    public SanicCharacter character = SanicCharacter.Unknown;

    private Checkpoint currentCheckpoint = null!;
    private Checkpoint nextCheckpoint = null!;

    public override void _Ready()
    {
        controller.Initialise(this);

        SetCollisionMaskValue(TriggerRespawn.layer, true);
    }

    public override void _Process(double delta)
    {
        controller.Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        Array<Node3D> collisions = GetCollidingBodies();
        
        //TODO better floor check
        //? How can we detect a loop vs a wall?
        if (collisions.Count == 0)
        {
            IsGrounded = false;
        }
        else
        {
            IsGrounded = true;
            //TODO get a normal somehow to use as up
        }
    }


    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        if (state.AngularVelocity.Length() > MaxSpeed)
        {
            state.AngularVelocity = state.AngularVelocity.Normalized() * MaxSpeed;
        }
    }

    public void OnCheckpointCollision(Checkpoint checkpoint)
    {
        if (checkpoint == nextCheckpoint)
        {
            currentCheckpoint = checkpoint;
            nextCheckpoint = checkpoint.next;

            if (currentCheckpoint.isFinishLine)
            {
                CurrentLap++;
            }
        }
    }

    public void OnRespawn()
    {
        AngularVelocity = new Vector3(0, 0, 0);
        LinearVelocity = new Vector3(0, 0, 0);
        Position = currentCheckpoint.Position + new Vector3(0, 10, 0);
    }

    public void ActivateLobby()
    {

    }

    //TODO Add UI and stuff
    public void ActivateRace(Checkpoint finishLine)
    {
        currentCheckpoint = finishLine;
        nextCheckpoint = finishLine.next;

        ObjectMarker checkpointMarker = ObjectMarker.Create(Camera, nextCheckpoint, new Color(0, 0, 0, 0));
        AddChild(checkpointMarker);

        BallUI uiOverlay = BallUI.Create(this);
        AddChild(uiOverlay);

        controller.ActivateRace();
    }
}