using System;
using System.Collections;
using UnityEngine;

namespace Sanicball.Gameplay
{
    public class LobbyBallSpawner : MonoBehaviour
    {
        private static LobbyBallSpawner? Instance;

        public static Rigidbody SpawnBall()
        {
            if (Instance is null)
            {
                Debug.LogError("LobbySpawner instance not found, cannot spawn a ball");
                throw new NullReferenceException();
            }

            Instance.StartCoroutine(Instance.Activate());

            return Instantiate(PrefabFactory.ballPrefab, Instance.transform.position, Instance.transform.rotation);
        }

        public const float moveDistance = 2f;
        public const float moveTime = 0.5f;

        private Vector3 topPosition;

        private IEnumerator Activate()
        {
            transform.position = topPosition;
            for (float time = 0; time < moveTime; time += Time.deltaTime)
            {
                transform.position += Vector3.up * moveDistance / moveTime;
                yield return new WaitForEndOfFrame();
            }
        }

        private void Start()
        { 
            Instance = this;
            topPosition = transform.position;
        }

        private void OnDestroy() { Instance = null; }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}
