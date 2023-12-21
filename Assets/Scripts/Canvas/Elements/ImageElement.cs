using UnityEngine;
using UnityEngine.UI;

public class ImageElement : IElement<Image>
{
    public GameObject gameObject;
    public Image Component { get; set; }
    public RectTransform Transform { get; set; }

    public ImageElement(string name, Texture2D texture)
    {
        gameObject = new(name);
        gameObject.AddComponent<CanvasRenderer>();

        Transform = gameObject.AddComponent<RectTransform>();
        Transform.sizeDelta = new(400, 100);

        Component = gameObject.AddComponent<Image>();
        Rect rectangle = new Rect(0, 0, texture.width, texture.height);
        Sprite sprite = Sprite.Create(texture, rectangle, new Vector2(0.5f, 0.5f));
        Component.sprite = sprite;
    }
}