using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Sanicball.Data;

namespace Sanicball
{
    [RequireComponent(typeof(Camera))]
    public class CameraEffects : MonoBehaviour
    {
        public static List<CameraEffects> All = new();

        public bool isOmniCam = false;

        [NonSerialized] public Bloom bloom;
        public Texture2D bloomLensFlareVignetteMask;
        public Shader bloomLensFlareShader;
        public Shader bloomScreenBlendShader;
        public Shader bloomBlurAndFlaresShader;
        public Shader bloomBrightPassFilter;

        [NonSerialized] public CameraMotionBlur blur;
        public Shader motionBlurShader;
        public Shader motionBlurDX11Shader;
        public Shader motionBlurReplacementClearShader;
        public Texture2D motionBlurNoiseTexture;

        void Start()
        {
            bloom = gameObject.AddComponent<Bloom>();
            bloom.bloomIntensity = 0.8f;
            bloom.bloomThreshold = 0.8f;

            bloom.lensFlareVignetteMask = bloomLensFlareVignetteMask;
            bloom.lensFlareShader = bloomLensFlareShader;
            bloom.screenBlendShader = bloomScreenBlendShader;
            bloom.blurAndFlaresShader = bloomBlurAndFlaresShader;
            bloom.brightPassFilterShader = bloomBrightPassFilter;

            bloom.enabled = GameSettings.Instance.bloom;

            blur = gameObject.AddComponent<CameraMotionBlur>();
            blur.filterType = CameraMotionBlur.MotionBlurFilter.LocalBlur;
            blur.velocityScale = 1;
            blur.maxVelocity = 1000;
            blur.minVelocity = 0.1f;

            blur.shader = motionBlurShader;
            blur.dx11MotionBlurShader = motionBlurDX11Shader;
            blur.replacementClear = motionBlurReplacementClearShader;
            blur.noiseTexture = motionBlurNoiseTexture;

            blur.enabled = GameSettings.Instance.motionBlur;
            
            All.Add(this);
        }

        private void OnDestroy()
        {
            All.Remove(this);
        }

        public void EnableEffects()
        {
            bloom.enabled = GameSettings.Instance.bloom;
            blur.enabled = GameSettings.Instance.motionBlur;
        }
    }
}