using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sanicball.Data;
using Sanicball.UI;

namespace Sanicball
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
        private static MusicPlayer? Instance;
        public static bool Mute 
        {
            set
            {
                if (Instance == null)
                {
                    Debug.LogError("Could not find a MusicPlayer, cannot unmute");
                    return;
                }

                Instance.GetComponent<AudioSource>().mute = value;
            } 
        }

        public static void Play()
        {
            if (Instance == null)
            {
                Debug.LogError("Cannot find MusicPlayer, cannot Play");
                return;
            }

            if (!GameSettings.Instance.music) return;
            string credits = Instance.playlist[Instance.currentSongID].Name;
            Instance.playerCanvas.Show(credits);
            Instance.aSource.Play();
            Instance.isPlaying = true;
        }

        public static void Pause()
        {
            if (Instance == null)
            {
                Debug.LogError("Cannot find MusicPlayer, cannot Play");
                return;
            }

            Instance.aSource.Pause();
            Instance.isPlaying = false;
        }

        public static float CalculateVolume(int sampleSize)
        {
            float[] samples = new float[sampleSize];
            Instance.aSource.GetOutputData(samples, 0); 
            return samples.Aggregate(0f, (accumulator, sample) => {
                return accumulator += sample * sample;
            });
        }

        //public GUISkin skin;
        public bool playerCanvasLobbyOffset = false;
        private MusicPlayerCanvas playerCanvas;

        public bool startPlaying = false;
        public bool fadeIn = false;

        public List<Song> playlist = new();
        public string SongCredits => playlist
            .Aggregate("", (accumulator, song) => {
                return accumulator + $"<b>s.Name</b> \n";
            });
        
        public AudioSource fastSource;

        [System.NonSerialized]
        public bool fastMode = false;

        private int currentSongID = 0;
        private bool isPlaying;

        //Song credits
        private readonly Timer timer = new();
        private const float target = 10;

        private const float slidePositionMax = 20;
        private float slidePosition = slidePositionMax;

        private AudioSource aSource;

        

        private void Start()
        {
            Instance = this;

            playerCanvas = MusicPlayerCanvas.Create();
            playerCanvas.lobbyOffset = playerCanvasLobbyOffset;
            
            //Shuffle playlist using Fisher-Yates algorithm
            for (int i = playlist.Count; i > 1; i--)
            {
                int j = Random.Range(0, i);
                (playlist[i - 1], playlist[j]) = (playlist[j], playlist[i - 1]);
            }

            if (ESportMode.ESportsReady() && Globals.scene != Scene.Lobby)
            {
                playlist.Insert(0, ESportMode.Song);
            }

            aSource = GetComponent<AudioSource>();
            aSource.clip = playlist[0].Clip;
            isPlaying = aSource.isPlaying;
            
            if (fadeIn) { aSource.volume = 0f; }
            if (GameSettings.Instance.music && startPlaying) { Play(); }
            if (!GameSettings.Instance.music) { fastSource.Stop(); }
        }

        private void Update()
        {
            aSource.mute = !GameSettings.Instance.music;
            
            if (fadeIn) { aSource.volume = Mathf.Min(aSource.volume + Time.deltaTime * 0.1f, 0.5f); }
            
            //If it's not playing but supposed to play, change song
            if (isPlaying && (!aSource.isPlaying || ControlTypeImpl.AnyChangingSong()))
            {
                currentSongID = (currentSongID + 1) % playlist.Count;

                aSource.clip = playlist[currentSongID].Clip;
                slidePosition = slidePositionMax;
                Play();
            }

            if (fastMode && fastSource.volume < 1)
            {
                fastSource.volume = Mathf.Min(1, fastSource.volume + Time.deltaTime * 0.25f);
                aSource.volume = 0.5f - fastSource.volume / 2;
            }
            if (!fastMode && fastSource.volume > 0)
            {
                fastSource.volume = Mathf.Max(0, fastSource.volume - Time.deltaTime * 0.5f);
                aSource.volume = 0.5f - fastSource.volume / 2;
            }
            
            slidePosition = timer.Finished(target) switch
            {
                true => Mathf.Lerp(slidePosition, 0, Time.deltaTime * 4),
                false => Mathf.Lerp(slidePosition, slidePositionMax, Time.deltaTime * 2)
            };
        }
    }

    //TODO, see if this can be turned into a record
    [System.Serializable]
    public readonly struct Song
    {
        public readonly string Name;
        public readonly AudioClip Clip;

        public Song(string Name, AudioClip Clip)
        {
            this.Name = Name;
            this.Clip = Clip;
        }
    }
}