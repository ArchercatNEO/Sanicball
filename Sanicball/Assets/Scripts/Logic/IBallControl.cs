using UnityEngine;
using Sanicball.Data;

namespace Sanicball.Logic
{
    public interface IBallControl
    {
        public ControlType CtrlType { get; }

        public Vector3 CalculateDirection();

        public bool ShouldBreak();

        public bool ShouldJump();

        public bool ShouldRespawn();
    }
}