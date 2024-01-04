using System.Collections.Generic;
using UnityEngine;

namespace Sanicball
{
    public class AINodeSplitter : AINode
    {
        [SerializeField] private AINodeSplitterTarget[] targets = {};
        private readonly List<AINodeSplitterTarget> weighted = new();
        
        private AINodeSplitter()
        {
            //Pick a random next node based on their weights
            foreach (AINodeSplitterTarget node in targets)
                for (int j = 0; j < node.Weight; j++) 
                    weighted.Add(node);
        }

        public override AINode NextNode
        {
            get
            {
                int randomChoice = Random.Range(0, weighted.Count);
                return weighted[randomChoice].Node;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 3f);

            foreach (AINodeSplitterTarget target in targets)
            {
                if (target.Node is null) continue;
                Gizmos.DrawLine(transform.position, target.Node.transform.position);
            }
        }
    }

    [System.Serializable]
    public class AINodeSplitterTarget
    {
        [SerializeField] private AINode? node = null;
        [SerializeField] private int weight = 1;

        public AINode? Node { get { return node; } }
        public int Weight { get { return weight; } }
    }
}