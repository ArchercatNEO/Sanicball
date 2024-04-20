using System;
using Godot;
using Godot.Collections;
using Sanicball.Characters;
using Sanicball.GameMechanics;
using Sanicball.Scenes;

namespace Sanicball.Ball;

/// <summary>
/// The true type of a Ball.
/// Contains its controller (ai/player/remote)
/// Contains its character and associated personality
/// </summary>
public partial class SanicBall : RigidBody3D
{
    public const int MaxSpeed = 80;

    private static readonly AudioStreamMP3 jump = GD.Load<AudioStreamMP3>("res://ball/S1-Sound/jump.mp3");
    private static readonly AudioStreamWav roll = GD.Load<AudioStreamWav>("res://ball/S1-Sound/rolling.wav");
    private static readonly AudioStreamWav speed = GD.Load<AudioStreamWav>("res://ball/S1-Sound/speednoise.wav");
    private static readonly AudioStreamWav brake = GD.Load<AudioStreamWav>("res://ball/S1-Sound/brake.wav");

    private static readonly PackedScene lobbyPrefab = GD.Load<PackedScene>("res://ball/LobbyBall.tscn");
    private static readonly PackedScene racePrefab = GD.Load<PackedScene>("res://ball/RaceBall.tscn");

    public static SanicBall CreateLobby(SanicCharacter character, ISanicController controller)
    {
        SanicBall instance = lobbyPrefab.Instantiate<SanicBall>();
        instance.controller = controller;
        instance.character = character;

        LobbyCamera.Instance?.Subscribe(instance);

        return instance;
    }

    /// <summary>
    /// Create a Sanicball from the race prefab with a viewport and associated camera
    /// </summary>
    /// <param name="character">The character that will be assigned to the ball</param>
    /// <param name="controller">The controller that will move the ball (player/ai/remote)</param>
    /// <param name="finishLine">As all stages are a loop finish line will be used as the stating checkpoint</param>
    /// <returns></returns>
    public static SanicBall CreateRace(SanicCharacter character, ISanicController controller, Checkpoint finishLine)
    {
        //Making the root a container makes split screen easier
        Node viewportContainer = racePrefab.Instantiate();

        SanicBall instance = viewportContainer.GetNode<SanicBall>("SubViewport/Ball");
        instance.controller = controller;
        instance.character = character;

        instance._currentCheckpoint = finishLine;

        controller.ActivateRace();

        return instance;
    }

    [Export] private MeshInstance3D Renderer = null!;
    [Export] private CollisionShape3D Collider = null!;

    /// <summary>
    /// The camera this ball will use to rotate input
    /// </summary>
    /// <value>
    /// Inside the lobby it will be the global lobby camera. In a race it will be the camera attached to the ball itself
    /// </value>
    public Camera3D Camera { get; private set; } = null!;

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


    private ObjectMarker? checkpointMarker;
    private Checkpoint? _currentCheckpoint;
    private Checkpoint? CurrentCheckpoint
    {
        get => _currentCheckpoint;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            _currentCheckpoint = value;

            checkpointMarker?.QueueFree();
            checkpointMarker = ObjectMarker.Create(Camera, value.next, new Color(0, 0, 0, 0));
            AddChild(checkpointMarker);
        }
    }


    public override void _Ready()
    {
        Renderer ??= GetNode<MeshInstance3D>("Renderer");
        Renderer.MaterialOverride = character.Material;
        if (character.MeshOverride is not null)
        {
            Renderer.Mesh = character.MeshOverride;
        }

        Collider ??= GetNode<CollisionShape3D>("Collider");
        if (character.CollisionOverride is not null)
        {
            Collider.Shape = character.CollisionOverride;
        }

        //In the lobby this is the lobby camera
        //In a race it will be the camera attached to the ball
        Camera = GetViewport().GetCamera3D();

        //TODO fix this up
        //the marker needs a camera but we only got it now so it must be created late
        if (_currentCheckpoint is not null)
        {
            checkpointMarker = ObjectMarker.Create(Camera, _currentCheckpoint.next, new Color(0, 0, 0, 0));
            AddChild(checkpointMarker);
        }


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
        if (checkpoint == CurrentCheckpoint?.next)
        {
            CurrentCheckpoint = checkpoint;

            if (CurrentCheckpoint.isFinishLine)
            {
                CurrentLap++;
            }
        }
    }

    //TODO set respawn as lobby floor during lobby
    public void OnRespawn()
    {
        AngularVelocity = new Vector3(0, 0, 0);
        LinearVelocity = new Vector3(0, 0, 0);
        Position = CurrentCheckpoint.Position + new Vector3(0, 10, 0);
    }
}