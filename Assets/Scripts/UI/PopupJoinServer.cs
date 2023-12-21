using Sanicball.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    public class PopupJoinServer : MonoBehaviour
    {
        [SerializeField] private InputField ipInput;
        [SerializeField] private InputField portInput;
        [SerializeField] private Text portOutput;

        private const int LOWEST_PORT_NUM = 1024;
        private const int HIGHEST_PORT_NUM = 49151;

        public void Connect()
        {
            portOutput.text = "";

            if (!int.TryParse(portInput.text, out int port))
            {
                portOutput.text = "Port must be a number!";
                return;
            }

            if (port < LOWEST_PORT_NUM || port > HIGHEST_PORT_NUM)
            {
                portOutput.text = $"Port number must be between {LOWEST_PORT_NUM} and {HIGHEST_PORT_NUM}.";
                return;

            }

            //Success, start the server
            MatchStarter.JoinOnlineGame(ipInput.text, port);
        }
    }
}
