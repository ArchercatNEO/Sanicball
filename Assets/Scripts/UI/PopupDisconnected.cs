using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    public class PopupDisconnected : MonoBehaviour
    {
        static Popup prefab => Resources.Load<Popup>("Prefabs/User Interface/Popups/DisconnectedPopup");
        public static void OpenDisconnected(string reason)
        {
            PopupHandler.OpenPopup(prefab);
            FindObjectOfType<PopupDisconnected>().Reason = reason;
        }

        [SerializeField] private Text reasonField = null;
        public string Reason { set { reasonField.text = value; } }
    }
}