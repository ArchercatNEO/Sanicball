using System.Net;
using UnityEngine;
using UnityEngine.UI;
using Sanicball.Data;
using Sanicball.Logic;

namespace Sanicball.UI
{
    public class ServerListItem : MonoBehaviour
    {
        [SerializeField] private Text serverNameText = null;
        [SerializeField] private Text serverStatusText = null;
        [SerializeField] private Text playerCountText = null;
        [SerializeField] private Text pingText = null;

        private ServerInfo info;
        private IPEndPoint endpoint;

        public void Init(ServerInfo info, IPEndPoint endpoint, int pingMs, bool isLocal)
        {
            serverNameText.text = info.Config.ServerName;
            serverStatusText.text = info.InRace ? "In race" : "In lobby";
            if (isLocal)
            {
                serverStatusText.text += " - LAN server";
            }
            playerCountText.text = info.Players + "/" + info.Config.MaxPlayers;
            pingText.text = pingMs + "ms";

            this.endpoint = endpoint;
        }

        public void Join()
        {
            MatchStarter.JoinOnlineGame(endpoint);
        }
    }
}
