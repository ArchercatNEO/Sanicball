using Godot;

namespace Sanicball.Characters;

public partial class SanicCharacter
{
    public static readonly PackedScene Unknown = GD.Load<PackedScene>("res://characters/C00-Default/Ball.tscn");
    public static readonly PackedScene Sanic = GD.Load<PackedScene>("res://characters/C01-Sanic/Sanic.tscn");
    public static readonly PackedScene Knackles = GD.Load<PackedScene>("res://characters/C02-Knackles/Knackles.tscn");
}