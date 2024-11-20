using Godot;

namespace Sanicball.Characters;

//The fastest thing alive.
//Accelerates extra fast
[GodotClass]
public partial class Sanic : Character
{
    public Sanic()
    {
        AngularAcceleration = 80;

        SkinName = "Sanic";
        Credits = "Deviantart user franz888";
        Color = new Color(0, 0, 1);

        Material = GD.Load<Material>("res://src/characters/C01-Sanic/material.tres");
        Icon = GD.Load<Texture2D>("res://src/characters/C01-Sanic/icon.svg");
        MinimapIcon = GD.Load<Texture2D>("res://src/characters/C01-Sanic/minimap.svg");
    }
}
