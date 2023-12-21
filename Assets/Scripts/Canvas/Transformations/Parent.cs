using UnityEngine;

public static class Parent
{
    public static IElement<P> SetParent<P, Q>(IElement<P> child, IElement<Q> parent)
    {
        child.Transform.SetParent(parent.Transform);
        return child;
    }

    public static void SetParent<T>(IElement<T> child, GameObject parent)
    {
        child.Transform.SetParent(parent.transform);
    }

    public static void SetParent<T>(GameObject child, IElement<T> parent)
    {
        child.transform.SetParent(parent.Transform);
    }

    public static void SetParent(GameObject child, GameObject parent)
    {
        child.transform.SetParent(parent.transform);
    }
}