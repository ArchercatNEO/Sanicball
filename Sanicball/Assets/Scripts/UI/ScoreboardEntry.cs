using Sanicball.Logic;
using SanicballCore;
using UnityEngine;
using UnityEngine.UI;

namespace Sanicball.UI
{
    [RequireComponent(typeof(ToggleCanvasGroup))]
    public class ScoreboardEntry : MonoBehaviour
    {
        private static ScoreboardEntry entryPrefab => Resources.Load<ScoreboardEntry>("Prefabs/User Interface/ScoreboardEntry");

        [SerializeField] private Text positionField = null;
        [SerializeField] private Image iconField = null;
        [SerializeField] private Text nameField = null;
        [SerializeField] private Text timeField = null;

        public static ScoreboardEntry Create(Transform container, Sprite icon, string name, IRaceReport report)
        {
            ScoreboardEntry e = Instantiate(entryPrefab);
            e.transform.SetParent(container, false);

            e.GetComponent<ToggleCanvasGroup>().Show();
            
            e.iconField.sprite = icon;
            
            e.nameField.text = name;
            
            e.timeField.text = report.AsTime();
            
            e.positionField.color = report.ToColor();
            e.positionField.text = report.AsPosition();
            return e;
        }
    }
}