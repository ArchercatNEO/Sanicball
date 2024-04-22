using Godot;

namespace Sanicball.Scenes;

//TODO store the panel being shown in the save file
public partial class RacePreview : Node
{
    private static readonly PackedScene prefab = GD.Load<PackedScene>("res://src/scenes/RacePreview.tscn");

    public static RacePreview Create(string stageName)
    {
        RacePreview preview = prefab.Instantiate<RacePreview>();
        preview.stageName.Text = stageName;
        return preview;
    }

    [Export] private Label stageName = null!;
    [Export] private Label startRaceHotkey = null!;
    [Export] private HBoxContainer controlPanel = null!;

    public override void _Ready()
    {
        //Reset volume to 0 and fade the music back in
        /* Tween soundReset = CreateTween();
        soundReset.TweenProperty(this, ":volume", 1, 1); */

        Tween loopPaths = CreateTween();
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsKeyPressed(Key.F1))
        {
            controlPanel.Hide();
        }
    }
}