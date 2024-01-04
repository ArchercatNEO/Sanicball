using UnityEngine;
using UnityEngine.UI;
using Sanicball.UI;

namespace Sanicball.Logic
{
    public class LobbyReferences : MonoBehaviour
    {
        private static LobbyReferences? Instance;
        private void Awake()
        {
            Instance = this;
            CameraFade.StartAlphaFade(Color.black, true, 1f);
        }


        [SerializeField] private MatchSettingsPanel matchSettingsPanel = null;
        [SerializeField] private Text countdownField = null;
        [SerializeField] private RectTransform markerContainer = null;


        public static MatchSettingsPanel? MatchSettingsPanel { get => Instance?.matchSettingsPanel; }
        public static Text? CountdownField { get => Instance?.countdownField; }
        public static RectTransform? MarkerContainer { get => Instance?.markerContainer; }
    }
}
