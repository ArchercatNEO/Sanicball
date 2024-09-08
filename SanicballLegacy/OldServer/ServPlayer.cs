using System.Diagnostics;
using SanicballCore;

namespace SanicballServer;

internal class ServPlayer(Guid clientGuid, ControlType ctrlType, int initialCharacterId)
{
    public Guid ClientGuid { get; } = clientGuid;
    public ControlType CtrlType { get; } = ctrlType;
    public int CharacterId { get; set; } = initialCharacterId;
    public bool ReadyToRace { get; set; } = false;

    public bool CurrentlyRacing { get; set; } = false;
    public Stopwatch RacingTimeout { get; } = new();
    public bool TimeoutMessageSent { get; set; } = false;
}
