using Godot;

namespace Sanicball.Plugins;

[GlobalClass, Tool]
public partial class AutoFontSize : Label
{
    private int _minimumSize = 10;
    [Export]
    public int MinimumSize
    {
        get => _minimumSize;
        set
        {
            _minimumSize = value;
            ResizeText();
        }
    }

    private int _maximumSize = 20;
    [Export]
    public int MaximumSize
    {
        get => _maximumSize;
        set
        {
            _maximumSize = value;
            ResizeText();
        }
    }

    private void ResizeText()
    {
        float area = Size.X * Size.Y;
        float pxRes = area / Text.Length;
        int fontSize = Mathf.RoundToInt(pxRes);
        int clampedSize = Mathf.Clamp(fontSize, MinimumSize, MaximumSize);
        AddThemeFontSizeOverride("font_size", clampedSize);
    }

    public override void _Ready()
    {
        ClipText = true;
    }

    public override bool _Set(StringName property, Variant value)
    {
        if (property == "text")
        {
            Text = value.AsString();
            ResizeText();
            return true;
        }

        return false;
    }
}