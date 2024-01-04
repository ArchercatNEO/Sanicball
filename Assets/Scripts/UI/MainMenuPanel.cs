using SanicballCore;
using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        [SerializeField] private Text versionNameField = null;
        [SerializeField] private Text taglineField = null;

        private SlideCanvasGroup? activePanel;

        public void SetActivePanel(SlideCanvasGroup panel)
        {
            //Opening new panel
            if (activePanel == null)
            {
                panel.Open();
                activePanel = panel;
            }
            //Changing panel
            else
            {
                CloseActivePanel();
                if (activePanel != panel)
                {
                    panel.Open();
                    activePanel = panel;
                }
            }
        }

        public void CloseActivePanel()
        {
            activePanel.Close();
            activePanel = null;
        }

        private void Start()
        {
            versionNameField.text = GameVersion.AS_STRING;
            taglineField.text = GameVersion.TAGLINE;
        }

        private void Update()
        {
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1)) && FindObjectsOfType<Popup>().Length <= 0)
            {
                if (activePanel != null)
                {
                    CloseActivePanel();
                }
                else
                {
                    Application.Quit();
                }
            }
        }
    }
}