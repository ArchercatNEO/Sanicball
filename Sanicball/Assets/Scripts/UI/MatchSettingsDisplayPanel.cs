﻿using UnityEngine;
using UnityEngine.UI;
using Sanicball.Data;
using SanicballCore;

namespace Sanicball.UI
{
    public class MatchSettingsDisplayPanel : MonoBehaviour
    {
        private static MatchSettingsDisplayPanel? Instance;
        
        [Header("Fields")]
        public Text stageName;

        public Image stageImage;
        public Text lapCount;
        public Text aiOpponents;
        public Text aiSkill;

        private Vector3 targetStageCamPos;

        [SerializeField] private Animation settingsChangedAnimation = null;

        [SerializeField] private Camera stageLayoutCamera = null;

        private void Start()
        {
            Instance = this;
            Instance.UpdateSettingsInternal(Globals.settings);
        }

        public static void UpdateSettings(MatchSettings settings)
        {
            if (Instance is null)
            {
                Debug.LogError("Idk");
                return;
            }

            Instance.UpdateSettingsInternal(settings);
        }

        private void UpdateSettingsInternal(MatchSettings s)
        {
            targetStageCamPos = new Vector3(s.StageId * 50, stageLayoutCamera.transform.position.y, stageLayoutCamera.transform.position.z);
            stageName.text = ActiveData.Stages[s.StageId].name;
            stageImage.sprite = ActiveData.Stages[s.StageId].picture;
            lapCount.text = s.Laps + (s.Laps == 1 ? " lap" : " laps");
            aiOpponents.text = "";
            /*foreach (var i in s.aiCharacters)
            {
                aiOpponents.text += ActiveData.Characters[i].name + "\n";
            }*/
            aiSkill.text = "AI Skill: " + s.AISkill;

            settingsChangedAnimation.Rewind();
            settingsChangedAnimation.Play();
        }

        private void Update()
        {
            if (Vector3.Distance(stageLayoutCamera.transform.position, targetStageCamPos) > 0.1f)
            {
                stageLayoutCamera.transform.position = Vector3.Lerp(stageLayoutCamera.transform.position, targetStageCamPos, Time.deltaTime * 10f);
                if (Vector3.Distance(stageLayoutCamera.transform.position, targetStageCamPos) <= 0.1f)
                {
                    stageLayoutCamera.transform.position = targetStageCamPos;
                }
            }
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }

    public record SettingsChangedMessage : Packet
    {
        private readonly MatchSettings matchSettings;

        public SettingsChangedMessage(MatchSettings settings)
        {
            matchSettings = settings;
        }

        public override void Consume()
        {
            Globals.settings = matchSettings;
            MatchSettingsDisplayPanel.UpdateSettings(matchSettings);
        }
    }
}