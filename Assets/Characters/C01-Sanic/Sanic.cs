using Godot;

namespace Sanicball.Characters;

public partial record class SanicCharacter
{
	public static readonly SanicCharacter Sanic = new()
	{
		Name = "Sanic",
		Credits = "Deviantart user franz888",

		Color = new(0x004CFF),
		Material = GD.Load<Material>("res://Assets/Characters/C01-Sanic/Sanic.tres"),
		Icon = GD.Load<Texture>("res://Assets/Characters/C01-Sanic/SanicIcon.png"),
		MinimapIcon = GD.Load<Texture>("res://Assets/Characters/C01-Sanic/SanicMinimap.png")
	};
}
