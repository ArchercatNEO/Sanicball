using System.Collections;
using System.Collections.Generic;

namespace SanicballCore
{
    public class MatchState(List<MatchClientState> clients, List<MatchPlayerState> players, MatchSettings settings, bool inRace, float curAutoStartTime)
    {
        public List<MatchClientState> Clients { get; private set; } = clients;
        public List<MatchPlayerState> Players { get; private set; } = players;
        public MatchSettings Settings { get; private set; } = settings;
        public bool InRace { get; private set; } = inRace;
        public float CurAutoStartTime { get; private set; } = curAutoStartTime;

        public void WriteToMessage(Lidgren.Network.NetBuffer message)
        {
            //Client count
            message.Write(Clients.Count);                   //Int32
            //Clients
            for (int i = 0; i < Clients.Count; i++)
            {
                MatchClientState c = Clients[i];
                //Write guid (as byte array)
                message.Write(c.Guid);                      //Byte array w size
                //Write name
                message.Write(c.Name);                      //String
            }
            //Player count
            message.Write(Players.Count);
            //Players
            for (int i = 0; i < Players.Count; i++)
            {
                MatchPlayerState p = Players[i];
                //Client GUID (as byte array)
                message.Write(p.ClientGuid);                //Byte array w size
                //Control type enum as int
                message.Write((int)p.CtrlType);             //Int32 (Cast to ControlType)
                //Ready to race bool
                message.Write(p.ReadyToRace);               //Bool
                //Character id
                message.Write(p.CharacterId);               //Int32
            }
            //Match settings properties, written in the order they appear in code
            message.Write(Settings.StageId);                //Int32
            message.Write(Settings.Laps);                   //Int32
            message.Write(Settings.AICount);                //Int32
            message.Write((int)Settings.AISkill);           //Int32 (Cast to AISkillLevel)
            message.Write(Settings.AutoStartTime);          //Int32
            message.Write(Settings.AutoStartMinPlayers);    //Int32
            message.Write(Settings.AutoReturnTime);         //Int32
            message.Write(Settings.VoteRatio);              //Float
            message.Write((int)Settings.StageRotationMode); //Int32 (Cast to StageRotationMode)

            //In race
            message.Write(InRace);
            //Cur auto start time
            message.Write(CurAutoStartTime);
        }

        public static MatchState ReadFromMessage(Lidgren.Network.NetBuffer message)
        {
            //Clients
            int clientCount = message.ReadInt32();
            List<MatchClientState> clients = new List<MatchClientState>();
            for (int i = 0; i < clientCount; i++)
            {
                System.Guid guid = message.ReadGuid();
                string name = message.ReadString();

                clients.Add(new MatchClientState(guid, name));
            }
            //Players
            int playerCount = message.ReadInt32();
            List<MatchPlayerState> players = new List<MatchPlayerState>();
            for (int i = 0; i < playerCount; i++)
            {
                System.Guid clientGuid = message.ReadGuid();
                ControlType ctrlType = (ControlType)message.ReadInt32();
                bool readyToRace = message.ReadBoolean();
                int characterId = message.ReadInt32();

                players.Add(new MatchPlayerState(clientGuid, ctrlType, readyToRace, characterId));
            }
            //Match settings
            MatchSettings settings = new MatchSettings()
            {
                StageId = message.ReadInt32(),
                Laps = message.ReadInt32(),
                AICount = message.ReadInt32(),
                AISkill = (AISkillLevel)message.ReadInt32(),
                AutoStartTime = message.ReadInt32(),
                AutoStartMinPlayers = message.ReadInt32(),
                AutoReturnTime = message.ReadInt32(),
                VoteRatio = message.ReadFloat(),
                StageRotationMode = (StageRotationMode)message.ReadInt32()
            };
            bool inRace = message.ReadBoolean();
            float curAutoStartTime = message.ReadFloat();

            return new MatchState(clients, players, settings, inRace, curAutoStartTime);
        }
    }

    public class MatchClientState(System.Guid guid, string name)
    {
        public System.Guid Guid { get; private set; } = guid;
        public string Name { get; private set; } = name;
    }

    public class MatchPlayerState(System.Guid clientGuid, ControlType ctrlType, bool readyToRace, int characterId)
    {
        public System.Guid ClientGuid { get; private set; } = clientGuid;
        public ControlType CtrlType { get; private set; } = ctrlType;
        public bool ReadyToRace { get; private set; } = readyToRace;
        public int CharacterId { get; private set; } = characterId;
    }
}
