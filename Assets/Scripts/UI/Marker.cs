using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sanicball.Ball;
using Sanicball.Logic;

namespace Sanicball.UI
{
    //TODO UIify this to use straight up constructors instead of instantiate
    //TODO Make this part of the camera instead of using a camera
    [RequireComponent(typeof(RectTransform), typeof(Image))]
    public class Marker : MonoBehaviour
    {
        private const float clampMin = 0.2f;
        private const float clampMax = 1 - clampMin;

        static Marker prefab => Resources.Load<Marker>("Prefabs/User Interface/Marker");

        public static Marker CreateCheckpoint(Transform parent, Transform target, Camera camera)
        {
            Marker marker = Instantiate(prefab);
            marker.transform.SetParent(parent, false);
            marker.Target = target;
            marker.camera = camera;
            marker.Clamp = true;
            marker.textField.text = "Checkpoint";
            marker.rectTransform = marker.GetComponent<RectTransform>();
            return marker;
        }

        public static Marker CreatePlayer(Transform parent, AbstractBall player, Camera camera)
        {
            Color color = player.character.color;
            color.a = 0.2f;

            Marker marker = Instantiate(prefab);
            marker.transform.SetParent(parent, false);
            marker.Target = player.transform;
            marker.camera = camera;
            marker.textField.text = player.name;
            marker.rectTransform = marker.GetComponent<RectTransform>();
            marker.GetComponent<Image>().color = color;
            return marker;
        }

        public static (Marker, Marker[]) CreateAll(Transform parent, Transform checkpoint, Camera camera)
        {
            Marker marker = Instantiate(prefab);
            marker.transform.SetParent(parent, false);
            marker.Target = checkpoint;
            marker.camera = camera;
            marker.Clamp = true;
            marker.textField.text = "Checkpoint";
            marker.rectTransform = marker.GetComponent<RectTransform>();
            marker.GetComponent<Image>().color = Color.clear;

            Marker[] markers = RaceManager.players.Select(player => {
                Color color = player.character.color;
                color.a = 0.2f;

                Marker playerMarker = Instantiate(prefab);
                playerMarker.transform.SetParent(parent, false);
                playerMarker.Target = player.transform;
                playerMarker.camera = camera;
                playerMarker.textField.text = player.name;
                playerMarker.rectTransform = playerMarker.GetComponent<RectTransform>();
                playerMarker.GetComponent<Image>().color = color;
                return playerMarker;
            }).ToArray();

            return (marker, markers);
        }

        public Transform? Target;
        private Camera? camera;
        private bool Clamp = false;
        
        
        [SerializeField] private Text textField;
        private RectTransform rectTransform = null!;

        private void Update()
        {
            if (Target is null || camera is null)
            {
                Destroy(gameObject);
                return;
            }

            var relativePosition = camera.transform.InverseTransformPoint(Target.position);
            relativePosition.z = Mathf.Max(relativePosition.z, 1);

            Vector3 p = camera.WorldToViewportPoint(camera.transform.TransformPoint(relativePosition));
            if (Clamp)
            {
                p.x = Mathf.Clamp(p.x, clampMin, clampMax);
                p.y = Mathf.Clamp(p.y, clampMin, clampMax);
            }

            rectTransform.anchorMin = p;
            rectTransform.anchorMax = p;
        }
    }
}