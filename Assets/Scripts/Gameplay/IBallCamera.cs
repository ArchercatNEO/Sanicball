using UnityEngine;
using Sanicball.Data;

namespace Sanicball.Gameplay
{
    public interface IBallCamera
    {
        void SetDirection(Quaternion dir);

        void Remove();

        Quaternion RotateCamera(Rigidbody target);

        public static IBallCamera Create(ControlType ctrlType)
        {
            if (GameSettings.Instance.useOldControls) return PivotCamera.Create(ctrlType);
            return OmniCamera.Create(ctrlType);
        }
    }
}