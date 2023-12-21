using UnityEngine;
using UnityEngine.UI;
using Sanicball.Data;
using Sanicball.IO;

namespace Sanicball.UI
{
    /// <summary>
    /// Handles active state and input for a local player identified by its control type.
    /// </summary>
    public class LocalPlayerPanel : MonoBehaviour
    {
        private static LocalPlayerPanel prefab => Resources.Load<LocalPlayerPanel>("Prefabs\\User Interface\\LocalPlayerPanel");
        
        public static LocalPlayerPanel Create(ControlType ctrlType, LocalPlayerManager manager)
        {
            LocalPlayerPanel instance = Instantiate(prefab);
            instance.transform.SetParent(manager.transform, false);
            instance.playerManager = manager;
            instance.AssignedCtrlType = ctrlType;
            return instance;
        }

        public static LocalPlayerPanel Create(ControlType ctrlType, LocalPlayerManager manager, Player player)
        {
            new PlayerChangedMessage(ctrlType, player.charId, player.readyToRace).Send();
            LocalPlayerPanel instance = Instantiate(prefab);
            instance.transform.SetParent(manager.transform, false);
            instance.playerManager = manager;
            instance.AssignedCtrlType = ctrlType;
            instance.AssignedPlayer = player;
            instance.characterSelectSubpanel.gameObject.SetActive(false);
            return instance;
        }

        [System.NonSerialized] public LocalPlayerManager playerManager;

        [SerializeField] private ImageColorToggle readyIndicator;
        [SerializeField] private Image ctrlTypeImageField = null;
        [SerializeField] private Text helpTextField = null;
        [SerializeField] private Sprite[] controlTypeIcons;
        [SerializeField] private CharacterSelectPanel characterSelectSubpanel = null;

        public ControlType AssignedCtrlType { get; set; }
        public Player? AssignedPlayer { get; set; }

        private bool uiPressed = false;

        private void Start()
        {
            ctrlTypeImageField.sprite = controlTypeIcons[(int)AssignedCtrlType];
            
            helpTextField.text = ShowCharacterSelectHelp();
            
            characterSelectSubpanel.CharacterSelected += (sender, e) => {
                if (AssignedPlayer is null)
                {
                    new PlayerJoinedMessage(AssignedCtrlType, e.SelectedCharacter).Send();
                    AssignedPlayer = new Player(false, e.SelectedCharacter);
                }
                else
                {
                    new PlayerChangedMessage(AssignedCtrlType, e.SelectedCharacter, AssignedPlayer.readyToRace).Send();
                }
            };

            characterSelectSubpanel.CancelSelected += (sender, e) => {
                if (AssignedPlayer != null) { new PlayerLeftMessage(AssignedCtrlType).Send(); }
                playerManager.RemoveControlType(AssignedCtrlType);
                
                Destroy(gameObject);
            };
        }

        private void Update()
        {
            //This method handles input from the assigned controltype (if any)
            if (PauseMenu.GamePaused) return; //Short circuit if paused

            bool cActive = characterSelectSubpanel.gameObject.activeSelf;

            if (AssignedCtrlType.IsRespawning() && !cActive && AssignedPlayer is not null)
            {
                readyIndicator.On = !AssignedPlayer.readyToRace;
                new PlayerChangedMessage(AssignedCtrlType, AssignedPlayer.charId, !AssignedPlayer.readyToRace).Send();
            }

            bool accept = AssignedCtrlType.IsOpeningMenu();

            AssignedCtrlType.UIMoves(out bool up, out bool down, out bool left, out bool right);

            if (!(accept || left || right || up || down))
            {
                uiPressed = false;
                return;
            }

            if (uiPressed) {return;}
            uiPressed = true;


            if (accept)
            {
                if (cActive)
                {
                    characterSelectSubpanel.Accept();
                    helpTextField.text = ShowMainHelp();
                }
                else
                {
                    characterSelectSubpanel.gameObject.SetActive(true);
                    helpTextField.text = ShowCharacterSelectHelp();
                }
            }

            if (!cActive) return;
            if (left) characterSelectSubpanel.Left();
            if (right) characterSelectSubpanel.Right();
            if (up) characterSelectSubpanel.Up();
            if (down) characterSelectSubpanel.Down();
        }

        private string ShowMainHelp()
        {
            string kbConfirm = KeybindCollection.GetKeyBindName(Keybind.Menu);
            const string joyConfirm = "X";
            string kbAction = KeybindCollection.GetKeyBindName(Keybind.Respawn);
            const string joyAction = "Y";

            string confirm = AssignedCtrlType == ControlType.Keyboard ? kbConfirm : joyConfirm;
            string action = AssignedCtrlType == ControlType.Keyboard ? kbAction : joyAction;

            return string.Format("<b>{0}</b>: Change character\n<b>{1}</b>: Toggle ready to play", confirm, action);
        }

        private string ShowCharacterSelectHelp()
        {
            const string kbLeft = "Left";
            const string joyLeft = kbLeft;
            const string kbRight = "Right";
            const string joyRight = kbRight;
            string kbConfirm = KeybindCollection.GetKeyBindName(Keybind.Menu);
            const string joyConfirm = "X";

            string left = AssignedCtrlType == ControlType.Keyboard ? kbLeft : joyLeft;
            string right = AssignedCtrlType == ControlType.Keyboard ? kbRight : joyRight;
            string confirm = AssignedCtrlType == ControlType.Keyboard ? kbConfirm : joyConfirm;

            return $"<b>{left}</b>/<b>{right}</b>: Select character\n<b>{confirm}</b>: Confirm";
        }
    }
}