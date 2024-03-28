using Godot;

namespace Sanicball.Characters;

public partial class SanicCharacter
{
    public static readonly SanicCharacter Unknown = GD.Load<SanicCharacter>("res://characters/C00-Default/Unknown.tres");
    public static readonly SanicCharacter Sanic = GD.Load<SanicCharacter>("res://characters/C01-Sanic/Sanic.tres");
    public static readonly SanicCharacter Knackles = GD.Load<SanicCharacter>("res://characters/C02-Knackles/Knackles.tres");
}