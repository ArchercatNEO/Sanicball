using Godot;

namespace Sanicball.GameMechanics;

[GlobalClass]
public partial class Checkpoint : Area3D
{
    [Export] public bool isFinishLine = false;
    [Export] public Checkpoint next = null!;

    public override void _Ready()
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
