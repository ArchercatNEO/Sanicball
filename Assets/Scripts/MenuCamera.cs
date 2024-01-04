using System.Collections;
using UnityEngine;

namespace Sanicball
{
    public class MenuCamera : MonoBehaviour
    {
        public static MenuCamera? Instance;
        
        public Transform targetBall;
        public Transform pathParent;
        public StandardShaderFade fade;

        public float moveSpeed = 0.2f;
        public float fadeTime = 0.4f;

        public IEnumerator Resize(float time, float targetWidth)
        {
            Camera camera = GetComponent<Camera>();
            for (float pos = 0; pos < time; pos += Time.deltaTime)
            {
                //var targetWidth = menuWidth * canvas.scaleFactor;
                float smoothedPos = Mathf.SmoothStep(0f, 1f, pos / time);
                camera.rect = new Rect(0, 0, 1f - (smoothedPos * targetWidth) / Screen.width, 1);
                yield return new WaitForFixedUpdate();
            }
        }

        private void Awake() { Instance = this; }
        private void OnDestroy() { Instance = null; }
        
        private void Start()
        {
            StartCoroutine(CycleBalls());
            CameraFade.StartAlphaFade(Color.black, true, 5);
        }

        private IEnumerator CycleBalls()
        {
            while (true)
            {
                foreach (Transform child in pathParent)
                {
                    MenuCameraPath path = child.GetComponent<MenuCameraPath>();
                    
                    targetBall.GetComponent<Renderer>().material = path.character;
                    transform.position = path.transform.position;
                    
                    //Calculate time before hitting end point
                    float dist = Vector3.Distance(transform.position, path.endPoint.position);
                    float endTime = dist / moveSpeed;

                    for (float time = 0; time < endTime - fadeTime; time += Time.deltaTime)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, path.endPoint.position, moveSpeed * Time.deltaTime);
                        transform.LookAt(targetBall.position);
                        yield return new WaitForFixedUpdate();
                    }

                    fade.FadeIn(fadeTime);

                    for (float time = 0; time < fadeTime; time += Time.deltaTime)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, path.endPoint.position, moveSpeed * Time.deltaTime);
                        transform.LookAt(targetBall.position);
                        yield return new WaitForFixedUpdate();
                    }

                    fade.FadeOut(fadeTime);
                }
            }
        }
    }
}