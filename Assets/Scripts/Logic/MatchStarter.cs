using System.Net;
using UnityEngine;
using SanicballCore;
using Lidgren.Network;

namespace Sanicball.Logic
{
    public class MatchStarter : MonoBehaviour
    {
        public const string APP_ID = "Sanicball";

        [SerializeField] private MatchManager matchManagerPrefab = null;
        [SerializeField] private UI.Popup connectingPopupPrefab = null;
        [SerializeField] private UI.PopupHandler popupHandler = null;

        private UI.PopupConnecting activeConnectingPopup;

        //NetClient for when joining online matches
        private NetClient joiningClient;

        private void Update()
        {
            if (joiningClient == null) return;
            
            NetIncomingMessage msg = joiningClient.ReadMessage();
            for (; msg != null; msg = joiningClient.ReadMessage())
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                        Debug.Log(msg.ReadString());
                        break;

                    case NetIncomingMessageType.WarningMessage:
                        Debug.LogWarning(msg.ReadString());
                        break;

                    case NetIncomingMessageType.ErrorMessage:
                        Debug.LogError(msg.ReadString());
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus) msg.ReadByte();

                        switch (status)
                        {
                            case NetConnectionStatus.Connected:
                                Debug.Log("Connected! Now waiting for match state");
                                activeConnectingPopup.ShowMessage("Receiving match state...");
                                break;

                            case NetConnectionStatus.Disconnected:
                                activeConnectingPopup.ShowMessage(msg.ReadString());
                                break;

                            default:
                                string statusMsg = msg.ReadString();
                                Debug.Log("Status change received: " + status + " - Message: " + statusMsg);
                                break;
                        }
                        break;

                    case NetIncomingMessageType.Data:
                        byte type = msg.ReadByte();
                        if (type == MessageType.InitMessage)
                        {
                            try
                            {
                                //Called when succesfully connected to a server
                                MatchState matchInfo = MatchState.ReadFromMessage(msg);
                                Instantiate(matchManagerPrefab).InitOnlineMatch(joiningClient, matchInfo);
                            }
                            catch (System.Exception ex)
                            {
                                activeConnectingPopup.ShowMessage("Failed to read match message - cannot join server!");
                                Debug.LogError("Could not read match state, error: " + ex.Message);
                            }

                        }
                        break;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                popupHandler.CloseActivePopup();
                joiningClient.Disconnect("Cancelled");
                joiningClient = null;
            }
        }

        public void BeginLocalGame()
        {
            MatchManager manager = Instantiate(matchManagerPrefab);
            manager.InitLocalMatch();
        }

        public void JoinOnlineGame(string ip = "127.0.0.1", int port = 25000)
        {
            JoinOnlineGame(new IPEndPoint(IPAddress.Parse(ip), port));
        }

        public void JoinOnlineGame(IPEndPoint endpoint)
        {
            NetPeerConfiguration conf = new(APP_ID);
            conf.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            conf.EnableMessageType(NetIncomingMessageType.DebugMessage);
            conf.EnableMessageType(NetIncomingMessageType.WarningMessage);
            conf.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            joiningClient = new NetClient(conf);
            joiningClient.Start();

            //Create approval message
            NetOutgoingMessage approval = joiningClient.CreateMessage();

            ClientInfo info = new(GameVersion.AS_FLOAT, GameVersion.IS_TESTING);
            var thing = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            approval.Write(thing);

            //Debug.Log(approval.ToString());
            joiningClient.Connect(endpoint, approval);

            popupHandler.OpenPopup(connectingPopupPrefab);

            activeConnectingPopup = FindObjectOfType<UI.PopupConnecting>();
        }
    }
}