using Godot;

namespace Sanicball.GameMechanics;

[GodotClass]
public partial class Checkpoint : Area3D, ICollidableEmmitter
{
    [BindProperty]
    public bool isFinishLine = false;

    [BindProperty]
    public Checkpoint next = null!;

    public int Layer { get; } = 10;

    protected override void _Ready()
    {
        BodyEntered += body =>
        {
            if (body is ICollidableReciever<Checkpoint> reciever)
            {
                reciever.OnCollision(this);
            }
        };
    }
}
