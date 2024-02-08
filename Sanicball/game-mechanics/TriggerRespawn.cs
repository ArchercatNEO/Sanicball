using Godot;

namespace Sanicball.GameMechanics;

internal partial class TriggerRespawn : Area3D
{
    public override void _Ready()
    {
        BodyEntered += (collider) =>
        {
            if (collider is IRespawnable respawnable)
            {
                respawnable.Respawn();
            }
        };
    }
}

public interface IRespawnable
{
    void Respawn();
}