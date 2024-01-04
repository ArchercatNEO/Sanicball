using System.Linq;
using Sanicball.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    public class ClientListEntry : MonoBehaviour
    {
        [SerializeField] private Text nameField = null;
        [SerializeField] private Text playerCountField = null;

        public void FillFields(Client client)
        {
            nameField.text = client.name;

            var players = client.players;
            int playersTotal = players.Count();
            int playersReady = players.Count(a => a.Value.readyToRace);

            if (playersTotal == 0)
            {
                playerCountField.text = "Spectating";
            }
            else
            {
                playerCountField.text = playersReady + "/" + playersTotal + " ready";
            }
        }
    }
}