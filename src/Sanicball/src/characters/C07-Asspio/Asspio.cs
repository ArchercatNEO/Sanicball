using Godot;

namespace Sanicball.Characters;

//TODO: give this guy an ability
[GodotClass]
public partial class Asspio : Character
{
    public Asspio()
    {
        SkinName = "Asspio";
        Credits = "BK-TN";
        Color = new Color(1, 0, 0.72549f);

        Material = GD.Load<Material>("res://src/characters/C07-Asspio/material.tres");
        Icon = GD.Load<Texture2D>("res://src/characters/C07-Asspio/icon.png");
        MinimapIcon = GD.Load<Texture2D>("res://src/characters/C07-Asspio/minimap.png");
    }
}
