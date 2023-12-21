using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuPanel
{
    public float time = 1f;

    public bool isOpen = false;
    public Vector2 closedPosition;
    public UnityEvent onOpen;
    public UnityEvent onClose;

    public GameObject gameObject;

    private float pos = 0f;
    private Vector2 startPosition;
    public GroupElement CanvasGroup;
    public RectTransform Transform;

    public MenuPanel(GameObject parent)
    {
        CanvasGroup = new GroupElement("Options");
        CanvasGroup.Transform.anchorMin = new(0.8f, 0);


        gameObject = CanvasGroup.gameObject;
        Transform = CanvasGroup.Transform;
        Parent.SetParent(gameObject, parent);

        CanvasGroup.Transform.offsetMin = new(0, 0);
        CanvasGroup.Transform.offsetMin = new(0, 0);

        Image image = gameObject.AddComponent<Image>();
        image.color = Color.blue;
    }
    public void Animate()
    {
        startPosition = Transform.anchoredPosition;
        CanvasGroup.Component.interactable = isOpen;
        CanvasGroup.Component.alpha = isOpen ? 1f : 0f;
        if (isOpen)
            pos = 1f;

        UpdatePosition();

    }
    public void Open()
    {
        isOpen = true;
        CanvasGroup.Component.alpha = 1f;
        CanvasGroup.Component.interactable = true;
        //onOpen.Invoke();
    }

    public void Close()
    {
        isOpen = false;
        CanvasGroup.Component.interactable = false;
        //onClose.Invoke();
    }

    private void Update()
    {
        if (isOpen && pos < 1f)
        {
            pos = Mathf.Min(1, pos + Time.deltaTime / time);
        }
        if (!isOpen && pos > 0f)
        {
            pos = Mathf.Max(0, pos - Time.deltaTime / time);
            if (pos <= 0f)
            {
                CanvasGroup.Component.alpha = 0f;

            }
        }

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        var smoothedPos = Mathf.SmoothStep(0f, 1f, pos);

        Transform.anchoredPosition = Vector2.Lerp(startPosition + closedPosition, startPosition, smoothedPos);
    }
}