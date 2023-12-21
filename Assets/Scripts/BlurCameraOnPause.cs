using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Sanicball
{
    [RequireComponent(typeof(Camera))]
    public class BlurCameraOnPause : MonoBehaviour
    {
        private const string pauseTag = "Pause";
        private const float targetBlurSize = 5f;

        private bool paused = false;
        private BlurOptimized? blur;
        [SerializeField] private Shader shader;


        private void Update()
        {
            if (!paused && UI.PauseMenu.GamePaused)
            {
                paused = true;
                blur = gameObject.AddComponent<BlurOptimized>();
                blur.downsample = 1;
                blur.blurSize = 0;
                blur.blurIterations = 3;
                blur.blurType = BlurOptimized.BlurType.StandardGauss;
                blur.blurShader = shader;
            }
            else
            {
                if (blur is not null)
                {
                    blur.blurSize = Mathf.Min(blur.blurSize + Time.unscaledDeltaTime * 40, targetBlurSize);
                }

                if (!UI.PauseMenu.GamePaused)
                {
                    paused = false;
                    Destroy(blur);
                }
            }
        }
    }
}