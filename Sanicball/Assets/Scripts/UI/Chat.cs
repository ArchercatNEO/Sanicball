using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sanicball.Data;
using SanicballCore.MatchMessages;

namespace Sanicball.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Chat : MonoBehaviour
    {
        private static Chat prefab => Resources.Load<Chat>("Prefabs/User Interface/Chat/ChatCanvas");
        public static Chat Instance = Instantiate(prefab);

        private const float MAX_VISIBLE_TIME = 4f;
        private const float FADE_TIME = 0.2f;

        [SerializeField] private Text chatMessagePrefab = null;
        [SerializeField] private RectTransform chatMessageContainer = null;
        [SerializeField] private InputField messageInputField = null;
        [SerializeField] private RectTransform hoverArea = null;

        private GameObject prevSelectedObject;
        private CanvasGroup canvasGroup;
        private float visibleTime = 0;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }

        public void Update()
        {
            EventSystem es = EventSystem.current;
            if (ControlTypeImpl.IsOpeningChat() && es.currentSelectedGameObject != messageInputField.gameObject)
            {
                prevSelectedObject = es.currentSelectedGameObject;
                es.SetSelectedGameObject(messageInputField.gameObject);
            }

            if (es.currentSelectedGameObject == messageInputField.gameObject)
            {
                visibleTime = MAX_VISIBLE_TIME;
                if (Input.GetKeyDown(KeyCode.Return))
                    SendMessage();
            }

            if (Input.mousePosition.x < hoverArea.sizeDelta.x && Input.mousePosition.y < hoverArea.sizeDelta.y)
            {
                visibleTime = MAX_VISIBLE_TIME;
            }

            if (visibleTime > 0)
            {
                visibleTime -= Time.deltaTime;
                canvasGroup.alpha = 1;
            }
            else if (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha = Mathf.Max(canvasGroup.alpha - Time.deltaTime / FADE_TIME, 0);
            }
        }

        public void EnableInput() { ControlTypeImpl.KeyboardEnabled = true; }
        public void DisableInput() { ControlTypeImpl.KeyboardEnabled = false; }

        public void SendMessage()
        {
            string text = messageInputField.text;

            if (text.Trim() != string.Empty)
            {
                Client myClient = Client.clients[Constants.guid];
                new ChatMessage(ChatMessageType.User, myClient.name, text).Send();
            }
            EventSystem.current.SetSelectedGameObject(prevSelectedObject);

            messageInputField.text = string.Empty;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public record ChatMessage : Packet
        {
            private readonly ChatMessageType type;
            private readonly string from;
            private readonly string text;

            public ChatMessage(ChatMessageType type, string from, string text)
            {
                this.type = type;
                this.from = from;
                this.text = text;
            }

            public override void Consume()
            {
                Instance.visibleTime = MAX_VISIBLE_TIME;

                Text messageObj = Instantiate(Instance.chatMessagePrefab);
                messageObj.transform.SetParent(Instance.chatMessageContainer, false);
                messageObj.text = type switch
                {
                    ChatMessageType.User => $"<color=#6688ff><b>{from}</b></color>: {text}",
                    ChatMessageType.System => $"<color=#ffff77><b>{text}</b></color>",
                    _ => throw new InvalidCastException()
                };
            }
        }
    }
}