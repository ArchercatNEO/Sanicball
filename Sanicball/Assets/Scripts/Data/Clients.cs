using System;
using System.Collections.Generic;
using UnityEngine;
using Sanicball.Ball;
using Sanicball.Gameplay;
using Sanicball.Logic;
using Sanicball.Scenes;

namespace Sanicball.Data
{
    [Serializable]
    public class Client
    {
        /// <summary>
        /// List of all clients in the match. Only serves a purpose in online play.
        /// In local play, this list will always only contain the local client.
        /// </summary>
        public static Dictionary<Guid, Client> clients = new();

        public static void Reset() => clients.Clear();
        public static void CopyFrom(Dictionary<Guid, Client> client)
        {
            clients = client;
        }

        public static Client AddClient(Guid guid, string name)
        {
            Client instance = new(name, new());
            clients.Add(guid, instance);
            Debug.Log($"New client {name}");
            return instance;
        }

        public static void RemoveClient(Guid guid)
        {
            clients.Remove(guid, out Client client);
            Debug.Log($"{client.name} left");

            //TODO: Find some way to still have the player in the race, although disabled - so that players leaving while finished don't just disappear
            foreach (ControlType control in Enum.GetValues(typeof(ControlType)))
            {
                Logic.Player key = new(guid, control); 
                RaceManager.playerDict.Remove(key);
            }
        }

        public readonly string name = "Player";
        public readonly Dictionary<ControlType, Player> players;

        public Client(string name, Dictionary<ControlType, Player> players)
        {
            this.name = name;
            this.players = players;
        }
    }

    public record ClientJoinedMessage : Packet
    {
        private readonly Guid guid;
        private readonly string name;

        public ClientJoinedMessage()
        {
            this.guid = Constants.guid;
            this.name = GameSettings.Instance.nickname;
        }

        public override void Consume() => Client.AddClient(guid, name);
    }

    public record ClientLeftMessage : Packet
    {
        private readonly Guid guid;

        public override void Consume() => Client.RemoveClient(guid);
    }


    [Serializable]
    public class Player
    {
        public static AbstractBall AddPlayer(Guid guid, ControlType ctrlType, int character)
        {
            LobbyUI.lobbyTimer.Reset();

            Client client = Client.clients[guid];
            Player player = new(false, character);
            Client.clients[guid].players[ctrlType] = player;
            
            string name = $"{client.name} ({ctrlType.AsName()})";
            var ball = LobbyBallSpawner.SpawnBall();
            var body = ball.GetComponent<Rigidbody>();
            if (guid == Constants.guid)
            {
                return PlayerBall.Spawn(body, ctrlType, character, name, LobbyCamera.Instance);
            }
            else
            {
                return RemoteBall.LobbySpawn(body);
            }
        }

        public static void RemovePlayer(Guid guid, ControlType ctrlType)
        {
            Client.clients[guid].players.Remove(ctrlType);
        }

        public static void ChangePlayer(Guid guid, ControlType ctrlType, int character, bool readyToRace)
        {
            Client client = Client.clients[guid];
            Player player = client.players[ctrlType];

            if (player is null)
            {
                Debug.LogError("Player that does not exist tried to change");
                return;
            }

            if (player.charId != character)
            {
                player.charId = character;
                string name = $"{client.name} ({ctrlType.AsName()})";
                var ball = LobbyBallSpawner.SpawnBall();
                var body = ball.GetComponent<Rigidbody>();
                if (guid == Constants.guid)
                {
                    PlayerBall.Spawn(body, ctrlType, character, name, LobbyCamera.Instance);
                }
                else
                {
                    RemoteBall.LobbySpawn(body);
                }
            }

            if (player.readyToRace != readyToRace)
            {
                player.readyToRace = readyToRace;
                
                //Check if all players are ready and start/stop lobby timer accordingly
                bool anyUnready = false;
                foreach (var client1 in Client.clients.Values)
                    foreach (var player2 in client1.players.Values)
                        anyUnready |= !player2.readyToRace;

                
                if (anyUnready) {LobbyUI.lobbyTimer.Reset();}
                else {LobbyUI.lobbyTimer.Start();}
            }
        }

        /// <summary>
        /// Contains all players in the game, even ones from other clients in online races.
        /// Players are seperate from clients because each client can have
        /// up to 4 players playing in splitscreen.
        /// </summary>

        //Needed to know which player we're talking about
        #region Instance State

        public bool readyToRace;
        public int charId;

        public Player(bool readyToRace, int initialCharacterId)
        {
            this.readyToRace = readyToRace;
            this.charId = initialCharacterId;
        }

        #endregion Instance State
    }

    public record PlayerJoinedMessage : Packet
    {
        private readonly Guid guid;
        private readonly ControlType ctrlType;
        private readonly int character;

        public PlayerJoinedMessage(ControlType ctrlType, int character)
        {
            this.guid = Constants.guid;
            this.ctrlType = ctrlType;
            this.character = character;
        }

        public override void Consume() => Player.AddPlayer(guid, ctrlType, character);
    }

    public record PlayerChangedMessage : Packet
    {
        private readonly Guid guid;
        private readonly ControlType ctrlType;
        private readonly int character;
        private readonly bool readyToRace;

        public PlayerChangedMessage(ControlType ctrlType, int character, bool readyToRace)
        {
            this.guid = Constants.guid;
            this.ctrlType = ctrlType;
            this.character = character;
            this.readyToRace = readyToRace;
        }

        public override void Consume() => Player.ChangePlayer(guid, ctrlType, character, readyToRace);
    }

    public record PlayerLeftMessage : Packet
    {
        private readonly Guid guid = Constants.guid;
        private readonly ControlType ctrlType;

        public PlayerLeftMessage(ControlType ctrlType)
        {
            this.ctrlType = ctrlType;
        }

        public override void Consume() => Player.RemovePlayer(guid, ctrlType);
    }
}
