using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sanicball.Data;

namespace Sanicball.UI
{
    /// <summary>
    /// Tracks used control types for local players and handles them joining/leaving the lobby.
    /// </summary>
    public class LocalPlayerManager : MonoBehaviour
    {
        private const int maxPlayers = 4;
        private static readonly List<ControlType> usedControls = new();

        [SerializeField] private Text matchJoiningHelpField = null;

        private void Start()
        {
            //Create local player panels for players already in the game
            foreach (var (control, p) in Client.clients[Constants.guid].players)
            {
                if (control == ControlType.None) { continue; }
                LocalPlayerPanel.Create(control, this, p);
                usedControls.Add(control);
            }

            UpdateHelpText();
        }

        private void Update()
        {
            //This where I check if any control types are trying to join
            if (PauseMenu.GamePaused) return; //Short circuit if the game is paused
            
            
            foreach (ControlType ctrlType in System.Enum.GetValues(typeof(ControlType)))
            {
                if (usedControls.Count >= maxPlayers) { break; }
                if (!ctrlType.IsOpeningMenu() || usedControls.Contains(ctrlType)) { continue; }
                
                usedControls.Add(ctrlType);
                LocalPlayerPanel.Create(ctrlType, this);
            }
            
            UpdateHelpText();
        }

        private void OnDestroy()
        {
            usedControls.Clear();
        }

        public void RemoveControlType(ControlType ctrlType)
        {
            usedControls.Remove(ctrlType);
            UpdateHelpText();
        }

        private void UpdateHelpText()
        {
            matchJoiningHelpField.text = "";
            if (usedControls.Count >= maxPlayers) return;
            if (!usedControls.Contains(ControlType.Keyboard))
            {
                matchJoiningHelpField.text += "Press <b>" + IO.KeybindCollection.GetKeyBindName(IO.Keybind.Menu) + "</b> to join with a keyboard. ";
            }
            matchJoiningHelpField.text += "Press <b>X</b> to join with a joystick.";
        }
    }
}