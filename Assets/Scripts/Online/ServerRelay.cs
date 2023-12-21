using System;
using System.Collections.Generic;
using System.Net;
using Sanicball.Data;
using Sanicball.Logic;
using SanicballCore;
using UnityEngine;
using Newtonsoft.Json;
using Lidgren.Network;
using Sanicball.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.Object;

public static class ServerRelay
{
    public static bool OnlineMode { get; private set; }
    public static readonly NetClient client;
    public static readonly List<Packet> messages = new();

    static ServerRelay()
    {
        NetPeerConfiguration config = new(Constants.APP_ID);
        client = new(config);
        client.Start();
    }

    //TODO make the key not a byte
    private static readonly Dictionary<byte, Func<NetBuffer, Packet>> dataConstructors = new() {
        {0, Packet.ReadFromJson},
        {1, buffer => new InitMatchMessage(buffer)},
        {2, buffer => new RemoteMovement(buffer)},
    };

    private static readonly Dictionary<byte, Func<NetBuffer, Packet>> connectionConstructors = new() {
        {5, buffer => new ConnectedPacket(buffer)},
        {7, buffer => new DisconnectedPacket(buffer)},
    };

    //TODO change packet to take in Scene
    public static void NextMessage(Scene scene)
    {
        if (!OnlineMode)
        {
            messages.ForEach(packet => packet.Consume());
            messages.Clear();
            return;
        }

        NetIncomingMessage message = client.ReadMessage();
        for (; message != null; message = client.ReadMessage())
        {
            NetIncomingMessageType type = message.MessageType;
            switch (type)
            {
                case NetIncomingMessageType.Data:
                    byte discriminator = message.ReadByte();

                    if (dataConstructors.ContainsKey(discriminator))
                        dataConstructors[discriminator](message).Consume();
                    else
                        Debug.Log("Nope");

                    continue;


                case NetIncomingMessageType.StatusChanged:
                    byte discriminator1 = message.ReadByte();

                    if (connectionConstructors.ContainsKey(discriminator1))
                        connectionConstructors[discriminator1](message).Consume();
                    else
                        Debug.Log("Status change received: " + discriminator1 + " - Message: " + message.ReadString());

                    continue;

                case NetIncomingMessageType.VerboseDebugMessage:
                case NetIncomingMessageType.DebugMessage:
                    Debug.Log(message.ReadString());
                    continue;

                case NetIncomingMessageType.WarningMessage:
                    Debug.LogWarning(message.ReadString());
                    continue;

                case NetIncomingMessageType.ErrorMessage:
                    Debug.LogError(message.ReadString());
                    continue;

                default:
                    Debug.Log("Received unhandled message of type " + type);
                    continue;
            }
        }
    }

    public static NetConnection Connect(IPEndPoint endpoint)
    {
        ClientInfo info = new(GameVersion.AS_FLOAT, GameVersion.IS_TESTING);
        string json = JsonConvert.SerializeObject(info);

        NetOutgoingMessage approval = client.CreateMessage();
        approval.Write(json);
        return client.Connect(endpoint, approval);
    }

    //TODO Figure out how to share client to not need to do this export business
    public static void Disconnect(string reason)
    {
        client.Disconnect(reason);
    }

    public record ConnectedPacket : Packet
    {
        private readonly string message = "Disconnected";

        public ConnectedPacket(NetBuffer buffer)
        {
            message = buffer.ReadString();
        }

        public override void Consume()
        {
            OnlineMode = true;
            FindObjectOfType<PopupConnecting>().ShowMessage(message);
            Debug.Log("Connected! Now waiting for match state");
        }
    }

    public record DisconnectedPacket : Packet
    {
        private readonly string message = "Disconnected";

        public DisconnectedPacket(NetBuffer buffer)
        {
            message = buffer.ReadString();
        }

        public override void Consume()
        {
            OnlineMode = false;
            FindObjectOfType<PopupConnecting>().ShowMessage(message);

            //TODO if scene == menu
            SceneManager.LoadScene(Constants.menuName);
            PopupDisconnected.OpenDisconnected(message);
        }
    }
}