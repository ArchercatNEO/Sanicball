using Godot;

namespace Sanicball.Ball;

public class Ball<TController> where TController : IBallController
{
    public static readonly PackedScene defaultChar = GD.Load<PackedScene>("res://characters/C00-Default/Unknown.tres");
    
    public static RigidBody3D Create(TController controller, PackedScene? character = null)
    {
        character ??= defaultChar;
        RigidBody3D ball = character.Instantiate<RigidBody3D>();
        return ball;
    }

    private TController controller = default!;
}

public interface IBallController
{

}

public class FrozenController : IBallController
{

}