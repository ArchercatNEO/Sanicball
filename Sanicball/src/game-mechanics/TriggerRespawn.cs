using Godot;
using Sanicball.Characters;

namespace Sanicball.GameMechanics;

[GodotClass]
public partial class TriggerRespawn : Area3D
{
    public const int layer = 10;

    protected override void _Ready()
    {
        SetCollisionLayerValue(layer, true);
        BodyEntered += (body) =>
        {
            if (body is Character ball)
            {
                ball.OnRespawn();
            }
        };
    }
}
