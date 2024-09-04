using System;
using Godot;

namespace Sanicball.Ball;

/// <summary>
/// An AI navigation Node
/// </summary>
//TODO Tool debug things

public partial class AiNode : Area3D
{   
    [BindProperty] private AiNode[] nextOptions = [];
    //TODO Weights

    public AiNode NextNode
    {
        get
        {
            double rng = Random.Shared.NextDouble();
            
            double totalWeight = 0;
            int index;
            for (index = 0; index < nextOptions.Length; index++)
            {
                //Should be the weight value from the weight array
                double weight = 1 / nextOptions.Length;
                
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
            if (body is SanicBall ball)
            {
                ball.AiNodePassed(this);
            }
        };
    }
}