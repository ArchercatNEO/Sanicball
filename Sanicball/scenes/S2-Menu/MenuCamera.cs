using System;
using System.Linq;
using Godot;
using Sanicball.Utils;
using Serilog;

namespace Sanicball.Scenes;

/// <summary>
/// Responsible for the ball animations in the background
/// </summary>
[GodotClass]
public partial class MenuCamera : Camera3D
{
    [BindProperty] private float speed = 20;

    [BindProperty] private MeshInstance3D? ballMesh;
    [BindProperty] private WorldEnvironment? lightingEnv;

    protected override void _Ready()
    {
        if (ballMesh == null)
        {
            Log.Warning("Camera has no ball mesh: disabling mesh switching");
        }

        if (lightingEnv == null)
        {
            Log.Warning("Camera has no WorldEnvironment: disabling fade in/out");
        }

        Tween animation = CreateTween();
        animation.SetLoops();

        var paths = this.ChildrenStream().OfType<MenuPath>();
        foreach (MenuPath path in paths)
        {
            animation.TweenCallback(Callable.From(() =>
            {
                Position = path.Start;
                if (ballMesh is not null)
                {
                    ballMesh.MaterialOverride = path.CharacterMat;
                }
            }));

            Vector3 displacement = path.End - path.Start;
            float time = displacement.Length() / speed;
            Vector3 beforeFade = (time - 1) * speed * displacement.Normalized();
            
            animation.SetParallel(true);
            animation.TweenProperty(this, new NodePath(":position"), beforeFade, time - 1);
            if (ballMesh is not null)
            {
                animation.TweenMethod(Callable.From<float>(_ => LookAt(ballMesh.Position)), 0, 0, time - 1);
            }
            if (lightingEnv is not null)
            {
                animation.TweenProperty(lightingEnv.CameraAttributes, new NodePath(":exposure_multiplier"), 1, 1);
            }
            animation.SetParallel(false);
            
            animation.SetParallel(true);
            animation.TweenProperty(this, new NodePath(":position"), path.End, 1);
            if (ballMesh is not null)
            {
                animation.TweenMethod(Callable.From<Vector3>(_ => LookAt(ballMesh.Position)), 0, 0, 1);
            }
            if (lightingEnv is not null)
            {
                animation.TweenProperty(lightingEnv.CameraAttributes, new NodePath(":exposure_multiplier"), 0, 1);
            }

            animation.SetParallel(false);
        }
    }
}
