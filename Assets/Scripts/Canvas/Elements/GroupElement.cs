using UnityEngine;

public class GroupElement : IElement<CanvasGroup>
{
    public GameObject gameObject;
    public CanvasGroup Component { get; set; }
    public RectTransform Transform { get; set; }

    public GroupElement(string name)
    {
        gameObject = new(name);

        Transform = gameObject.AddComponent<RectTransform>();
        Scaling.Expand(this);


        gameObject.AddComponent<CanvasRenderer>();

        Component = gameObject.AddComponent<CanvasGroup>();
        Component.alpha = 1;
        Component.interactable = true;
        Component.blocksRaycasts = true;
    }

}