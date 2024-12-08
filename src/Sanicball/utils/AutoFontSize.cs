using Godot;

namespace Sanicball.Utils;

[GodotClass]
public partial class AutoFontSize : Label
{
    [BindProperty]
    public int MinimumSize
    {
        get;
        set
        {
            field = value;
            ResizeText();
        }
    } = 10;

    [BindProperty]
    public int MaximumSize
    {
        get;
        set
        {
            field = value;
            ResizeText();
        }
    } = 20;

    private AutoFontSize()
    {
        ClipText = true;
    }

    private void ResizeText()
    {
        var area = Size.X * Size.Y;
        var glyphPx = area / Text.Length;
        var fontSize = Mathf.RoundToInt(glyphPx);
        var clampedSize = Mathf.Clamp(fontSize, MinimumSize, MaximumSize);
        AddThemeFontSizeOverride(new StringName("font_size"), clampedSize);
    }

    protected override bool _Set(StringName property, Variant value)
    {
        if (property == Label.PropertyName.Text)
        {
            Text = value.AsString();
            ResizeText();
            return true;
        }

        return false;
    }
}
