using System.Collections.Generic;
using System.Linq;
using Sanicball.Data;
using Sanicball.Logic;
using UnityEngine;

namespace Sanicball.UI
{
    public class Scoreboard : MonoBehaviour
    {
        public RectTransform entryContainer = null;
        [SerializeField] private SlideCanvasGroup slide = null;

        private void Update() { if (!slide.isOpen) { slide.Open(); } }
    }
}