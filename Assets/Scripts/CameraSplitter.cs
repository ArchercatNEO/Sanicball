using UnityEngine;

namespace Sanicball
{
    public class CameraSplitter : MonoBehaviour
    {
        static int count = 0;
        public static void SetSpliScreen(Camera camera, int position)
        {
            count++;
            camera.rect = (count, position) switch
            {
                (2, 0) => new Rect(0, 0.5f, 1, 0.5f),
                (2, _) => new Rect(0, 0f, 1, 0.5f),
                (3, 0) => new Rect(0, 0.5f, 1, 0.5f),
                (3, 1) => new Rect(0, 0, 0.5f, 0.5f),
                (3, 2) => new Rect(0.5f, 0, 0.5f, 0.5f),
                (3, _) => camera.rect,
                (4, 0) => new Rect(0, 0.5f, 0.5f, 0.5f),
                (4, 1) => new Rect(0.5f, 0.5f, 0.5f, 0.5f),
                (4, 2) => new Rect(0, 0, 0.5f, 0.5f),
                (4, 3) => new Rect(0, 0, 0.5f, 0.5f),
                (4, _) => camera.rect,
                _ => new Rect(0, 0, 1, 1)
            };

            AudioListener? listener = camera.GetComponent<AudioListener>();
            if (listener is not null)
            {
                listener.enabled = position == 0;
            }
        }
    }
}