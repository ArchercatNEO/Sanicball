using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Sanicball.Data;
using Sanicball.UI;
using Sanicball.Scenes;
using SanicballCore;
using Lidgren.Network;
using ControlType = Sanicball.Data.ControlType;

namespace Sanicball.Logic
{
    //TODO make this static with UI constructors
    public class MatchStarter
    {
        private static Popup connectingPopupPrefab => Resources.Load<Popup>("Prefabs/User Interface/Popups/ConnectingPopup");

        public static void BeginLocalGame()
        {
            Globals.FromDefault();

            LobbyUI.Load(0);
            LobbyReferences.MatchSettingsPanel?.Show();
            new ClientJoinedMessage().Send();
        }

        public static void StartOnlineGame(Dictionary<Guid, Client> Clients, MatchSettings Settings, bool InRace, float CurAutoStartTime)
        {
            Client.CopyFrom(Clients); //!Inline 
            Globals.settings = Settings;

            //TODO Get and apply travel time
            if (!InRace) { LobbyUI.Load(CurAutoStartTime); }
            else { RaceManager.Load(true); }

            new ClientJoinedMessage().Send();
        }

        public static void JoinOnlineGame(IPEndPoint endpoint)
        {
            ServerRelay.Connect(endpoint);
            PopupHandler.OpenPopup(connectingPopupPrefab);
        }

        public static void JoinOnlineGame(string ip = "127.0.0.1", int port = 25000)
        {
            JoinOnlineGame(new IPEndPoint(IPAddress.Parse(ip), port));
        }

        //TODO put all logic into popups
        /* private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PopupHandler.CloseActivePopup();
                ServerRelay.Disconnect("Cancelled");
            }
        } */
    }

    public record InitMatchMessage : Packet
    {
        public override void Consume() => MatchStarter.StartOnlineGame(Clients, Settings, InRace, CurAutoStartTime);
        
        private readonly Dictionary<Guid, Client> Clients = new();
        private readonly MatchSettings Settings;
        private readonly bool InRace;
        private readonly float CurAutoStartTime;

        public InitMatchMessage(NetBuffer buffer)
        {
            int clientCount = buffer.ReadInt32();
            for (int i = 0; i < clientCount; i++)
            {
                Guid guid = buffer.ReadGuid();
                string name = buffer.ReadString();

                Dictionary<ControlType, Data.Player> players = new();
                int playerCount = buffer.ReadInt32();
                for (int j = 0; j < playerCount; j++)
                {
                    ControlType ctrlType = (ControlType)buffer.ReadInt32();
                    byte readyToRace = buffer.ReadByte();
                    int characterId = buffer.ReadInt32();

                    players.Add(ctrlType, new Data.Player(readyToRace != 0, characterId));
                }

                Clients.Add(guid, new Client(name, players));
            }

            //Match settings
            Settings = new()
            {
                StageId = buffer.ReadInt32(),
                Laps = buffer.ReadInt32(),
                AICount = buffer.ReadInt32(),
                AISkill = (AISkillLevel)buffer.ReadInt32(),
                AutoStartTime = buffer.ReadInt32(),
                AutoStartMinPlayers = buffer.ReadInt32(),
                AutoReturnTime = buffer.ReadInt32(),
                VoteRatio = buffer.ReadFloat(),
                StageRotationMode = (StageRotationMode)buffer.ReadInt32()
            };

            InRace = buffer.ReadByte() != 0;
            CurAutoStartTime = buffer.ReadFloat();
        }
    }
}