using Godot;

namespace Sanicball.Characters;

//TODO: give this guy an ability
[GodotClass]
public partial class Asspio : Character
{
    public Asspio() : base()
    {
        SkinName = "Asspio";
        Credits = "BK-TN";
        Color = new(1, 0, 0.72549f, 1);
        
        Material = GD.Load<Material>("res://src/characters/C07-Asspio/material.tres");
        Icon = GD.Load<Texture2D>("res://src/characters/C07-Asspio/icon.svg");
        MinimapIcon = GD.Load<Texture2D>("res://src/characters/C07-Asspio/minimap.svg");
    }
}