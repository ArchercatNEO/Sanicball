using System;
using System.Collections;
using Sanicball.Logic;
using SanicballCore;
using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    //TODO, make the main class the container class and turn these into entries
    /// <summary>
    /// 
    /// </summary>
    public class PlayerPortrait : MonoBehaviour
    {
        private const int spacing = 64;
        
        private static PlayerPortrait portraitPrefab => Resources.Load<PlayerPortrait>("Prefabs\\User Interface\\PlayerPortrait");
        private static Transform portraitContainer = null;

        public static PlayerPortrait Create(string name, Color color, Func<float> func)
        {
            PlayerPortrait portrait = Instantiate(portraitPrefab);

            portrait.nameField.text = name;
            portrait.characterImage.color = color;

            portrait.CalculateRaceProgress = func;

            return portrait;
        }

        public Func<float> CalculateRaceProgress { get; private set; }

        [SerializeField] private Text nameField;
        [SerializeField] private Image characterImage;
        [SerializeField] private Text positionField;

        private RectTransform trans;

        private void Start()
        {
            transform.SetParent(portraitContainer, false);
            trans = GetComponent<RectTransform>();
        }

        public void ChangeUI(int position)
        {
            positionField.text = Utils.GetPosString(position);
            
            float y = trans.anchoredPosition.y;
            y = Mathf.Lerp(y, -(position - 1) * 64, Time.deltaTime * 10);
            trans.anchoredPosition = new Vector2(trans.anchoredPosition.x, y);
        }

        public void FinishRace(int position)
        {
            positionField.text = Utils.GetPosString(position);
            positionField.color = new Color(0f, 0.5f, 1f);
            StartCoroutine(Animate(position));
        }

        public IEnumerator Animate(int position)
        {
            float y = trans.anchoredPosition.y;
            y = Mathf.Lerp(y, -(position - 1) * 64, Time.deltaTime * 10);
            trans.anchoredPosition = new Vector2(trans.anchoredPosition.x, y);
            
            yield return new WaitForFixedUpdate();
        }
    }
}