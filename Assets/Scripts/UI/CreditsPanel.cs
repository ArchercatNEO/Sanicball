using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sanicball.Data;

namespace Sanicball.UI
{
    public class CreditsPanel : MonoBehaviour
    {

        public Text characterList;
        public Text trackList;
        public MusicPlayer musicPlayerPrefab;

        void Start()
        {
            characterList.text =  ActiveData.Characters
                .Where(a => !a.hidden)
                .OrderBy(a => a.tier)
                .Aggregate("", (accumulator, chara) => {
                    return accumulator + $"{chara.name}: <b>{chara.artBy}</b> \n";
                });

            trackList.text = musicPlayerPrefab.SongCredits;
        }
    }
}