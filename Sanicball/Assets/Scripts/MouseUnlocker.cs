using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sanicball
{
    public class MouseUnlocker : MonoBehaviour
    {
        private static readonly MouseUnlocker prefab; //Resource.Load

        /// <summary>
        /// This way we guarantee there will always be a one and only MouseUnlocker
        /// </summary>
        /* static MouseUnlocker()
        {
            Instantiate(prefab);
            SceneManager.activeSceneChanged += (sender, e) =>
            {
                Instantiate(prefab);
            };
        } */

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
