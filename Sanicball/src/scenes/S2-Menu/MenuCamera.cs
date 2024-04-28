using System.Linq;
using Godot;

namespace Sanicball.Scenes;

/// <summary>
/// Responsible for the ball animations in the background
/// </summary>
public partial class MenuCamera : Camera3D
{
    [Export] private float speed = 20;

    [Export] private MeshInstance3D ballMesh = null!;
    [Export] private WorldEnvironment? lightingEnv;

    public override void _Ready()
    {
        Vector3 ballPosition = ballMesh.Position;

        Tween animation = CreateTween();
        animation.SetLoops();

        var paths = GetChildren().OfType<MenuPath>();
        foreach (MenuPath path in paths)
        {
            animation.TweenCallback(Callable.From(() => ballMesh.MaterialOverride = path.CharacterMat));
            animation.TweenCallback(Callable.From(() => Position = path.Start));

            Vector3 target = path.End.MoveToward(path.Start, speed);
            float time = path.Start.DistanceTo(target) / speed;

            animation.SetParallel(true);
            animation.TweenProperty(this, ":position", target, time);
            animation.TweenMethod(Callable.From<Vector3>(_ => LookAt(ballPosition)), ballPosition, ballPosition, time);
            animation.SetParallel(false);

            if (lightingEnv is not null)
            {
                animation.TweenProperty(lightingEnv.CameraAttributes, ":exposure_multiplier", 0, time);
            }

            animation.SetParallel(true);
            animation.TweenProperty(this, ":position", path.End, Position.DistanceTo(path.End) / speed);
            animation.TweenMethod(Callable.From<Vector3>(_ => LookAt(ballPosition)), ballPosition, ballPosition, Position.DistanceTo(path.End) / speed);
            animation.SetParallel(false);

            if (lightingEnv is not null)
            {
                animation.TweenProperty(lightingEnv.CameraAttributes, ":exposure_multiplier", 1, time);
            }
        }
    }
}
