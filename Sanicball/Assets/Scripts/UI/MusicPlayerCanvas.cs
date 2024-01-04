using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    public class MusicPlayerCanvas : MonoBehaviour
    {

        static MusicPlayerCanvas playerCanvasPrefab => Resources.Load<MusicPlayerCanvas>("Prefabs\\User Interface\\MusicPlayerCanvas"); //Resources.Load
        public CanvasGroup panel;
        public Text label;
        public bool lobbyOffset = false;

        float showTimer = 0;

        public static MusicPlayerCanvas Create()
        {
            return Instantiate(playerCanvasPrefab);
        }

        void Start()
        {
            panel.alpha = 0;
            if (lobbyOffset)
            {
                var rt = panel.GetComponent<RectTransform>();
                var p = rt.anchoredPosition;
                p.y += 48;
                rt.anchoredPosition = p;
            }
        }

        void Update()
        {
            if (showTimer > 0)
            {
                panel.alpha = Mathf.Lerp(panel.alpha, 1, Time.deltaTime * 20);
                showTimer -= Time.deltaTime;
            }
            else
            {
                panel.alpha = Mathf.Lerp(panel.alpha, 0, Time.deltaTime * 20);
            }
        }

        public void Show(string text)
        {
            label.text = text;
            showTimer = 5;
        }
    }
}
