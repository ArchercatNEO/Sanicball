using UnityEngine;

namespace Sanicball.UI
{
    //TODO make static somehow
    public class PopupHandler : MonoBehaviour
    {
        private static PopupHandler? Instance;
        private static Popup? activePopup;

        public static void OpenPopup(Popup popupPrefab)
        {
            if (Instance == null)
            {
                Debug.LogError("PopupHandler not found, cannot open popup");
                return;
            }

            if (activePopup != null)
            {
                //Closing old popup
                activePopup.Close();
                Instance.groupDisabledOnPopup.interactable = true;
            }
            //Opening new popup
            activePopup = Instantiate(popupPrefab);
            activePopup.transform.SetParent(Instance.targetParent, false);
            activePopup.onClose += () =>
            {
                Instance.groupDisabledOnPopup.interactable = true;
            };
            Instance.groupDisabledOnPopup.interactable = false;
        }

        public static void CloseActivePopup()
        {
            if (Instance == null)
            {
                Debug.LogError("PopupHandler not found, cannot close popup");
                return;
            }

            activePopup?.Close();
            activePopup = null;
            Instance.groupDisabledOnPopup.interactable = true;
        }

        [SerializeField] private CanvasGroup groupDisabledOnPopup;
        [SerializeField] private Transform targetParent;

        private void Start()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
    }
}