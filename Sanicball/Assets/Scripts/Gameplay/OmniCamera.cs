using Sanicball.Ball;
using Sanicball.Data;
using UnityEngine;

namespace Sanicball.Gameplay
{
    [RequireComponent(typeof(Camera))]
    public class OmniCamera : MonoBehaviour, IBallCamera
    {
        private static OmniCamera Prefab => Resources.Load<OmniCamera>("Prefabs/Cameras/PlayerCamera");
        public static OmniCamera Create(ControlType ctrl)
        {
            OmniCamera instance = Instantiate(Prefab);
            instance.CtrlType = ctrl;
            instance.AttachedCamera = instance.GetComponent<Camera>();
            return instance;
        }

        public Camera AttachedCamera { get; private set; }
        public ControlType CtrlType { get; set; }
        public float fovOffset = 0;

        [SerializeField] private float orbitHeight = 0.5f;
        [SerializeField] private float orbitDistance = 4.0f;

        private Vector3 OrbitHeight => Vector3.up * orbitHeight;
        private Vector3 OrbitDistance => Vector3.back * orbitDistance;


        private Quaternion currentDirection = Quaternion.Euler(0, 0, 0);
        private Quaternion currentDirectionWithOffset = Quaternion.Euler(0, 0, 0);
        private Vector3 up = Vector3.up;


        public void SetDirection(Quaternion dir)
        {
            currentDirection = dir;
        }

        public void Remove()
        {
            Destroy(gameObject);
        }

        //Translate input to a rotation that will map a vector to fo
        private static Quaternion InputRotation(ControlType ctrlType)
        {
            Vector2 inputVector = ctrlType.CameraVector();
            if (inputVector == Vector2.zero) return Quaternion.identity;

            Vector3 flatDirection = new(inputVector.x, 0, inputVector.y);
            // Generate the rotation needed to map [0, 0, 1] to flatDirection
            Quaternion cameraRotation = Quaternion.LookRotation(flatDirection);
            // Scale down the rotation based on our magnitude
            Quaternion scaledRotation = Quaternion.Slerp(Quaternion.identity, cameraRotation, flatDirection.magnitude);
            return scaledRotation;
        }

        //Rotate the camera towards the velocity of the rigidbody
        public Quaternion RotateCamera(Rigidbody Target)
        {
            //Set the up vector, and make it lerp towards the target's up vector if the target has a Ball
            Vector3 targetUp = Target.GetComponent<AbstractBall>()?.Up ?? Vector3.up;
            up = Vector3.Lerp(up, targetUp, Time.deltaTime * 100 /*~6*/);

            const float maxTrans = 20f;
            // If we are moving generate a rotation to map [0, 0, 1] to velocity and [0, 1, 0] to up
            // Otherwise generate an empty rotation
            Quaternion velocityRotation = (Target.velocity != Vector3.zero) ? Quaternion.LookRotation(Target.velocity, up) : Quaternion.identity;
            // Interpolate between our current facing rotation and our new velocity rotation
            Quaternion finalTargetDir = Quaternion.Slerp(currentDirection, velocityRotation, (Target.velocity.magnitude - 10) / maxTrans);

            //Lerp towards the final rotation
            Quaternion inputRotation = InputRotation(CtrlType);
            currentDirection = Quaternion.Slerp(currentDirection, finalTargetDir, Time.deltaTime * 4);
            currentDirectionWithOffset = Quaternion.Slerp(currentDirectionWithOffset, currentDirection * inputRotation, Time.deltaTime * 6);


            transform.position = Target.transform.position + OrbitHeight + currentDirectionWithOffset * OrbitDistance;
            transform.rotation = currentDirectionWithOffset;

            //Set camera FOV to get higher with more velocity
            AttachedCamera.fieldOfView = Mathf.Lerp(AttachedCamera.fieldOfView, Mathf.Min(60f + Target.velocity.magnitude, 100f) + fovOffset, Time.deltaTime * 20);

            return currentDirection;
        }
    }
}