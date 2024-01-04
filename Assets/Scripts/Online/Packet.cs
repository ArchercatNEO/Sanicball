using Lidgren.Network;
using Newtonsoft.Json;
using SanicballCore;

public abstract record Packet
{
    //TODO see if we can share client without making it public
    private static readonly NetClient client = ServerRelay.client;
    protected static readonly JsonSerializerSettings settings = new() { TypeNameHandling = TypeNameHandling.All };

    public void Send()
    {
        if (this.RejectSend()) return;
        if (!ServerRelay.OnlineMode)
        {
            ServerRelay.messages.Add(this);
            return;
        }

        NetOutgoingMessage buffer = client.CreateMessage();
        buffer.Write(MessageType.MatchMessage);
        buffer.WriteTime(false);
        this.WriteInto(buffer);

        client.SendMessage(buffer, NetDeliveryMethod.ReliableOrdered);
    }

    public virtual bool RejectSend() => false;

    public virtual void WriteInto(NetBuffer buffer)
    {
        string json = JsonConvert.SerializeObject(this);
        buffer.Write(json);
    }

    public static Packet ReadFromJson(NetBuffer buffer)
    => JsonConvert.DeserializeObject<Packet>(buffer.ReadString(), settings);


    public abstract void Consume();
}