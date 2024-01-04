using UnityEngine;

namespace Sanicball.Gameplay
{
    //TODO merge with LobbySpawner
    public class LobbyPlatform : MonoBehaviour
    {
        public float moveDistance = 2f;
        public float moveTime = 0.5f;

        private float currentPos = 0f; //1 = lowered, 0 = raised

        private Vector3 basePos;

        private static LobbyPlatform? Instance;

        public static void TryActivate()
        {
            if (Instance == null)
            {
                Debug.LogError("LobbyPlatform not found, cannot activate");
                return;
            }

            Instance.Activate();
        }

        private void Activate()
        {
            currentPos = 1f;
        }

        private void Start()
        {
            Instance = this;
            basePos = transform.position;
        }

        private void Update()
        {
            if (currentPos > 0f)
            {
                currentPos = Mathf.Max(currentPos - (Time.deltaTime / moveTime), 0);
            }
            var smoothPos = Mathf.SmoothStep(0f, 1f, currentPos);
            transform.position = basePos + new Vector3(0, -moveDistance * smoothPos, 0);

            //For debugging
            /*if (Input.GetKeyDown(KeyCode.L)) {
                Activate();
            }*/
        }
    }
}
