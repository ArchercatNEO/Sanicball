using System.Collections.Generic;
using UnityEngine;

namespace Sanicball.Gameplay
{
    //TODO, find a nicer way to use where this is pointing without the interface
    [RequireComponent(typeof(Camera))]
    public class LobbyCamera : MonoBehaviour, IBallCamera
    {
        public static LobbyCamera Instance => FindObjectOfType<LobbyCamera>();
        public float rotationSpeed;
        private Quaternion startRotation;

        private readonly List<Rigidbody?> balls = new();
        private Vector3 sum = Vector3.zero;


        public Quaternion RotateCamera(Rigidbody target)
        {
            balls.Add(target);
            sum += target.transform.position;
            return transform.rotation;
        }

        //We aren't interested in changing the direction here
        public void SetDirection(Quaternion dir) { }

        public void Remove()
        {
            Destroy(gameObject);
        }

        private void Start()
        {
            startRotation = transform.rotation;
        }

        private void LateUpdate()
        {
            Quaternion targetRotation = startRotation;
            if (balls.Count != 0)
            {
                //Divide sum by number of balls to get the average position (<3 you vector math)
                Vector3 target = sum / balls.Count;
                targetRotation = Quaternion.LookRotation(target - transform.position);
            }

            //Rotate towards target point
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            sum = Vector3.zero;
            balls.Clear();
        }
    }
}
