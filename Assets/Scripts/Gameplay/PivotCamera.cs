using Sanicball.Data;
using UnityEngine;

namespace Sanicball.Gameplay
{
    public class PivotCamera : MonoBehaviour, IBallCamera
    {
        private static PivotCamera prefab;
        public static PivotCamera Create(ControlType ctrl)
        {
            PivotCamera instance = Instantiate(prefab);
            instance.CtrlType = ctrl;
            instance.UseMouse = ctrl == ControlType.Keyboard;
            return instance;
        }

        public Camera AttachedCamera { get { return attachedCamera; } }
        public ControlType CtrlType { get; set; }
        public bool UseMouse { get; set; }

        [SerializeField] private Camera attachedCamera;
        [SerializeField] private Vector3 defaultCameraPosition = new(6, 2.8f, 0);

        private float cameraDistance = 1;
        private float cameraDistanceTarget = 1;

        //From smoothmouselook
        [SerializeField] private int smoothing = 2;
        [SerializeField] public int yMin = -85;
        [SerializeField] public int yMax = 85;

        private float xtargetRotation = 90;
        private float ytargetRotation = 0;
        private float sensitivityMouse = 3;
        private float sensitivityKeyboard = 10;

        public void SetDirection(Quaternion dir)
        {
            Vector3 eulerAngles = dir.eulerAngles + new Vector3(0, 90, 0);
            xtargetRotation = eulerAngles.y;
            ytargetRotation = eulerAngles.z;
        }

        public void Remove()
        {
            Destroy(gameObject);
        }

        private void Start()
        {
            if (UseMouse)
            {
                sensitivityMouse = GameSettings.Instance.oldControlsMouseSpeed;
                sensitivityKeyboard = GameSettings.Instance.oldControlsKbSpeed;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        public Quaternion RotateCamera(Rigidbody Target)
        {
            //Mouse look
            if (UseMouse)
            {
                if (Input.GetMouseButtonDown(0) && ControlTypeImpl.KeyboardEnabled && !UI.PauseMenu.GamePaused)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }

                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    float yAxisMove = Input.GetAxis("Mouse Y") * sensitivityMouse;
                    ytargetRotation += -yAxisMove;

                    float xAxisMove = Input.GetAxis("Mouse X") * sensitivityMouse;
                    xtargetRotation += xAxisMove;
                }
            }

            //Keyboard controls
            Vector2 cameraVector = CtrlType.CameraVector();

            /*if (cameraVector.x < 0)
                xtargetRotation -= 20 * sensitivityKeyboard * Time.deltaTime;
            if (cameraVector.x > 0)
                xtargetRotation += 20 * sensitivityKeyboard * Time.deltaTime;
            if (cameraVector.y > 0)
                ytargetRotation -= 20 * sensitivityKeyboard * Time.deltaTime;
            if (cameraVector.y < 0)
                ytargetRotation += 20 * sensitivityKeyboard * Time.deltaTime;*/

            xtargetRotation += cameraVector.x * 20 * sensitivityKeyboard * Time.deltaTime;
            ytargetRotation -= cameraVector.y * 20 * sensitivityKeyboard * Time.deltaTime;

            ytargetRotation = Mathf.Clamp(ytargetRotation, yMin, yMax);
            xtargetRotation %= 360;
            ytargetRotation %= 360;

            Quaternion pivotRotation = Quaternion.Euler(0, xtargetRotation, ytargetRotation);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, pivotRotation, Time.deltaTime * 10 / smoothing);

            //Zooming
            cameraDistanceTarget = Mathf.Clamp(cameraDistanceTarget - (Input.GetAxis("Mouse ScrollWheel") * 2), 0, 10);
            cameraDistance = Mathf.Lerp(cameraDistance, cameraDistanceTarget, Time.deltaTime * 4);
            //Moving to the target
            transform.position = Target.transform.position;
            //Positioning the camera
            Vector3 targetPoint = defaultCameraPosition * cameraDistance;
            attachedCamera.transform.position = transform.TransformPoint(targetPoint);

            //Set camera FOV to get higher with more velocity
            AttachedCamera.fieldOfView = Mathf.Lerp(AttachedCamera.fieldOfView, Mathf.Min(60f + (Target.velocity.magnitude), 100f), Time.deltaTime * 4);

            return transform.rotation * Quaternion.Euler(0, -90, 0);
        }

        private void OnDestroy()
        {
            //Debug.Log("fuuuuuck");
            if (UseMouse)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}