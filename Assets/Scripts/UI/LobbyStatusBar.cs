using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sanicball.Scenes;

namespace Sanicball.UI
{
    public class LobbyStatusBar : MonoBehaviour
    {
        [SerializeField] private Text leftText = null;
        [SerializeField] private Text rightText = null;

        [SerializeField] private RectTransform clientList = null;
        [SerializeField] private ClientListEntry clientListEntryPrefab = null;

        private List<ClientListEntry> curClientListEntries = new();

        private void Start()
        {
            //Self destruct if not in online mode
            if (!ServerRelay.OnlineMode) { Destroy(gameObject); }
        }

        /* private void Update()
        {
            int clients = Client.clients.Count;
            int players = Player.players.Count;

            if (LobbyManager.autoStartTimer.isRunning)
                { leftText.text = $"Match will start in {GetTimeString()}, or when all players are ready."; }
            else if (Player.players.Count > 0)
                { leftText.text = "Match starts when all players are ready."; }
            else { leftText.text = "Match will not start without players."; }
            rightText.text = $"{pluralize("client", clients)} - {pluralize("player", players)}";

            curClientListEntries.ForEach(entry => Destroy(entry.gameObject));
            curClientListEntries.Clear();

            foreach (var (_, client) in Client.clients)
            {
                ClientListEntry listEntry = Instantiate(clientListEntryPrefab);
                listEntry.transform.SetParent(clientList, false);
                listEntry.FillFields(client);

                curClientListEntries.Add(listEntry);
            }
        } */

        private string pluralize(string name, int count)
        {
            return $"{count} {name}{(count > 1 ? "s" : "")}";
        }

        private string GetTimeString()
        {
            System.TimeSpan timeToUse = System.TimeSpan.FromSeconds(LobbyUI.autoStartTimeout - LobbyUI.autoStartTimer.Now());
            return string.Format("{0:00}:{1:00}", timeToUse.Minutes, timeToUse.Seconds);
        }
    }
}