using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using Sanicball.Characters;

namespace Sanicball.Scenes;

public partial class MenuCamera : Camera3D
{
    [Export] private StaticBody3D ball = null!;

    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        await Animate();
    }

    private async Task Animate()
    {

        Vector3 ballPosition = ball.Position;
        MeshInstance3D ballMesh = ball.GetNode<MeshInstance3D>("MeshInstance3D");

        var paths = GetNode<Node3D>("CameraPaths").GetChildren().Cast<MenuPath>();

        const float speed = 20; // m/s
        int fps = Engine.PhysicsTicksPerSecond;
        float distancePerTick = speed / fps;

        while (true)
        {
            foreach (MenuPath path in paths)
            {
                ballMesh.MaterialOverride = path.CharacterMat;
                Position = path.Start;

                

                float distance = path.Start.DistanceTo(path.End);
                for (; distance > speed; distance -= distancePerTick)
                {
                    Position = Position.MoveToward(path.End, distancePerTick);
                    LookAt(ballPosition);
                    await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
                }
                
                Sanicball.Environment.FadeOut();

                for (; distance > 0; distance -= distancePerTick)
                {
                    Position = Position.MoveToward(path.End, distancePerTick);
                    LookAt(ballPosition);
                    await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
                }

                Sanicball.Environment.FadeIn();
            }
        }
    }
}
