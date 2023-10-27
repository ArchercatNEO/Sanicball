using System.Collections.Generic;
using System.Net;
using Lidgren.Network;
using Newtonsoft.Json;
using Sanicball.Logic;
using SanicballCore;
using UnityEngine;

public static class ServerRelay
{
    private static readonly NetClient  client;
    private static readonly JsonSerializerSettings settings;

    static ServerRelay()
    {
        NetPeerConfiguration config = new("Sanicball");
        client = new(config);
        client.Start();

        settings = new() {TypeNameHandling = TypeNameHandling.All};
    }

    /// <summary>
    /// Use a <c>foreach</c>
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<IncomingData> NextMessage()
    {
        NetIncomingMessage message = client.ReadMessage();
        for (; message == null; message = client.ReadMessage())
        {
            NetIncomingMessageType type = message.MessageType;
            switch (type)
            {
                case NetIncomingMessageType.Data:
                    switch (message.ReadByte())
                    {
                        case MessageType.MatchMessage:
                            double timestamp = message.ReadTime(false);
                            string data = message.ReadString();
        
                            Debug.Log(timestamp);
                            Debug.Log(data);
                            MatchMessage json = JsonConvert.DeserializeObject<MatchMessage>(data, settings);
                            yield return new IncomingData(Messages.Event, timestamp, json);
                            continue;
                        
                        case MessageType.PlayerMovementMessage: 
                            double time = message.ReadTime(false);
                            PlayerMovement movement = PlayerMovement.ReadFromMessage(message);
                            yield return new IncomingData(Messages.Event, time, movement);
                            continue;
                        
                        default:
                            Debug.Log("Nope");
                            yield return new IncomingData();
                            continue;
                    }

                case NetIncomingMessageType.StatusChanged:
                    NetConnectionStatus status = (NetConnectionStatus) message.ReadByte();
                    string statusMsg = message.ReadString();
                    
                    if (status != NetConnectionStatus.Disconnecting)
                    {
                        Debug.Log("Status change received: " + status + " - Message: " + statusMsg);
                        yield return new IncomingData();
                        continue;
                    }

                    yield return new IncomingData(Messages.Disconnected, NetTime.Now, statusMsg);
                    continue;

                case NetIncomingMessageType.VerboseDebugMessage:
                case NetIncomingMessageType.DebugMessage:
                    Debug.Log(message.ReadString());
                    yield return new IncomingData();
                    continue;

                case NetIncomingMessageType.WarningMessage:
                    Debug.LogWarning(message.ReadString());
                    yield return new IncomingData();
                    continue;

                case NetIncomingMessageType.ErrorMessage:
                    Debug.LogError(message.ReadString());
                    yield return new IncomingData();
                    continue;

                default:
                    Debug.Log("Received unhandled message of type " + message.MessageType);
                    yield return new IncomingData();
                    continue;
            }
        }
    }

    public static void SendMessage()
    {

    }

    public static void Connect(IPEndPoint endpoint)
    {
            ClientInfo info = new(GameVersion.AS_FLOAT, GameVersion.IS_TESTING);
            string json = JsonConvert.SerializeObject(info);
            
            NetOutgoingMessage approval = client.CreateMessage();
            approval.Write(json);
            client.Connect(endpoint, approval);
    }
}

public enum Messages
{
    Void,
    Disconnected,
    Event,
    Movement
}

public class IncomingData
{
    public Messages type;
    public double time;
    public object argument;

    public IncomingData(Messages type, double time, object argument)
    {
        this.type = type;
        this.time = time;
        this.argument = argument;
    }

    public IncomingData()
    {
        this.type = Messages.Void;
        this.time = 0;
        this.argument = null;
    }
}