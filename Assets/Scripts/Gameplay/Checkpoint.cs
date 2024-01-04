using Sanicball.Logic;
using Sanicball.UI;
using UnityEngine;

namespace Sanicball.Gameplay
{
    public class Checkpoint : MonoBehaviour
    {
        //Data object to hold and hide things that are mostly not changed
        [SerializeField] private CheckpointData data;
        [SerializeField] private AINode firstAINode = null;

        public AINode FirstAINode { get { return firstAINode; } }

        /* public Marker ActivateMarker()
        {

        } */

        public void Show()
        {
            GetComponent<Renderer>().material = data.matShown;
            data.checkpointMinimap.material.mainTexture = data.texMinimapShown;
        }

        public void Hide()
        {
            GetComponent<Renderer>().material = data.matHidden;
            data.checkpointMinimap.material.mainTexture = data.texMinimapHidden;
        }

        public Vector3 GetRespawnPoint()
        {
            Vector3 origin = transform.position + Vector3.up * 100;
            return Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 200, data.ballSpawningMask) switch
            {
                true => hit.point,
                false => transform.position
            };
        }

        private void Start()
        {
            Hide();
        }

        private void OnDrawGizmos()
        {
            if (firstAINode is not null)
            {
                Gizmos.color = new Color(0.3f, 0.8f, 1f);
                Gizmos.DrawLine(transform.position, firstAINode.transform.position);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, GetRespawnPoint());
            Gizmos.DrawSphere(GetRespawnPoint(), 3);
        }
    }

    //TODO Make it static
    [System.Serializable]
    public class CheckpointData
    {
        public Renderer checkpointMinimap;

        public Material matShown;
        public Material matHidden;

        public Texture2D texMinimapShown;
        public Texture2D texMinimapHidden;

        public LayerMask ballSpawningMask;
    }

    [System.Serializable]
    public class CheckpointToAIPathConnection
    {
        [SerializeField] private string name;
        [SerializeField] private AINode firstNode;
        [SerializeField] private float selectionWeight = 1f;
        [SerializeField] private bool usedByBig = true;

        public AINode FirstNode { get { return firstNode; } }
        public float SelectionWeight { get { return selectionWeight; } }
        public bool UsedByBig { get { return usedByBig; } }
    }

    public class CheckpointStorer
    {
        public int pointer = 0;
        public int Length { get => StageReferences.Checkpoints.Length; }
        public Checkpoint Current { get => StageReferences.Checkpoints[pointer]; }
        public Checkpoint Next { get => StageReferences.Checkpoints[(pointer + 1) % Length]; }

        public void TryPassCheckpoint(Checkpoint checkpoint)
        {
            if (checkpoint != Next) return;
        }
    }
}