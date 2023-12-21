using UnityEngine;
using UnityEngine.UI;

public class TextElement : IElement<Text>
{
    public GameObject gameObject;
    public Text Component { get; set; }
    public RectTransform Transform { get; set; }

    public TextElement(string name, string text)
    {
        gameObject = new(name);

        Transform = gameObject.AddComponent<RectTransform>();
        Transform.sizeDelta = new(400, 50);

        gameObject.AddComponent<CanvasRenderer>();

        Component = gameObject.AddComponent<Text>();
        Component.text = text;
        Component.resizeTextForBestFit = true;
        Component.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        gameObject.AddComponent<ContentSizeFitter>();
    }
}