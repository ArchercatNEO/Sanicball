using Godot;

namespace Sanicball.Characters;

public partial record class SanicCharacter
{
    public static readonly SanicCharacter Taels = new()
    {
        Name = "Taels",
        Credits = "Deviantart user Mew087123",

        Color = new(0xEAC700),
        Material = GD.Load<Material>("res://Characters/C03-Taels/Taels.tres"),
        Icon = GD.Load<Texture>("res://Characters/C03-Taels/TaelsIcon.png"),
        MinimapIcon = GD.Load<Texture>("res://Characters/C03-Taels/TaelsMinimap.png")
    };
}
