using Lidgren.Network;

namespace SanicballServer;

internal class ServClient(Guid guid, string name, NetConnection connection)
{
    public Guid Guid { get; } = guid;
    public string Name { get; } = name;

    public NetConnection Connection { get; } = connection;

    public bool CurrentlyLoadingStage { get; set; }
    public bool WantsToReturnToLobby { get; set; }
}
