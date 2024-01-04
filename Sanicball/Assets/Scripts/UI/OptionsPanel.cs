using Sanicball.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    public class OptionsPanel : MonoBehaviour
    {
        [Header("Online")]
        public Text nickname;
        public Text serverListURL;
        public Text gameJoltAccount;

        [Header("Display")]
        public Text resolution;

        public Text fullscreen;
        public Text vsync;
        public Text speedUnit;

        [Header("Graphics")]
        public Text aa;

        public Text trails;
        public Text shadows;
        public Text motionBlur;
        public Text bloom;
        public Text reflectionQuality;
        public Text eSportsReady;

        [Header("Gameplay")]
        public Text controlMode;

        public Text cameraSpeedMouse;
        public Text cameraSpeedKeyboard;

        [Header("Audio")]
        public Text soundVolume;
        public Text music;
        public Text fast;

        private GameSettings tempSettings = new();

        public static OptionsPanel? Instance;

        private void Start()
        {
            Instance = this;
            RevertToCurrent();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void Apply()
        {
            GameSettings.Instance = tempSettings;
            GameSettings.Instance.Apply(true);
        }

        public void RevertToCurrent()
        {
            tempSettings = GameSettings.Instance;
            UpdateFields();
        }

        public void ResetToDefault()
        {
            //Do not reset nickname!!
            var nickname = tempSettings.nickname;
            tempSettings = new GameSettings();
            tempSettings.nickname = nickname;
            UpdateFields();
        }

        public void UpdateFields()
        {
            if (!gameObject.activeInHierarchy) return;

            nickname.text = tempSettings.nickname;
            serverListURL.text = tempSettings.serverListURL;
            gameJoltAccount.text = (!string.IsNullOrEmpty(tempSettings.gameJoltToken)) ? "Linked as " + tempSettings.gameJoltUsername : "Not linked";

            if (Screen.resolutions.Length > 0)
            {
                if (tempSettings.resolution >= Screen.resolutions.Length)
                    tempSettings.resolution = 0;
                var res = Screen.resolutions[tempSettings.resolution];
                resolution.text = res.width + " x " + res.height;
            }
            else
            {
                resolution.text = "None found!";
            }
            fullscreen.text = tempSettings.fullscreen ? "Fullscreen" : "Windowed";
            vsync.text = tempSettings.vsync ? "On" : "Off";
            speedUnit.text = tempSettings.useImperial ? "Imperial" : "Metric";

            aa.text = tempSettings.aa == 0 ? "Off" : ("x" + tempSettings.aa);
            trails.text = tempSettings.trails ? "On" : "Off";
            shadows.text = tempSettings.shadows ? "On" : "Off";
            motionBlur.text = tempSettings.motionBlur ? "On" : "Off";
            bloom.text = tempSettings.bloom ? "On" : "Off";
            reflectionQuality.text = tempSettings.reflectionQuality.ToString();
            eSportsReady.text = tempSettings.eSportsReady ? "Born ready" : "No way";

            controlMode.text = tempSettings.useOldControls ? "Rotate manually (Precise)" : "Follow velocity (Intuitive)";
            cameraSpeedMouse.text = tempSettings.oldControlsMouseSpeed.ToString("n1");
            cameraSpeedKeyboard.text = tempSettings.oldControlsKbSpeed.ToString("n1");

            soundVolume.text = tempSettings.soundVolume <= 0f ? "Off" : (tempSettings.soundVolume * 10f).ToString("n0");
            music.text = tempSettings.music ? "On" : "Off";
            fast.text = tempSettings.fastMusic ? "On" : "Off";
        }

        #region Value changers

        public void SetNickname(string nick)
        {
            tempSettings.nickname = nick;
            UpdateFields();
        }

        public void SetServerListURL(string url)
        {
            tempSettings.serverListURL = url;
            UpdateFields();
        }

        public void SetGameJoltInfo(string username, string token)
        {
            tempSettings.gameJoltUsername = username;
            tempSettings.gameJoltToken = token;
            UpdateFields();
        }

        //Used in the popup script for changing nickname
        public string GetNickname()
        {
            return tempSettings.nickname;
        }

        public string GetServerListURL()
        {
            return tempSettings.serverListURL;
        }

        public string GetGameJoltUsername()
        {
            return tempSettings.gameJoltUsername;
        }

        public string GetGameJoltToken()
        {
            return tempSettings.gameJoltToken;
        }

        public void ResolutionUp()
        {
            tempSettings.resolution++;
            if (tempSettings.resolution >= Screen.resolutions.Length)
                tempSettings.resolution = 0;
            UpdateFields();
        }

        public void ResolutionDown()
        {
            tempSettings.resolution--;
            if (tempSettings.resolution < 0)
                tempSettings.resolution = Screen.resolutions.Length - 1;
            UpdateFields();
        }

        public void FullscreenToggle()
        {
            tempSettings.fullscreen = !tempSettings.fullscreen;
            UpdateFields();
        }

        public void VsyncToggle()
        {
            tempSettings.vsync = !tempSettings.vsync;
            UpdateFields();
        }

        public void SpeedUnitToggle()
        {
            tempSettings.useImperial = !tempSettings.useImperial;
            UpdateFields();
        }

        public void AaUp()
        {
            tempSettings.aa = tempSettings.aa switch
            {
                0 => 2,
                2 => 4,
                4 => 8,
                8 => 0,
                _ => 0,
            };
            UpdateFields();
        }

        public void AaDown()
        {
            tempSettings.aa = tempSettings.aa switch
            {
                0 => 8,
                2 => 0,
                4 => 2,
                8 => 4,
                _ => 0,
            };
            UpdateFields();
        }

        public void TrailsToggle()
        {
            tempSettings.trails = !tempSettings.trails;
            UpdateFields();
        }

        public void ShadowsToggle()
        {
            tempSettings.shadows = !tempSettings.shadows;
            UpdateFields();
        }

        public void MotionBlurToggle()
        {
            tempSettings.motionBlur = !tempSettings.motionBlur;
            UpdateFields();
        }

        public void BloomToggle()
        {
            tempSettings.bloom = !tempSettings.bloom;
            UpdateFields();
        }

        public void ReflectionQualityUp()
        {
            int q = (int)tempSettings.reflectionQuality;
            q = Mathf.Min(q + 1, System.Enum.GetNames(typeof(ReflectionQuality)).Length - 1);
            tempSettings.reflectionQuality = (ReflectionQuality)q;
            UpdateFields();
        }

        public void ReflectionQualityDown()
        {
            int q = (int)tempSettings.reflectionQuality;
            q = Mathf.Max(q - 1, 0);
            tempSettings.reflectionQuality = (ReflectionQuality)q;
            UpdateFields();
        }

        public void ESportsToggle()
        {
            tempSettings.eSportsReady = !tempSettings.eSportsReady;
            UpdateFields();
        }

        public void UseOldControlsToggle()
        {
            tempSettings.useOldControls = !tempSettings.useOldControls;
            UpdateFields();
        }

        public void CameraSpeedMouseUp()
        {
            if (tempSettings.oldControlsMouseSpeed < 10f)
                tempSettings.oldControlsMouseSpeed += 0.5f;
            else
                tempSettings.oldControlsMouseSpeed = 10f;
            UpdateFields();
        }

        public void CameraSpeedMouseDown()
        {
            if (tempSettings.oldControlsMouseSpeed > 0.5f)
                tempSettings.oldControlsMouseSpeed -= 0.5f;
            else
                tempSettings.oldControlsMouseSpeed = 0.5f;
            UpdateFields();
        }

        public void CameraSpeedKbUp()
        {
            if (tempSettings.oldControlsKbSpeed < 10f)
                tempSettings.oldControlsKbSpeed += 0.5f;
            else
                tempSettings.oldControlsKbSpeed = 10f;
            UpdateFields();
        }

        public void CameraSpeedKbDown()
        {
            if (tempSettings.oldControlsKbSpeed > 0.5f)
                tempSettings.oldControlsKbSpeed -= 0.5f;
            else
                tempSettings.oldControlsKbSpeed = 0.5f;
            UpdateFields();
        }

        public void SoundVolumeUp()
        {
            if (tempSettings.soundVolume < 1f)
                tempSettings.soundVolume += 0.1f;
            else
                tempSettings.soundVolume = 0f;
            UpdateFields();
        }

        public void SoundVolumeDown()
        {
            if (tempSettings.soundVolume > 0f)
                tempSettings.soundVolume -= 0.1f;
            else
                tempSettings.soundVolume = 1f;
            UpdateFields();
        }

        public void MusicToggle()
        {
            tempSettings.music = !tempSettings.music;
            UpdateFields();
        }

        public void FastMusicToggle()
        {
            tempSettings.fastMusic = !tempSettings.fastMusic;
            UpdateFields();
        }

        #endregion Value changers
    }
}