using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SanicballCore;

namespace Sanicball.Data
{
    [Serializable]
    public class GameSettings
    {
        public static GameSettings Instance = Save.LoadFile<GameSettings>("GameSettings.json");
        static GameSettings()
        {
            AppDomain.CurrentDomain.ProcessExit += (object sender, EventArgs e) =>
            {
                Save.SaveFile("GameSettings.json", Instance);
            };
        }

        [Header("Online")]
        public string nickname = "";
        public string serverListURL = "https://sanicball.bdgr.zone/servers/";

        public string gameJoltUsername;
        public string gameJoltToken;

        [Header("Display")]
        public int resolution = 0;

        public bool fullscreen = true;
        public bool vsync = false;
        public bool useImperial = false;
        public bool showControlsWhileWaiting = true;

        [Header("Graphics")]
        public int aa = 0;

        public bool trails = true;
        public bool shadows = true;
        public bool motionBlur = false;
        public bool bloom = false;
        public ReflectionQuality reflectionQuality = ReflectionQuality.Off;
        public bool eSportsReady = false;

        [Header("Gameplay")]
        public bool useOldControls = false;

        public float oldControlsMouseSpeed = 3f;
        public float oldControlsKbSpeed = 10f;

        [Header("Audio")]
        public float soundVolume = 1f;

        public bool music = true;
        public bool fastMusic = true;

        //Since settings are saved and the user can modify them externally,
        //they should be validated when loaded
        public void Validate()
        {
            //Resolution
            resolution = Mathf.Clamp(resolution, 0, Screen.resolutions.Length - 1);

            //AA
            if (aa != 0 && aa != 2 && aa != 4 && aa != 8)
                aa = 0;

            //Mouse speed
            oldControlsMouseSpeed = Mathf.Clamp(oldControlsMouseSpeed, 0.5f, 10f);

            //KB speed
            oldControlsKbSpeed = Mathf.Clamp(oldControlsKbSpeed, 0.5f, 10f);

            //Sound volume
            soundVolume = Mathf.Clamp(soundVolume, 0f, 1f);
        }

        public void Apply(bool changeWindow)
        {
            //AA
            QualitySettings.antiAliasing = aa;

            //Vsync
            QualitySettings.vSyncCount = Convert.ToInt32(vsync);

            //Shadows
            GameObject dl = GameObject.Find("Directional light");
            if (dl != null)
            {
                dl.GetComponent<Light>().shadows = shadows ? LightShadows.Hard : LightShadows.None;
            }

            //Volume
            AudioListener.volume = soundVolume;

            //Mute
            MusicPlayer.Mute = false;

            //Camera effects
            foreach (var cam in CameraEffects.All)
            {
                cam.EnableEffects();
            }

            //Resolution and fullscreen
            if (!changeWindow) return;
            if (resolution >= Screen.resolutions.Length) return;

            Resolution res = Screen.resolutions[resolution];
            Screen.SetResolution(res.width, res.height, fullscreen);
        }
    }

    public enum ReflectionQuality
    {
        Off,
        Low,
        Medium,
        High
    }

    [Serializable]
    public class StageInfo
    {
        public string name;
        public int id;
        public string sceneName;
        public Sprite picture;
        public GameObject overviewPrefab;
    }

    [Serializable]
    public class RaceRecord
    {
        public static List<RaceRecord> records = Save.LoadFile<List<RaceRecord>>("Records.json");
        static RaceRecord()
        {
            AppDomain.CurrentDomain.ProcessExit += (object sender, EventArgs e) =>
            {
                Save.SaveFile("Records.json", records);
            };
        }

        public static RaceRecord? Best(CharacterTier tier)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            int stage = ActiveData.Stages.Where(a => a.sceneName == sceneName).First().id;

            return records
                .Where(record => record.IsSame(tier, stage))
                .OrderBy(a => a.Time)
                .FirstOrDefault();
        }

        public readonly CharacterTier Tier;
        public readonly float Time;
        public readonly DateTime Date;
        public readonly int Stage;
        public readonly int Character;
        public readonly float[] CheckpointTimes;
        public readonly float GameVersion;
        public readonly bool WasTesting;

        public RaceRecord(CharacterTier tier, float time, DateTime date, int stage, int character, float[] checkpointTimes)
        {
            Tier = tier;
            Time = time;
            Date = date;
            Stage = stage;
            Character = character;
            CheckpointTimes = checkpointTimes;
            GameVersion = SanicballCore.GameVersion.AS_FLOAT;
            WasTesting = SanicballCore.GameVersion.IS_TESTING;
        }

        public bool IsSame(CharacterTier tier, int stage)
        {
            return Tier == tier
                && Stage == stage
                && GameVersion == SanicballCore.GameVersion.AS_FLOAT
                && WasTesting == SanicballCore.GameVersion.IS_TESTING
            ;
        }
    }

    [Serializable]
    public class CharacterInfo
    {
        public string name;
        public string artBy;
        public BallStats stats;
        public Material material;
        public Sprite icon;
        public Color color = Color.white;
        public Material minimapIcon;
        public Material trail;
        public float ballSize = 1;
        public Mesh alternativeMesh = null;
        public Mesh collisionMesh = null;
        public CharacterTier tier = CharacterTier.Normal;
        public bool hidden = false;
    }
}