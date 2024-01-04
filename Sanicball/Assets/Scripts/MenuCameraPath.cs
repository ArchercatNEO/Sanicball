using UnityEngine;

namespace Sanicball
{
    public class MenuCameraPath : MonoBehaviour
    {
        public Transform endPoint;
        public Material character;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.05f);
            
            Gizmos.DrawLine(transform.position, endPoint.position);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(endPoint.position, 0.05f);
        }
    }
}
