using System;
using System.Collections.Generic;
using System.Linq;
using Sanicball.Data;
using Sanicball.Logic;
using SanicballCore;
using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    public class PlayerUI : MonoBehaviour
    {
        private static readonly Color finishedColor = new(0f, 0.5f, 1f);
        
        private static PlayerUI prefab => Resources.Load<PlayerUI>("Prefabs\\User Interface\\PlayerUI");
        public static PlayerUI Create(Camera camera, RacePlayer player)
        {
            PlayerUI instance = Instantiate(prefab);
            instance.TargetCamera = camera;
            instance.TargetPlayer = player;
            
            return instance;
        }

        //? These are set from the factory method and therefore cannot be null
        private Camera TargetCamera = null!;
        private RacePlayer TargetPlayer = null!;
        private readonly Timer timeout = new();

        [SerializeField] private RectTransform fieldContainer;
        [SerializeField] private Text speedField = null;
        [SerializeField] private Text speedFieldLabel = null;
        [SerializeField] private Text lapField = null;
        [SerializeField] private Text timeField = null;
        [SerializeField] private Text checkpointTimeField = null;
        [SerializeField] private Text checkpointTimeDiffField = null;

        [SerializeField] private AudioClip checkpointSound;
        private static AudioClip respawnSound => Resources.Load<AudioClip>("Sound\\Sfx\\respawn.wav");
        [SerializeField] private RectTransform markerContainer;

        public void Respawned()
        {
            UISound.Play(respawnSound);

            checkpointTimeField.text = "Respawn lap time penalty";
            checkpointTimeField.GetComponent<ToggleCanvasGroup>().ShowTemporarily(2f);

            checkpointTimeDiffField.color = Color.red;
            checkpointTimeDiffField.text = "+" + Utils.GetTimeString(TimeSpan.FromSeconds(5));
            checkpointTimeDiffField.GetComponent<ToggleCanvasGroup>().ShowTemporarily(2f);
        }

        public void NextCheckpointPassed(TimeSpan laptime)
        {
            UISound.Play(checkpointSound);
            checkpointTimeField.text = Utils.GetTimeString(laptime);
            checkpointTimeField.GetComponent<ToggleCanvasGroup>().ShowTemporarily(2f);
        }
        
        public void StoreRecord(float time, int checkpointIndex, CharacterTier tier)
        {

            RaceRecord? bestRecord = RaceRecord.Best(tier);
            if (bestRecord is null) //We have no full lap record and hit the end
            {
                if (checkpointIndex != StageReferences.Checkpoints.Length - 1) return;
                
                checkpointTimeDiffField.text = "Lap record set!";
                checkpointTimeDiffField.color = Color.blue;
                checkpointTimeDiffField.GetComponent<ToggleCanvasGroup>().ShowTemporarily(2f);
                return;
            }
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            int stage = ActiveData.Stages.Where(a => a.sceneName == sceneName).First().id;

            float diff = time - bestRecord.CheckpointTimes[checkpointIndex];
            bool faster = diff < 0;
            TimeSpan diffSpan = TimeSpan.FromSeconds(Mathf.Abs(diff));

            checkpointTimeDiffField.text = (faster ? "-" : "+") + Utils.GetTimeString(diffSpan);
            checkpointTimeDiffField.color = faster ? Color.blue : Color.red;
            checkpointTimeDiffField.GetComponent<ToggleCanvasGroup>().ShowTemporarily(2f);

            if (!faster || checkpointIndex != StageReferences.Checkpoints.Length - 1) return;
            checkpointTimeDiffField.text = "New lap record!";
        }

        private void Update()
        {
            fieldContainer.anchorMin = TargetCamera.rect.min;
            fieldContainer.anchorMax = TargetCamera.rect.max;

            float speed = TargetPlayer.Speed;
            string postfix = " ";

            //Speed label
            if (!GameSettings.Instance.useImperial)
            {
                postfix += (Mathf.Floor(speed) == 1f) ? "fast/h" : "fasts/h";
            }
            else
            {
                speed *= 0.62f;
                postfix += (Mathf.Floor(speed) == 1f) ? "lightspeed" : "lightspeeds";
                speedFieldLabel.fontSize = 62;
            }

            //Speed field size and color
            const int min = 96;
            const int max = 150;
            float size = max - (max - min) * Mathf.Exp(-speed * 0.02f);
            speedField.fontSize = (int)size;
            speedField.text = Mathf.Floor(speed).ToString();
            speedFieldLabel.text = postfix;

            //Lap counter
            lapField.color = TargetPlayer.FinishReport.ToColor();
            lapField.text = TargetPlayer.FinishReport.AsLap();
            if (!TargetPlayer.RaceFinished)
            {
                lapField.text = "Lap " + TargetPlayer.Lap + "/" + Globals.settings.Laps;
            }

            //Race time
            
            timeField.color = TargetPlayer.FinishReport.ToColor();
            if (TargetPlayer.FinishReport != null)
            {
                timeField.color = finishedColor;
                timeField.text = TargetPlayer.FinishReport.AsTime();
            }

            if (timeout.Finished(10))
            {
                timeField.text += Environment.NewLine + "<b>Timeout</b> " + Utils.GetTimeString(TimeSpan.FromSeconds(timeout.Now()));
            }
        }
    }
}