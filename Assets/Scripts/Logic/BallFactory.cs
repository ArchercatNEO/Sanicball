using System;
using UnityEngine;
using Sanicball.Gameplay;
using SanicballCore;
using Sanicball.Data;

class BallFactory: MonoBehaviour
{
    [SerializeField] private GameObject origin;
    [SerializeField] private Ball ballPrefab;

    //These two are only used for visualizing spawn positions in the editor
    [SerializeField] private int editorBallCount = 8;

    [SerializeField] private float editorBallSize = 1;

    [SerializeField] private int columns = 10;

    [SerializeField] private LayerMask ballSpawningMask = new();

    public Ball SpawnBall(int position, BallType ballType, ControlType ctrlType, int character, string nickname)
    {
        float characterSize = ActiveData.Characters[character].ballSize;

        return SpawnBall(GetSpawnPoint(position, characterSize / 2f), transform.rotation, ballType, ctrlType, character, nickname);
    }

    public Vector3 GetSpawnPoint(int position, float offsetY)
    {
        //Get the row of the ball
        int row = position / columns;

        Vector3 dir = position % 2 == 0 
            ? Vector3.right * (position % columns / 2 + 0.5f) * 2
            :Vector3.left * (position % columns / 2 + 0.5f) * 2;

        dir += Vector3.back * 2f * row;

            RaycastHit hit;
            if (Physics.Raycast(transform.TransformPoint(dir + Vector3.up * 100), Vector3.down, out hit, 200, ballSpawningMask))
            {
                dir = transform.InverseTransformPoint(hit.point);
            }
        return transform.TransformPoint(dir) + Vector3.up * offsetY;
    }

        private void Start()
        {
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

    public Ball SpawnBall(Vector3 position, Quaternion rotation, BallType ballType, ControlType ctrlType, int character, string nickname)
        {
            Ball ball = Instantiate(ballPrefab, position, rotation);
            ball.Init(ballType, ctrlType, character, nickname);

            return ball;
        }
}
