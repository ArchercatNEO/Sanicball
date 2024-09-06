using Godot;

namespace Sanicball.GameMechanics;


[GodotClass]
public partial class Checkpoint : Area3D
{
    [BindProperty] public bool isFinishLine = false;
    [BindProperty] public Checkpoint next = null!;

    protected override void _Ready()
    {
        BodyEntered += (body) =>
        {
            if (body is ICheckpointReciever reciever)
            {
                reciever.GetReciever()?.OnCheckpointCollion(this);
            }
        };
    }
}

public interface ICheckpointReciever
{
    CheckpointReciever? GetReciever();
}
