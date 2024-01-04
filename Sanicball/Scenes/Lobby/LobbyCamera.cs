using Godot;
using Sanicball.Ball;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sanicball.Scenes;

public partial class LobbyCamera : Camera3D
{
	private static LobbyCamera? Instance = null;

	//Since vectors are passed by value we must instead poll the rotation to stay updated
	public static Func<Vector3>? TrySubscribe(AbstractBall ball)
	{
		if (Instance is null)
		{
			//? Consider adding checks to subscribers instead
			return null;
		}

		Instance.balls.Add(ball);
		ball.TreeExited += () => { Instance.balls.Remove(ball); };
		
		return () => Instance.Rotation;
	}

	private readonly List<AbstractBall> balls = new();
	private readonly Vector3 originRotation;

	private LobbyCamera()
	{
		originRotation = Rotation;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (balls.Count == 0)
		{
			Rotation = Rotation.Lerp(originRotation, (float)delta);
			return;
		}

		//? Rotate towards the average position of every player
		//! I have 0 confidence in this math
		Vector3 sum = balls.Aggregate(Vector3.Zero, (accum, ball) => accum += ball.Position);
		Vector3 meanPosition = sum / balls.Count;
		Vector3 relativePosition = meanPosition - Position;

		Vector3 cameraForwards = Quaternion.FromEuler(Rotation) * Vector3.Forward;

		Vector3 normal = cameraForwards.Cross(relativePosition).Normalized();
		if (normal == Vector3.Zero) { return; }
		
		float angle = Mathf.Asin(normal.Length() / (relativePosition.Length() * cameraForwards.Length()));
		
		Rotate(normal, angle);
	}

    public override void _ExitTree()
    {
        base._ExitTree();

		Instance = null;
    }
}
