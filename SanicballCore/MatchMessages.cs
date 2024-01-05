namespace SanicballCore.MatchMessages
{
    public delegate void MatchMessageHandler<T>(T message, float travelTime) where T : MatchMessage;
    public abstract record class MatchMessage;


    public record class AutoStartTimerMessage(bool Enabled) : MatchMessage;
    public record class ChangedReadyMessage(Guid ClientGuid, ControlType CtrlType, bool Ready) : MatchMessage;
    public record class CharacterChangedMessage(Guid ClientGuid, ControlType CtrlType, int NewCharacter) : MatchMessage;

    public enum ChatMessageType
    {
        System,
        User
    }
    public record class ChatMessage(string From, ChatMessageType Type, string Text) : MatchMessage;
    public record class CheckpointPassedMessage(Guid ClientGuid, ControlType CtrlType, float LapTime) : MatchMessage;
    public record class ClientJoinedMessage(Guid ClientGuid, string ClientName) : MatchMessage;
    public record class ClientLeftMessage(Guid ClientGuid) : MatchMessage;
    public record class DoneRacingMessage(Guid ClientGuid, ControlType CtrlType, double RaceTime, bool Disqualified) : MatchMessage;
    public record class LoadLobbyMessage : MatchMessage;
    public record class LoadRaceMessage : MatchMessage;
    public record class PlayerJoinedMessage(Guid ClientGuid, ControlType CtrlType, int InitialCharacter) : MatchMessage;
    public record class PlayerLeftMessage(Guid ClientGuid, ControlType CtrlType) : MatchMessage;
    public record class RaceFinishedMessage(Guid ClientGuid, ControlType CtrlType, float RaceTime, int RacePosition) : MatchMessage;
    public record class RaceTimeoutMessage(Guid ClientGuid, ControlType CtrlType, float Time) : MatchMessage;


    public record class SettingsChangedMessage(MatchSettings NewMatchSettings) : MatchMessage;
    public record class StartRaceMessage : MatchMessage;
}