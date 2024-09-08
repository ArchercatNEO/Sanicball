using Godot;

namespace Sanicball.Plugins;

[GodotClass]
public partial class AutoFontSize : Label
{
    private int _minimumSize = 10;
    [BindProperty]
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
    [BindProperty]
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
        float area = 100; //size.X * size.Y;
        float pxRes = area / Text.Length;
        int fontSize = Mathf.RoundToInt(pxRes);
        int clampedSize = Mathf.Clamp(fontSize, MinimumSize, MaximumSize);
        AddThemeFontSizeOverride(new StringName("font_size"), clampedSize);
    }

    protected override void _Ready()
    {
        ClipText = true;
    }

    protected override bool _Set(StringName property, Variant value)
    {
        if (property == new StringName("text"))
        {
            Text = value.AsString();
            ResizeText();
            return true;
        }

        return false;
    }
}
