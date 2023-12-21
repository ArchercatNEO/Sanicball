using UnityEngine;
using UnityEngine.UI;

public static class Translation
{
    public static void Translate(IElement<Text> element)
    {

    }

    public static void Center<T>(IElement<T> element)
    {
        element.Transform.position = new Vector2(300, 150);
    }
}