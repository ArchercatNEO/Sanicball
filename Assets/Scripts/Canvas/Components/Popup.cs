using UnityEngine;

public class Popup : MonoBehaviour, IElement<CanvasGroup>
{
    public static Popup popup;
    public RectTransform Transform { get; set; }
    public CanvasGroup Component { get; set; }

    public Popup(string name)
    {
        Component = new CanvasGroup();
        Component.alpha = 0f;
    }

    public static void Open<T>()
    {
        // Make sure to delete the old popup before opening a new one
        Close();

        popup = new Popup("Popup");
        popup.Component.alpha = 1f;
    }

    public static void Close()
    {
        popup.Component.alpha = 0f;
        Destroy(popup);
        popup = null;
    }
}