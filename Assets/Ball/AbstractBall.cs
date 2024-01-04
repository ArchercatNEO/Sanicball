using Godot;

namespace Sanicball.Ball;

public abstract partial class AbstractBall : RigidBody3D
{
	private static readonly AudioStreamMP3 jump = GD.Load<AudioStreamMP3>("res://Assets/Ball/Sfx/jump.mp3");
	private static readonly AudioStreamMP3 roll = GD.Load<AudioStreamMP3>("res://Assets/Ball/Sfx/rolling.wav");
	private static readonly AudioStreamMP3 speed = GD.Load<AudioStreamMP3>("res://Assets/Ball/Sfx/speednoise.wav");
	private static readonly AudioStreamMP3 brake = GD.Load<AudioStreamMP3>("res://Assets/Ball/Sfx/brake.wav");

	public const int MaxSpeed = 1000;
	public const int InputAcceleration = 50;

	public override void _IntegrateForces(PhysicsDirectBodyState3D state)
	{
		base._IntegrateForces(state);

		if (state.LinearVelocity.Length() > MaxSpeed)
		{
			state.LinearVelocity = state.LinearVelocity.Normalized() * MaxSpeed;
		}
	}
}
