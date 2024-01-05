using Godot;
using System;
using Sanicball.Characters;
using Godot.Collections;

namespace Sanicball.Scenes;

public partial class MenuCamera : Camera3D
{
    [Export]
    public StaticBody3D ball;
    private Vector3 ballPosition = Vector3.Zero;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AnimationPlayer animator = GetNode<AnimationPlayer>("AnimationPlayer");
        Array<Node> paths = GetNode<Node3D>("CameraPaths").GetChildren();

        ballPosition = ball.Position;
        MeshInstance3D ballMesh = ball.GetNode<MeshInstance3D>($"MeshInstance3D");

        Animation animation = new();

        int posIndex = animation.AddTrack(Animation.TrackType.Position3D);
        animation.TrackSetPath(posIndex, GetPath());

        //int matChangeIndex = animation.AddTrack(Animation.TrackType.Method);
        foreach (Node path in paths)
        {
            GD.Print(path.GetMeta("Start"));
            GD.Print(path.GetMeta("End"));
            animation.PositionTrackInsertKey(posIndex, 0, (Vector3)path.GetMeta("Start"));
            animation.PositionTrackInsertKey(posIndex, 3, (Vector3)path.GetMeta("End"));
            break;
        };

        AnimationLibrary lib = new();
        lib.AddAnimation("characterCycle", animation);

        animator.AddAnimationLibrary("menulib", lib);
        animation.LoopMode = Animation.LoopModeEnum.Pingpong;
        animator.Play("menulib/characterCycle");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Translate(new(Random.Shared.NextInt64(), Random.Shared.NextInt64(), Random.Shared.NextInt64()));
        GD.Print(GlobalPosition);
        //LookAt(ballPosition);
    }
}
