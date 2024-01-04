using System.Collections.Generic;
using UnityEngine;

namespace Sanicball.Scenes
{
    public class OnlinePanel : MenuPanel
    {
        private static readonly List<string> serverIps = new();
        public OnlinePanel(float AnimTime, Vector2 ClosedPosition) : base(AnimTime, ClosedPosition)
        {
        }
    }
}