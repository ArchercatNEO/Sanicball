using Godot;

namespace Sanicball.Characters;

public interface ISanicController
{
    ForceRequest InputTransformer(InputEvent @event);
}

public readonly struct ForceRequest(Vector3 force)
{
    public readonly Vector3 Force = force;
}