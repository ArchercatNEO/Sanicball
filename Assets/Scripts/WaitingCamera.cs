using Sanicball.Data;
using Sanicball.Logic;
using UnityEngine;

namespace Sanicball
{
    public class WaitingCamera : MonoBehaviour
    {
        private static WaitingCamera prefab => Resources.Load<WaitingCamera>("Prefabs/Cameras/WaitingCamera");
        public static WaitingCamera Create()
        {
            return Instantiate(prefab);
        }

        private const float switchTime = 8f;
        private const float moveSpeed = 10f;
        private readonly CameraOrientation[] orientations = StageReferences.WaitingCameraOrientations;
        private static readonly Timer timer = new();


        private int currentOrientation = 0;


        private float vol = 0;

        private void AlignWithCurrentOrientation()
        {
            transform.position = orientations[currentOrientation].transform.position;
            transform.rotation = orientations[currentOrientation].CameraRotation;
        }

        // Use this for initialization
        private void Start()
        {
            timer.Start();
            CameraFade.StartAlphaFade(Color.black, true, 4f);
            AudioListener.volume = 0;
            AlignWithCurrentOrientation();
        }

        // Update is called once per frame
        private void Update()
        {
            if (timer.Finished(switchTime))
            {
                timer.Reset();
                timer.Start();

                currentOrientation++;
                currentOrientation %= orientations.Length;

                AlignWithCurrentOrientation();
            }

            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            if (vol < 1f)
            {
                vol = Mathf.Min(1f, vol + Time.deltaTime / 4);
                AudioListener.volume = Mathf.Lerp(0, GameSettings.Instance.soundVolume, vol);
            }
        }
    }
}