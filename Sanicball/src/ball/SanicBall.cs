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
public partial class SanicBall : RigidBody3D, ICheckpointReciever
{
    public const int MaxSpeed = 80;

    private static readonly AudioStreamMP3 jump = GD.Load<AudioStreamMP3>("res://src/ball/S1-Sound/jump.mp3");
    private static readonly AudioStreamWav roll = GD.Load<AudioStreamWav>("res://src/ball/S1-Sound/rolling.wav");
    private static readonly AudioStreamWav speed = GD.Load<AudioStreamWav>("res://src/ball/S1-Sound/speednoise.wav");
    private static readonly AudioStreamWav brake = GD.Load<AudioStreamWav>("res://src/ball/S1-Sound/brake.wav");

    /// <summary>
    /// Contains: the ball, a renderer and a collider
    /// </summary>
    private static readonly PackedScene lobbyPrefab = GD.Load<PackedScene>("res://src/ball/LobbyBall.tscn");

    public static SanicBall CreateLobby(SanicCharacter character, ISanicController controller)
    {
        SanicBall instance = lobbyPrefab.Instantiate<SanicBall>();
        instance.controller = controller;
        instance.character = character;

        return instance;
    }

    /// <summary>
    /// Contains everything lobbyPrefab contains as well as a viewport + container and a camera
    /// </summary>
    private static readonly PackedScene racePrefab = GD.Load<PackedScene>("res://src/ball/RaceBall.tscn");
    
    /// <summary>
    /// Create a Sanicball from the race prefab with a viewport and associated camera
    /// </summary>
    /// <param name="character">The character that will be assigned to the ball</param>
    /// <param name="controller">The controller that will move the ball (player/ai/remote)</param>
    /// <param name="finishLine">As all stages are a loop finish line will be used as the stating checkpoint</param>
    /// <returns></returns>
    public static SanicBall CreateRace(SanicCharacter character, ISanicController controller, CheckpointReciever reciever)
    {
        //Making the root a container makes split screen easier
        Node viewportContainer = racePrefab.Instantiate();

        SanicBall instance = viewportContainer.GetNode<SanicBall>("SubViewport/Ball");
        instance.controller = controller;
        instance.character = character;

        instance.checkpointReciever = reciever;

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
    public CheckpointReciever? checkpointReciever = null;

    public CheckpointReciever? GetReciever()
    {
        return checkpointReciever;
    }

    private ISanicController controller = new PlayerBall() { ControlType = Account.ControlType.Keyboard };
    public SanicCharacter character = SanicCharacter.Unknown;
    
    /// <summary>
    /// The last recorded normal relative to the floor. Neccesary for things like loops to work properly
    /// </summary>
    public Vector3 UpOverride { get; private set; } = Vector3.Up;
    public bool IsGrounded { get; private set; } = false;

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

        Label counter = new();
        AddChild(counter);
        checkpointReciever?.AddCounter(counter);

        ObjectMarker? checkpointMarker = checkpointReciever?.AddTracking(Camera);
        if (checkpointMarker != null)
        {
            AddChild(checkpointMarker);
        }

        controller.Initialise(this);

        SetCollisionMaskValue(TriggerRespawn.layer, true);
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

        controller.Process(delta);
    }

    //TODO use character stats instead of a const
    public override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        if (state.AngularVelocity.Length() > character.MaxAngularSpeed)
        {
            state.AngularVelocity = state.AngularVelocity.Normalized() * character.MaxAngularSpeed;
        }
    }

    public void OnRespawn()
    {
        AngularVelocity = new Vector3(0, 0, 0);
        LinearVelocity = new Vector3(0, 0, 0);

        //TODO set respawn as lobby floor during lobby
        //Lobby -> lobby spawner
        //Race -> last checkpoint
        //Race after finished -> finish line
        Position = checkpointReciever.CurrentCheckpoint.Position + new Vector3(0, 1, 0);
    }
}