using UnityEngine;
using UnityEngine.UI;

public class InputGroup : IElement<GroupElement>
{
    public InputElement input;
    public TextElement description;
    public GroupElement Component { get; set; }
    public RectTransform Transform { get; set; }

    public InputGroup(string name, Sprite sprite, string description, string placeholder = "player")
    {
        Component = new GroupElement(name);
        Transform = Component.Transform;

        this.description = new TextElement("Info Text", description);
        this.description.Transform.localPosition = new Vector2(0, 100);
        Parent.SetParent(this.description, this);

        TextElement text = new TextElement("Display Text", "");
        text.Component.supportRichText = false;
        text.Component.color = Color.gray;

        input = new InputElement("Input Field", text, sprite);
        input.Component.text = placeholder;
        input.Component.inputType = InputField.InputType.Standard;
        input.Component.interactable = true;
        Parent.SetParent(input, this);
    }

}
