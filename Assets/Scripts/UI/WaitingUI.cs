using UnityEngine;
using UnityEngine.UI;
using Sanicball.Data;

namespace Sanicball.UI
{
    public class WaitingUI : MonoBehaviour
    {
        private static WaitingUI prefab => Resources.Load<WaitingUI>("Prefabs/User Interface/WaitingUI");

        public static WaitingUI Create()
        {
            WaitingUI instance = Instantiate(prefab);
            instance.controlsPanel.alpha = GameSettings.Instance.showControlsWhileWaiting ? 1 : 0;
            instance.stageNameField.text = ActiveData.Stages[Globals.settings.StageId].name;
            if (ServerRelay.OnlineMode)
            {
                instance.infoField.text = "Waiting for other players...";
            }
            return instance;
        }

        [SerializeField] private Text stageNameField;

        [SerializeField] private Text infoField;
        [SerializeField] private CanvasGroup controlsPanel;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.JoystickButton6))
            {
                GameSettings.Instance.showControlsWhileWaiting = !GameSettings.Instance.showControlsWhileWaiting;
            }

            controlsPanel.alpha = Mathf.Lerp(controlsPanel.alpha, GameSettings.Instance.showControlsWhileWaiting ? 1 : 0, Time.deltaTime * 20);
        }
    }
}