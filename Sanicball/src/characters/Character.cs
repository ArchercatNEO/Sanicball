//Handles nodes and virtual methods

using System;
using Godot;
using Godot.Collections;
using Sanicball.Account;
using Sanicball.GameMechanics;

namespace Sanicball.Characters;

/// <summary>
/// The base type of all characters
/// Contains its controller (ai/player/remote)
/// </summary>
[GodotClass]
public partial class Character : RigidBody3D, ICollidableReciever<Checkpoint>
{
    //TODO: Implement dynamic loading instead of hardcoding the clips
    private static readonly AudioStreamMP3 jump = GD.Load<AudioStreamMP3>("res://src/ball/S1-Sound/jump.mp3");
    private static readonly AudioStreamWav roll = GD.Load<AudioStreamWav>("res://src/ball/S1-Sound/rolling.wav");
    private static readonly AudioStreamWav speed = GD.Load<AudioStreamWav>("res://src/ball/S1-Sound/speednoise.wav");
    private static readonly AudioStreamWav brake = GD.Load<AudioStreamWav>("res://src/ball/S1-Sound/brake.wav");

    public Character()
    {
        renderer = new();
        AddChild(renderer);

        collider = new();
        AddChild(collider);
    }

    public IController controller = new PlayerController() { ControlType = ControlType.Keyboard };

    #region Node refs
    
    protected readonly MeshInstance3D renderer;
    protected readonly CollisionShape3D collider;

    /// <summary>
    /// The camera this ball will use to rotate input
    /// </summary>
    /// <value>
    /// Inside the lobby it will be the global lobby camera. In a race it will be the camera attached to the ball itself
    /// </value>
    public Camera3D Camera { get; private set; } = null!;

    #endregion Node refs

    public event Action<Checkpoint>? CheckpointPassed;
    private Checkpoint currentCheckpoint;

    public void OnCollision(Checkpoint checkpoint)
    {
        if (currentCheckpoint.next == checkpoint)
        {
            currentCheckpoint = checkpoint;
            CheckpointPassed?.Invoke(checkpoint);
        }
    }

    /// <summary>
    /// The last recorded normal relative to the floor. Neccesary for things like loops to work properly
    /// </summary>
    public Vector3 UpOverride { get; private set; } = Vector3.Up;
    public bool IsGrounded { get; private set; } = false;

    protected override void _Ready()
    {
        //In the lobby this is the lobby camera
        //In a race it will be the camera attached to the ball
        Camera = GetViewport().GetCamera3D();

        SetCollisionMaskValue(TriggerRespawn.layer, true);
    }

    protected override void _PhysicsProcess(double delta)
    {
        GodotArray<Node3D> collisions = GetCollidingBodies();

        //TODO: better floor check
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

        if (controller is PlayerController player)
        {
            Vector3 force = player.ControlType.NormalizedForce();

            //Rotate force so that Vector3.Forwards == Camera.Forwards
            force = Quaternion.FromEuler(Camera.Rotation) * force;

            //Convert a linear force into the axis of rotation
            Vector3 torque = -force.Cross(UpOverride);

            torque *= AngularAcceleration;

            ApplyTorque(torque);
        }
    }

    protected override void _Input(InputEvent @event)
    { 
        if (@event is InputEventKey { Keycode: Key.Space})
        {
            ApplyImpulse(UpOverride * 10);
        }
    }

    protected override void _IntegrateForces(PhysicsDirectBodyState3D state)
    {
        if (state.AngularVelocity.Length() > MaxAngularSpeed)
        {
            state.AngularVelocity = state.AngularVelocity.Normalized() * MaxAngularSpeed;
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
        Position = currentCheckpoint.Position + new Vector3(0, 1, 0);
    }
}
