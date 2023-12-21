using UnityEngine;

namespace Sanicball
{
    //TODO a linked link is just a degenerate tree -> all types of nodes can be merged
    public abstract class AINode : MonoBehaviour
    {
        public abstract AINode NextNode { get; }
    }
}