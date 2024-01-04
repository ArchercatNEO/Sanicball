using UnityEngine;
using Sanicball.Logic;
using Sanicball.UI;
using Sanicball.Data;
using System.Linq;
using System.Collections.Generic;
using Sanicball.Ball;
using System.Collections;

namespace Sanicball
{
    public class EndOfMatch : MonoBehaviour
    {
        private static Scoreboard prefab => Resources.Load<Scoreboard>("Prefabs/User Interface/ScoreboardCanvas");
        private static EndOfMatch? Instance;

        [SerializeField] private Camera cam = null;
        
        [SerializeField] private Transform[] topPositionSpawnpoints = null;
        [SerializeField] private Transform lowerPositionsSpawnpoint = null;

        private Vector3 angle = Vector3.zero;

        private void Update()
        {
            transform.Rotate(angle * Time.deltaTime * 10);
        }

        public static void Activate(List<RaceFinishReport> reports)
        {
            if (Instance is null)
            {
                Debug.LogError("Failed to active end of match, handler not found");
                return;
            }

            //Activate with fade
            CameraFade.StartAlphaFade(Color.black, false, 1f, 0, () =>
            {
                CameraFade.StartAlphaFade(Color.black, true, 1f);

                Instance.cam.gameObject.SetActive(true);
                Instance.angle = new Vector3(0, 1, 0);
                Scoreboard scoreboard = Instantiate(prefab);
                foreach (RaceFinishReport report in reports)
                {
                    Instance.Spawn();
                    
                    /* Sprite icon = report.character.icon;
                    ScoreboardEntry.Create(scoreboard.entryContainer, icon, report.name, report); */
                }
            });
        }

        private IEnumerable Spawn()
        {
            for (int i = 0; true; i++)
            {
                Vector3 position = lowerPositionsSpawnpoint.position;
                if (i < topPositionSpawnpoints.Length) { position = topPositionSpawnpoints[i].position; }

                Rigidbody playerToMove = Instantiate(PrefabFactory.ballPrefab).GetComponent<Rigidbody>();
                playerToMove.transform.position = position;
                playerToMove.transform.rotation = transform.rotation;
                playerToMove.velocity = Random.insideUnitSphere * 0.5f;
                playerToMove.angularVelocity = new Vector3(0, Random.Range(-50f, 50f));
                playerToMove.gameObject.layer = LayerMask.NameToLayer("Racer");
                yield return playerToMove;
            }
        }

        private void Start() { Instance = this; }
        private void OnDestroy() { Instance = null; }
    }
}