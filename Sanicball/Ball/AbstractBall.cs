using Godot;
using Sanicball.Characters;

namespace Sanicball.Ball;

public interface IBall
{
	public static abstract CSharpScript AsScript { get; }
}

public abstract partial class AbstractBall : RigidBody3D
{
	private static readonly PackedScene prefab = GD.Load<PackedScene>("res://Ball/Ball.tscn");
	public static T Create<T>(SanicCharacter character) where T : AbstractBall, IBall
	{
		RigidBody3D ball = prefab.Instantiate<RigidBody3D>();
		ulong id = ball.GetInstanceId();
		ball.SetScript(T.AsScript);
		
		return (T)InstanceFromId(id);
	}
	
	private static readonly AudioStreamMP3 jump = GD.Load<AudioStreamMP3>("res://Ball/Sfx/jump.mp3");
	private static readonly AudioStreamWav roll = GD.Load<AudioStreamWav>("res://Ball/Sfx/rolling.wav");
	private static readonly AudioStreamWav speed = GD.Load<AudioStreamWav>("res://Ball/Sfx/speednoise.wav");
	private static readonly AudioStreamWav brake = GD.Load<AudioStreamWav>("res://Ball/Sfx/brake.wav");

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
