using System.Linq;
using Godot;

namespace Sanicball.Scenes;

public partial class MenuCamera : Camera3D
{
    [Export] private float speed = 20;
    [Export] private MeshInstance3D ballMesh = null!;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Vector3 ballPosition = ballMesh.Position;

        Tween animation = GetTree().CreateTween();
        animation.BindNode(this);
        animation.SetLoops();

        var paths = GetChildren().Cast<MenuPath>();
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

            //animation.TweenCallback(Callable.From(Sanicball.Environment.FadeOut));

            animation.SetParallel(true);
            animation.TweenProperty(this, ":position", path.End, Position.DistanceTo(path.End) / speed);
            animation.TweenMethod(Callable.From<Vector3>(_ => LookAt(ballPosition)), ballPosition, ballPosition, Position.DistanceTo(path.End) / speed);
            animation.SetParallel(false);

            //animation.TweenCallback(Callable.From(Sanicball.Environment.FadeIn));
        }
    }
}
