using UnityEngine;
using UnityEngine.UI;

public class InputElement : IElement<InputField>
{
    public GameObject gameObject;
    public RectTransform Transform { get; set; }
    public InputField Component { get; set; }

    public InputElement(string name, TextElement text, Sprite sprite)
    {
        gameObject = new(name);

        Transform = gameObject.AddComponent<RectTransform>();
        Transform.sizeDelta = new(400, 50);

        gameObject.AddComponent<CanvasRenderer>();
        Image box = gameObject.AddComponent<Image>();
        box.sprite = sprite;

        Component = gameObject.AddComponent<InputField>();
        Component.interactable = true;
        Component.targetGraphic = box;
        Component.textComponent = text.Component;
        Component.characterLimit = 32;

        Parent.SetParent(text, this);
    }

}