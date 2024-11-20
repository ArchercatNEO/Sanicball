using Godot;

namespace Sanicball.Scenes;

//TODO store the panel being shown in the save file
[GodotClass]
public partial class RacePreview : Node
{
    private static readonly PackedScene prefab = GD.Load<PackedScene>("res://scenes/RacePreview.tscn");

    public static RacePreview Create(string stageName)
    {
        var preview = prefab.Instantiate<RacePreview>();
        preview.stageName.Text = stageName;
        return preview;
    }

    [BindProperty] private HBoxContainer controlPanel = null!;

    [BindProperty] private Label stageName = null!;
    [BindProperty] private Label startRaceHotkey = null!;

    protected override void _Ready()
    {
        //Reset volume to 0 and fade the music back in
        /* Tween soundReset = CreateTween();
        soundReset.TweenProperty(this, ":volume", 1, 1); */

        var loopPaths = CreateTween();
    }

    protected override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey { Keycode: Key.F1 })
        {
            controlPanel.Hide();
        }
    }
}
