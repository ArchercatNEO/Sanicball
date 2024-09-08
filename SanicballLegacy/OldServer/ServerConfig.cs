namespace SanicballServer;

//Used as response when a client sends a server a discovery request.
internal struct ServerConfig
{
    public string ServerName { get; set; }
    public bool ShowOnList { get; set; }
    public string[] ServerListURLs { get; set; }
    public int PrivatePort { get; set; }
    public string PublicIP { get; set; }
    public int PublicPort { get; set; }
    public int MaxPlayers { get; set; }
}

//Used as response when a client sends a server a discovery request.
internal struct ServerInfo
{
    public ServerConfig Config { get; set; }
    public int Players { get; set; }
    public bool InRace { get; set; }
}
