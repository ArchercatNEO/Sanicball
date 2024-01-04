using UnityEngine;
using Sanicball.Gameplay;
using System;

namespace Sanicball.Logic
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

            //TODO Inline LobbyPlataform into this class
            LobbyPlatform.TryActivate();

            return Instantiate(PrefabFactory.ballPrefab, Instance.transform.position, Instance.transform.rotation);
        }

        private void Start()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}