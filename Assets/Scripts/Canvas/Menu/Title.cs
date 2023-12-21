using UnityEngine;

public class Title
{
    public ImageElement Component { get; set; }

    //? Local variables for customisation
    private float time = 0.5f;
    private Vector2 positionChange = new Vector2(100, 100);
    private Vector2 sizeChange = new Vector2(100, 100);


    //? Local variables to keep track of state
    private RectTransform transform;
    private Vector2 startPosition;
    private Vector2 startSize;
    private float pos = 0f;

    public Title(GameObject parent)
    {
        Component = new ImageElement("Sanicball", TextureFactory.Sanicball);
        Parent.SetParent(Component, parent);
        Translation.Center(Component);

        transform = Component.Transform;
        startPosition = transform.anchoredPosition;
        startSize = transform.sizeDelta;
    }

    public void Spin()
    {
        if (pos >= 1f) return;

        pos = Mathf.Min(1f, pos + Time.deltaTime / time);

        float smoothedPos = Mathf.SmoothStep(0f, 1f, pos);

        transform.anchoredPosition = Vector2.Lerp(startPosition, startPosition + positionChange, smoothedPos);
        transform.sizeDelta = Vector2.Lerp(startSize, startSize + sizeChange, smoothedPos);
    }

}
