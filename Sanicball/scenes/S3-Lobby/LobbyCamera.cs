using Godot;
using Sanicball.Ball;
using Sanicball.Characters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sanicball.Scenes;

/// <summary>
/// Looks towards the mean position of all players in the lobby.
/// If there are no players it will default to wherever it was looking when it was instantiated
/// </summary>
//TODO moving around enough causes the camera to flip, avoid that
public partial class LobbyCamera : Camera3D
{
    public static LobbyCamera? Instance { get; private set; }
    public override void _EnterTree() { Instance = this; }
    public override void _ExitTree() { Instance = null; }


    [Export] private float rotationSpeed = 10;

    private readonly List<SanicBall> balls = [];
    private readonly Vector3 originRotation;

    private LobbyCamera()
    {
        originRotation = Rotation;
    }

    //Since vectors are passed by value we must instead poll the rotation to stay updated
    public Func<Vector3> Subscribe(SanicBall ball)
    {
        balls.Add(ball);
        ball.TreeExited += () => { balls.Remove(ball); };
        return () => Rotation;
    }

    public override void _Process(double delta)
    {
        //Prevent a div 0 by returning to starting location
        if (balls.Count == 0)
        {
            Rotation = Rotation.Lerp(originRotation, (float)delta);
            return;
        }

        //? Rotate towards the meaned position of every player
        Vector3 sum = balls.Aggregate(Vector3.Zero, (accum, ball) => accum += ball.Position);
        Vector3 meanPosition = sum / balls.Count;
        Vector3 relativePosition = meanPosition - Position;

        Vector3 cameraForwards = Quaternion.FromEuler(Rotation) * Vector3.Forward;

        Vector3 normal = cameraForwards.Cross(relativePosition).Normalized();
        if (normal == Vector3.Zero) { return; }

        float angle = cameraForwards.AngleTo(relativePosition);

        Rotate(normal, angle * (float)delta * rotationSpeed);
    }
}
