using Godot;

namespace Sanicball.Utils;

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

    private AutoFontSize()
    {
        ClipText = true;
    }

    private void ResizeText()
    {
        float area = Size.X * Size.Y;
        float glyphPx = area / Text.Length;
        int fontSize = Mathf.RoundToInt(glyphPx);
        int clampedSize = Mathf.Clamp(fontSize, MinimumSize, MaximumSize);
        AddThemeFontSizeOverride(new StringName("font_size"), clampedSize);
    }

    protected override bool _Set(StringName property, Variant value)
    {
        if (property == PropertyName.Text)
        {
            Text = value.AsString();
            ResizeText();
            return true;
        }

        return false;
    }
}
