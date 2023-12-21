using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sanicball.Data;

namespace Sanicball
{
    public class ESportMode : MonoBehaviour
    {
        private static readonly ESportMode prefab;
        private static ESportMode? Instance;
        public static void TryCreate()
        {
            if (!ESportsReady()) return;
            Instance = Instantiate(prefab);
        }

        public static bool ESportsReady()
        {
            if (!GameSettings.Instance.eSportsReady) return false;

            return !Client.clients[Constants.guid].players.Values.Any(
                player => player.charId != 13
            );
        }

        public Texture2D screenOverlay;
        public Texture2D solidWhite;
        public Texture2D snoop;

        private bool screenOverlayEnabled = false;
        private bool bass = false;
        bool started = false;

        private Color currentColor = new(1, 0, 0, 0.2f);
        const float COLOR_TIME = 60.0f / 110.0f;
        private float colorTimer = COLOR_TIME;

        Vector2 snoopPos = new(0, 0);
        Vector2 snoopTarget = new(0, 0);

        //Groove
        private const int qSamples = 1024; // array size
        private const float refValue = 0.1f; // RMS value for 0 dB
        private float rmsValue; // sound level - RMS
        private float dbValue; // sound level - dB
        private readonly float[] samples = new float[qSamples]; // audio samples
        float RMSmin = 0f;
        float RMSmax = 0f;

        readonly List<Camera> cameras = new();
        readonly Timer timer = new();
        AudioSource music;
        
        public static void StartTheShit()
        {
            Instance?.timer.Start();
        }


        private void Start4Real()
        {
            started = true;
            screenOverlayEnabled = true;
            music = FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>();
            foreach (CameraEffects e in CameraEffects.All)
            {
                cameras.Add(e.GetComponent<Camera>());
                e.bloom.bloomIntensity = 2f;
                e.bloom.bloomThreshold = 0.6f;
                e.blur.velocityScale = 4;
            }
        }

        //Groove
        void GetVolume()
        {
            music.GetOutputData(samples, 0); // fill array with samples
            float sum = 0f;
            for (var i = 0; i < qSamples; i++)
            {
                sum += samples[i] * samples[i]; // sum squared samples
            }
            rmsValue = Mathf.Sqrt(sum / qSamples); // rms = square root of average
            dbValue = 20 * Mathf.Log10(rmsValue / refValue); // calculate dB
            if (dbValue < -160) dbValue = -160; // clamp it to -160dB min
        }

        private void Update()
        {
            if (Camera.main != null)
            {
                transform.position = Camera.main.transform.position;
                transform.rotation = Camera.main.transform.rotation;
            }


            bass = 0.02f < timer.Now() && timer.Now() < 0.2f;
            if (timer.Finished(1))
            {
                timer.Reset();
                Start4Real();
            }

            if (screenOverlayEnabled)
            {
                snoopPos = Vector2.MoveTowards(snoopPos, snoopTarget, Time.deltaTime * 32);
                if (snoopPos == snoopTarget)
                {
                    snoopTarget = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
                }

                colorTimer -= Time.deltaTime;
                if (colorTimer <= 0)
                {
                    currentColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.2f);
                    colorTimer += COLOR_TIME;
                }
            }

            if (started)
            {
                //Groove
                GetVolume();
                RMSmin = Mathf.Min(RMSmin, rmsValue);
                RMSmax = Mathf.Max(RMSmax, rmsValue);
                Camera.main.backgroundColor = Color.Lerp(Color.magenta, Color.blue, rmsValue);

                var fov = 20 - rmsValue * 80;
                foreach (Camera camera in cameras)
                {
                    var omni = camera.GetComponent<Sanicball.Gameplay.OmniCamera>();
                    if (omni)
                    {
                        omni.fovOffset = fov;
                    }
                    else
                    {
                        camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 72 + fov, Time.deltaTime * 20);
                    }
                }
            }
        }

        private void OnGUI()
        {
            Rect getRekt = new(0, 0, Screen.width, Screen.height);

            if (screenOverlayEnabled)
            {
                //Background
                GUIStyle colorStyle = new();
                colorStyle.normal.background = solidWhite;
                colorStyle.stretchWidth = true;
                colorStyle.stretchHeight = true;
                GUI.backgroundColor = currentColor;
                GUI.Box(getRekt, "", colorStyle);
                GUI.backgroundColor = Color.white;

                //Overlay
                GUIStyle mlgStyle = new();
                mlgStyle.normal.background = screenOverlay;
                mlgStyle.stretchWidth = true;
                mlgStyle.stretchHeight = true;
                GUI.Box(getRekt, "", mlgStyle);

                //Snoop
                SpriteSheetGUI ssgui = GetComponent<SpriteSheetGUI>();
                Texture2D texture = snoop;
                Rect snoopRect = new(snoopPos.x, snoopPos.y, 8, 16);
                GUI.BeginGroup(new Rect(snoopRect.x, snoopRect.y, texture.width * snoopRect.width * ssgui.Size.x, texture.height * snoopRect.height * ssgui.Size.y));
                GUI.color = new Color(1, 1, 1, 0.4f);
                GUI.DrawTexture(new Rect(-texture.width * snoopRect.width * ssgui.Offset.x, -texture.height * snoopRect.height * ssgui.Offset.y, texture.width * snoopRect.width, texture.height * snoopRect.height), texture);
                GUI.EndGroup();
            }

            if (bass)
            {
                GUIStyle newstyle = new() {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 600,
                    fontStyle = FontStyle.Bold
                };
                newstyle.normal.textColor = new Color(0, 1, 0, 0.5f);
                GUI.Label(getRekt, "BASS", newstyle);
            }
        }
    }
}