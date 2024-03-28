using Godot;

namespace Sanicball.GameMechanics;

public partial class TriggerRespawn : Area3D
{
    public const int layer = 10;

    public override void _Ready()
    {
        SetCollisionLayerValue(layer, true);
    }
}