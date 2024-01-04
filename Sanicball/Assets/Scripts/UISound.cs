using UnityEngine;

namespace Sanicball
{
    public static class UISound
    {
        private static readonly AudioSource Instance;

        static UISound()
        {
            Instance = new GameObject("UI Sound (Static)").AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(Instance);
        }

        public static void Play(AudioClip clip)
        {
            Instance.clip = clip;
            Instance.Play();
        }
    }
}
