using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sanicball.Scenes
{
    public class MenuPanel : VisualElement
    {
        private readonly float time = 1;
        private readonly Vector2 startPosition;
        private readonly Vector2 closedPosition;

        private readonly CancellationTokenSource cancelSource = new();
        
        public MenuPanel(float AnimTime, Vector2 ClosedPosition)
        {
            time = AnimTime;
            closedPosition = ClosedPosition;
            Open();
        }

        public void Open()
        {
            cancelSource.Cancel();
            Task.Run(async () => {
                    visible = true;
                    style.opacity = 1f;
                    pickingMode = PickingMode.Position;
                    
                    for (float pos = 0; pos < time; pos += Time.deltaTime)
                    {
                        var smoothedPos = Mathf.SmoothStep(0f, 1f, pos / time);
                        Vector2 position = Vector2.Lerp(startPosition + closedPosition, startPosition, smoothedPos);
                        
                        style.left = position.x;
                        style.bottom = position.y;
                        await Task.Delay(Mathf.FloorToInt(Time.deltaTime * 1000));
                    }

                }, cancelSource.Token);
        }

        public void Close()
        {
            cancelSource.Cancel();
            Task.Run(async () => {
                    pickingMode = PickingMode.Ignore;
                    
                    for (float pos = 0; pos < time; pos += Time.deltaTime)
                    {
                        var smoothedPos = Mathf.SmoothStep(0f, 1f, pos / time);
                        Vector2 position = Vector2.Lerp(startPosition + closedPosition, startPosition, smoothedPos);
                        
                        style.left = position.x;
                        style.bottom = position.y;
                        await Task.Delay(Mathf.FloorToInt(Time.deltaTime * 1000));
                    }

                    visible = false;
                    style.opacity = 0f;

                }, cancelSource.Token);
        }
    }
}

