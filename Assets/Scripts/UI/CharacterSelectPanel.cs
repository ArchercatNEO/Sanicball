using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sanicball.Data;
using Sanicball.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    public class CharacterSelectionArgs : EventArgs
    {
        public int SelectedCharacter { get; set; }

        public CharacterSelectionArgs(int selectedCharacter)
        {
            SelectedCharacter = selectedCharacter;
        }
    }

    public class CharacterSelectPanel : MonoBehaviour
    {
        private const int COLUMN_COUNT = 4;

        [SerializeField] private RectTransform entryContainer = null;
        [SerializeField] private CharacterSelectEntry entryPrefab = null;
        [SerializeField] private Text characterNameLabel;

        [SerializeField] private float scrollSpeed = 1f;
        [SerializeField] private float normalIconSize = 64;
        [SerializeField] private float selectedIconSize = 96;

        private CharacterSelectEntry[] activeEntries = {};
        private int selected = 0;
        private int Selected
        {
            get => selected;
            set
            {
                selected = value;
                if (value == 0)
                {
                    characterNameLabel.text = "Leave match";
                }
                else
                {
                    characterNameLabel.text = activeEntries[value].name;
                }
            }
        }
        private Data.CharacterInfo SelectedChar => activeEntries[selected].Character;
        private float targetX = 0;
        private float targetY = 0;

        [SerializeField] private Sprite cancelIconSprite;

        public event EventHandler<CharacterSelectionArgs> CharacterSelected;
        public event EventHandler CancelSelected;

        private IEnumerator Start()
        {
            /* GameSettings.Instance.eSportsReady 
                ? 
                : ActiveData.Characters.Where(a => a.tier == SanicballCore.CharacterTier.Hyperspeed);*/

            
            CharacterSelectEntry cancelEnt = Instantiate(entryPrefab);
            cancelEnt.IconImage.sprite = cancelIconSprite;
            cancelEnt.transform.SetParent(entryContainer.transform, false);
            
            List<CharacterSelectEntry> empty = new() {cancelEnt};
            IEnumerable<CharacterSelectEntry> charList = ActiveData.Characters
                .OrderBy(a => a.tier)
                .Where(character => !character.hidden)
                .Select(character => {
                    CharacterSelectEntry characterEnt = Instantiate(entryPrefab);
                    characterEnt.Init(character);
                    characterEnt.transform.SetParent(entryContainer.transform, false);
                    
                    return characterEnt;
                });

            
            empty.AddRange(charList);
            activeEntries = empty.ToArray();

            //Wait a single frame before selecting the first character.
            yield return null;
            Selected = 1;
        }

        public void Right() { Selected = (Selected + 1) % activeEntries.Length; }
        public void Left() { Selected = (Selected + activeEntries.Length - 1) % activeEntries.Length; }
        public void Up() { Selected = (Selected + activeEntries.Length - COLUMN_COUNT) % activeEntries.Length; }
        public void Down() { Selected = (Selected + COLUMN_COUNT) % activeEntries.Length; }

        public void Accept()
        {
            if (Selected == 0) { CancelSelected?.Invoke(this, EventArgs.Empty); }
            else
            {
                gameObject.SetActive(false);
                CharacterSelected?.Invoke(this, new CharacterSelectionArgs(Array.IndexOf(ActiveData.Characters, SelectedChar)));
            }
        }

        private void Update()
        {
            //Find the container's target X to center the Selected character
            targetX = entryContainer.sizeDelta.x / 2 - activeEntries[Selected].RectTransform.anchoredPosition.x;
            targetY = -entryContainer.sizeDelta.y / 2 - activeEntries[Selected].RectTransform.anchoredPosition.y;

            if (!Mathf.Approximately(entryContainer.anchoredPosition.x, targetX))
            {
                float x = Mathf.Lerp(entryContainer.anchoredPosition.x, targetX, scrollSpeed * Time.deltaTime);
                entryContainer.anchoredPosition = new Vector2(x, entryContainer.anchoredPosition.y);
            }

            if (!Mathf.Approximately(entryContainer.anchoredPosition.y, targetY))
            {
                float y = Mathf.Lerp(entryContainer.anchoredPosition.y, targetY, scrollSpeed * Time.deltaTime);
                entryContainer.anchoredPosition = new Vector2(entryContainer.anchoredPosition.x, y);
            }

            //Resize all elements
            for (int i = 0; i < activeEntries.Length; i++)
            {
                var element = activeEntries[i];

                float targetSize = (i == Selected) ? selectedIconSize : normalIconSize;

                if (!Mathf.Approximately(element.Size, targetSize))
                {
                    element.Size = Mathf.Lerp(element.Size, targetSize, scrollSpeed * Time.deltaTime);
                }
            }
        }
    }
}