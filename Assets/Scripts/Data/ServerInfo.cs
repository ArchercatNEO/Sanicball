namespace Sanicball.Data
{
    /// <summary>
    /// Used as response when a client sends a server a discovery request.
    /// </summary>
    public struct ServerConfig
    {
        public string ServerName { get; set; }
        public bool ShowInBrowser { get; set; }
        public int PrivatePort { get; set; }
        public string PublicIP { get; set; }
        public int PublicPort { get; set; }
        public int MaxPlayers { get; set; }
    }

    /// <summary>
    /// Used as response when a client sends a server a discovery request.
    /// </summary>
    public struct ServerInfo
    {
        public ServerConfig Config { get; set; }
        public int Players { get; set; }
        public bool InRace { get; set; }
    }
}