using System;
using Godot;
using Godot.Collections;
using Sanicball.Account;

namespace Sanicball.Characters;

//TODO: when/if discriminated unions come out use then instead
public interface IController { };

public class PlayerController : IController
{
    public ControlType ControlType { get; init; }
}

public class AiController : IController
{
    public required AiNode InitialNode { get; init;}
}

/// <summary>
/// An AI navigation Node
/// </summary>
//TODO: Tool debug things
[GodotClass]
public partial class AiNode : Area3D
{
    [BindProperty] private GodotArray<AiNode> nextOptions = [];
    //TODO Weights

    public AiNode NextNode
    {
        get
        {
            double rng = Random.Shared.NextDouble();

            double totalWeight = 0;
            int index;
            for (index = 0; index < nextOptions.Count; index++)
            {
                //Should be the weight value from the weight array
                double weight = 1 / nextOptions.Count;

                if (totalWeight < rng && rng < totalWeight + weight)
                {
                    break;
                }

                totalWeight += weight;
            }

            return nextOptions[index];
        }
    }

    protected override void _Ready()
    {
        BodyEntered += (body) =>
        {
            if (body is Character ball)
            {
                //ball.AiNodePassed(this);
            }
        };
    }
}


public class RemoteController : IController
{
}

/// <summary>
/// A movement object sent by remote balls for online matches
/// </summary>
public class RemoteMovement
{

}
