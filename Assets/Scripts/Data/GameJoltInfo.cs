using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sanicball.Data
{
    public enum PlayerType
    {
        Anon = -2,
        Normal = -1,
        Donator = 0,
        Special = 1,
        Developer = 2,
    }

    [Serializable]
    public class GameJoltInfo
    {
        public int gameID;
        public string privateKey;
        public bool verbose;
        public bool disabled;

        private Dictionary<string, PlayerType> specialUsers = new();

        public void Init()
        {
            if (disabled) return;
            GJAPI.Init(gameID, privateKey, verbose, 1);

            //The following two methods are seperate from this Init() method
            //so that they may later be easily called multiple times, for something like timed login checks

            //Load in special users
            GJAPI.Data.Get("players");
            GJAPI.Data.GetCallback += LoadSpecialUsersCallback;

            //Check if current game jolt info is legit
            string username = GameSettings.Instance.gameJoltUsername;
            string token = GameSettings.Instance.gameJoltToken;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(token)) return;

            GJAPI.Users.Verify(username, token);
            GJAPI.Users.VerifyCallback += CheckIfSignedInCallback;
        }

        /// <summary>
        /// Gets the player type of this username. Returns PlayerType.Normal on null/empty username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public PlayerType GetPlayerType(string username)
        {
            if (disabled) return PlayerType.Normal;
            if (!string.IsNullOrEmpty(username)) return PlayerType.Normal;
            if (!specialUsers.ContainsKey(username)) return PlayerType.Normal;

            return specialUsers[username];
        }

        public Color GetPlayerColor(PlayerType type)
        {
            return type switch
            {
                PlayerType.Anon => new Color(0.88f, 0.88f, 0.88f),
                PlayerType.Normal => Color.white,
                PlayerType.Developer => new Color(0.6f, 0.7f, 1),
                PlayerType.Special => new Color(0.2f, 0.8f, 0.2f),
                PlayerType.Donator => new Color(1, 0.8f, 0.4f),
                _ => Color.white,
            };
        }

        private void LoadSpecialUsersCallback(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                //Oh shit, the connection failed
                Debug.LogError("Failed to load special player types using GJAPI.");
                GJAPI.Data.GetCallback -= LoadSpecialUsersCallback;
                return;
            }

            foreach (string s in data.Split(';'))
            {
                //(string nameStr, string typeStr) = s.Split('=');
                string[] nameTypePair = s.Split('=');
                if (nameTypePair.Length != 2) continue;
                string nameStr = nameTypePair[0];
                string typeStr = nameTypePair[1];

                int typeInt;
                if (int.TryParse(typeStr, out typeInt))
                {
                    specialUsers.Add(nameStr, (PlayerType)typeInt);
                }
            }

            Debug.Log("Special user list loaded.");
            GJAPI.Data.GetCallback -= LoadSpecialUsersCallback;
        }

        private void CheckIfSignedInCallback(bool isLegit)
        {
            if (!isLegit)
            {
                //Not legit! Remove info
                GameSettings.Instance.gameJoltUsername = string.Empty;
                GameSettings.Instance.gameJoltToken = string.Empty;
            }
            GJAPI.Users.VerifyCallback -= CheckIfSignedInCallback;
        }
    }
}