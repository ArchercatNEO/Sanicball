using UnityEngine;

public interface IElement<T>
{
    public RectTransform Transform { get; set; }
    public T Component { get; set; }
}