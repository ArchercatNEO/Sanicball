using UnityEngine;

static class Scaling
{
    /// <summary>
    /// Expand the element to fit the bounds of the canvas
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static IElement<T> Expand<T>(IElement<T> element)
    {
        RectTransform transform = element.Transform;
        transform.anchorMin = Vector2.zero;
        transform.anchorMax = Vector2.one;
        transform.sizeDelta = Vector2.one;
        transform.offsetMin = Vector2.zero;
        transform.offsetMax = Vector2.zero;

        return element;
    }
}