using Godot;

namespace Sanicball.Characters;

public partial record class SanicCharacter
{
    public static readonly SanicCharacter Knackles = new()
    {
        Name = "Knackles",
        Credits = "an unknown artist",

        Color = new(0xFF0000),
        Material = GD.Load<Material>("res://Characters/C02-Knackles/Knackles.tres"),
        Icon = GD.Load<Texture>("res://Characters/C02-Knackles/KnacklesIcon.png"),
        MinimapIcon = GD.Load<Texture>("res://Characters/C02-Knackles/KnacklesMinimap.png")
    };
}
