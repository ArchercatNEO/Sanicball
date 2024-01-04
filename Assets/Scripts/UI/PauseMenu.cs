using Sanicball.Logic;
using Sanicball.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sanicball.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GamePaused { get { return GameObject.FindWithTag(pauseTag); } }
        
        private const string pauseTag = "Pause";
        private static PauseMenu prefab => Resources.Load<PauseMenu>("Prefabs/User Interface/PauseMenu");
        private static PauseMenu? Instance;


        [SerializeField] private GameObject firstSelected = null;
        [SerializeField] private Button contextSensitiveButton;
        [SerializeField] private Text contextSensitiveButtonLabel;

        private bool mouseWasLocked;

        public void MatchSettings()
        {
            LobbyReferences.MatchSettingsPanel.Show();
            Close();
        }

        public void BackToLobby()
        {
            new LoadLobbyMessage().Send();
            Close();
        }

        public void QuitMatch()
        {
            ServerRelay.Disconnect("Client left");
            SceneManager.LoadScene(Constants.menuName);
        }

        public void Close()
        {
            if (mouseWasLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            Destroy(gameObject);
        }

        private void Awake()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                mouseWasLocked = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void Start()
        {
            Instance = this;

            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(firstSelected);
            if (!ServerRelay.OnlineMode)
            {
                Time.timeScale = 0;
                AudioListener.pause = true;
            }

            if (SceneManager.GetActiveScene().name == Constants.lobbyName)
            {
                contextSensitiveButtonLabel.text = "Change match settings";
                contextSensitiveButton.onClick.AddListener(MatchSettings);
                if (ServerRelay.OnlineMode)
                {
                    contextSensitiveButton.interactable = false;
                }
            }
            else
            {
                contextSensitiveButtonLabel.text = "Return to lobby";
                contextSensitiveButton.onClick.AddListener(BackToLobby);
            }
        }

        //TODO get this thing to update without any pause menu
        private void Update()
        {
            //TODO refine the check to include what scene we're in
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
            {
                if (Instance is null)
                {
                    Instance = Instantiate(prefab);
                }
                else
                {
                    Instance.Close();
                }
            }
        }

        private void OnDestroy()
        {
            Instance = null;

            if (!ServerRelay.OnlineMode)
            {
                Time.timeScale = 1;
                AudioListener.pause = false;
            }
        }
    }
}