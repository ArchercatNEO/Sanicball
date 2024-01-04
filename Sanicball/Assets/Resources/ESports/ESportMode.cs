using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sanicball.Data;

namespace Sanicball
{
    public class ESportMode : MonoBehaviour
    {
        private static readonly LazyFile<AudioClip> music = new("ESports/BungeeRide");
        public static Song Song => new("Skrollex - Bungee Ride", music.File);

        private static readonly LazyFile<GameObject> hat = new("ESports/ESportsHat");
        public static GameObject Hat => hat.File;

        private static readonly LazyFile<Material> trail = new("Art/Characters/99 Super Sanic/SuperSanicTrailMLG");
        public static Material Trail => trail.File;
        
        private static Rect fullScreen = new(0, 0, Screen.width, Screen.height);
        
        public static bool ESportsReady()
        {
            if (!GameSettings.Instance.eSportsReady) { return false; }

            //Every local player is super sanic
            return Client.clients[Constants.guid].players.Values.All(
                player => player.charId == 13
            );
        }

        public static void StartEMode()
        {
            if (!ESportsReady()) { return; }
            GameObject parent = Camera.main.gameObject ?? new("ESports - Active");
            ESportMode eSport = parent.AddComponent<ESportMode>();
            eSport.enabled = false;
            eSport.StartCoroutine(eSport.StartEModeInternal());
        }

        private IEnumerator StartEModeInternal()
        {
            yield return new WaitForSeconds(0.02f);
            
            GUIStyle newstyle = new() {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 600,
                    fontStyle = FontStyle.Bold
                };
            newstyle.normal.textColor = new Color(0, 1, 0, 0.5f);
            GUI.Label(fullScreen, "BASS", newstyle);
            
            yield return new WaitForSeconds(0.98f);
            
            enabled = true;
        }

        private readonly List<Camera> cameras = new();
        private static readonly LazyFile<Texture2D> screenOverlay = new("ESports/Overlay");
        private void Start()
        {
            StartCoroutine(AnimateSnoop());
            StartCoroutine(AnimateBackground());

            GUIStyle mlgStyle = new();
            mlgStyle.normal.background = screenOverlay.File;
            mlgStyle.stretchWidth = true;
            mlgStyle.stretchHeight = true;
            GUI.Box(fullScreen, "", mlgStyle);

            foreach (CameraEffects e in CameraEffects.All)
            {
                cameras.Add(e.GetComponent<Camera>());
                e.bloom.bloomIntensity = 2f;
                e.bloom.bloomThreshold = 0.6f;
                e.blur.velocityScale = 4;
            }
        }

        private static readonly LazyFile<Texture2D> snoop = new("ESports/Snoop");
        private IEnumerator AnimateSnoop()
        {
            //TODO we lost a lot of information going from get to add
            SpriteSheetGUI ssgui = gameObject.AddComponent<SpriteSheetGUI>();
            Texture2D texture = snoop.File;
            
            Vector3 snoopPos = new(0, 0);
            while (true)
            {
                Vector3 snoopTarget = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
                float distance = Vector3.Distance(snoopPos, snoopTarget);
                while (distance > 0)
                {
                    distance -= Time.deltaTime * 32;
                    snoopPos = Vector2.MoveTowards(snoopPos, snoopTarget, Time.deltaTime * 32);
                    Rect snoopRect = new(snoopPos.x, snoopPos.y, 8, 16);
                    GUI.BeginGroup(new Rect(snoopRect.x, snoopRect.y, texture.width * snoopRect.width * ssgui.Size.x, texture.height * snoopRect.height * ssgui.Size.y));
                    GUI.color = new Color(1, 1, 1, 0.4f);
                    GUI.DrawTexture(new Rect(-texture.width * snoopRect.width * ssgui.Offset.x, -texture.height * snoopRect.height * ssgui.Offset.y, texture.width * snoopRect.width, texture.height * snoopRect.height), texture);
                    GUI.EndGroup();
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        const float COLOR_TIME = 60.0f / 110.0f;
        public LazyFile<Texture2D> solidWhite = new("Art/User Interface/Textures/BoxFlat");
        private IEnumerator AnimateBackground()
        {
            //Background
            GUIStyle colorStyle = new();
            colorStyle.normal.background = solidWhite.File;
            colorStyle.stretchWidth = true;
            colorStyle.stretchHeight = true;

            while (true)
            {
                Color currentColor = new(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.2f);
                GUI.backgroundColor = currentColor;
                GUI.Box(fullScreen, "", colorStyle);
                GUI.backgroundColor = Color.white;
                yield return new WaitForSeconds(COLOR_TIME);
            }
        }

        private const int qSamples = 1024; // array size
        private const float refValue = 0.1f; // RMS value for 0 dB
        private void Update()
        {
            //Groove
            float sum = MusicPlayer.CalculateVolume(qSamples);
            float rmsValue = Mathf.Sqrt(sum / qSamples); // rms = square root of average
            float dbValue = 20 * Mathf.Log10(rmsValue / refValue); // calculate dB
            dbValue = Mathf.Max(dbValue, -160); // clamp it to -160dB min
            
            Camera.main.backgroundColor = Color.Lerp(Color.magenta, Color.blue, rmsValue);

            float fov = 20 - rmsValue * 80;
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
}