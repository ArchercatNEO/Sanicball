using UnityEngine;
using Sanicball.Gameplay;

namespace Sanicball.Logic
{
    public class StageReferences : MonoBehaviour
    {
        private static StageReferences? Instance;
        private void Awake() { Instance = this; }
        private void OnDestroy() { Instance = null; }

        [SerializeField] private Checkpoint[] checkpoints;
        public static Checkpoint[]? Checkpoints
        {
            get
            {
                if (Instance is null)
                {
                    Debug.LogError("No stage referece active");
                    return null;
                }

                return Instance.checkpoints;
            }
        }

        [SerializeField] private CameraOrientation[] waitingCameraOrientations;
        public static CameraOrientation[]? WaitingCameraOrientations
        {
            get
            {
                if (Instance is null)
                {
                    Debug.LogError("No stage referece active");
                    return null;
                }

                return Instance.waitingCameraOrientations;
            }
        }
    }
}