using UnityEngine;
using UnityEngine.UI;
using Sanicball.Gameplay;
using Sanicball.Logic;
using Sanicball.UI;
using Sanicball.Data;
using Sanicball.Ball;

//!Todo, rewrite to not depend on active cameras
namespace Sanicball
{
    public class SpectatorView : MonoBehaviour
    {
        private static SpectatorView prefab => Resources.Load<SpectatorView>("Prefabs/Instantiated/SpectatorView");
        public static SpectatorView Create()
        {
            SpectatorView instance = Instantiate(prefab);
            return instance;
        }

        //! Enable/Disable camera/UI when switching
        [SerializeField] private Text spectatingField = null;
        private OmniCamera activeOmniCamera;
        private PlayerUI activePlayerUI;

        private bool leftPressed;
        private bool rightPressed;
        private int index = 0;
        
        //Since this is only done online (no AIBall) when there are no players (no PlayerBall)
        //We only ever need to handle RemoteBalls
        private void Update()
        {
            AbstractBall player = RaceManager.players[index];
            spectatingField.text = "Spectating <b>" + player.name + "</b>";
            activePlayerUI = player.playerUI;
            
            float xMove = ControlType.Keyboard.MovementVector().x;
            if (xMove < 0 && !leftPressed) { index = (index + RaceManager.players.Count - 1) % RaceManager.players.Count; }
            if (xMove > 0 && !rightPressed) { index = (index + 1) % RaceManager.players.Count; }

            leftPressed = xMove < 0;
            rightPressed = xMove > 0;
        }
    }
}