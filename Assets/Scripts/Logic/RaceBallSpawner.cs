using System;
using Sanicball.Data;
using Sanicball.Gameplay;
using SanicballCore;
using UnityEngine;

namespace Sanicball.Logic
{
    public class RaceBallSpawner : MonoBehaviour
    {
        //These two are only used for visualizing spawn positions in the editor
        [SerializeField] private int editorBallCount = 8;
        [SerializeField] private float editorBallSize = 1;

        [SerializeField] private int columns = 10;
        [SerializeField] private LayerMask ballSpawningMask = new();

        private static RaceBallSpawner? Instance;
        public static Rigidbody SpawnBall(float charSize, int position)
        {
            if (Instance is null)
            {
                Debug.LogError("RaceSpawner instance not found, cannot spawn a ball");
                throw new NullReferenceException();
            }

            Vector3 spot = Instance.GetSpawnPoint(position, charSize / 2f);

            return Instantiate(PrefabFactory.ballPrefab, spot, Instance.transform.rotation);
        }

        public Vector3 GetSpawnPoint(int position, float offsetY)
        {
            //Get the row of the ball
            int row = position / columns;

            Vector3 dir = position % 2 == 0 ? Vector3.right : Vector3.left;
            dir *= (position % columns / 2 + 0.5f) * 2;
            dir += Vector3.back * 2f * row;

            Vector3 origin = transform.TransformPoint(dir + Vector3.up * 100);
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 200, ballSpawningMask))
            {
                dir = transform.InverseTransformPoint(hit.point);
            }
            return transform.TransformPoint(dir) + Vector3.up * offsetY;
        }

        private void Start()
        {
            Instance = this;
            //Disable the arrow gizmo
            GetComponent<Renderer>().enabled = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.2f, 0.6f, 1);
            columns = Mathf.Max(1, columns);
            for (int i = 0; i < editorBallCount; i++)
            {
                Gizmos.DrawSphere(GetSpawnPoint(i, editorBallSize / 2f), editorBallSize / 2f);
            }
        }
    }
}