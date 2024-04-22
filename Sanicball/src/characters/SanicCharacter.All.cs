using Godot;

namespace Sanicball.Characters;

public partial class SanicCharacter
{
    public static readonly SanicCharacter Unknown = GD.Load<SanicCharacter>("res://src/characters/C00-Default/Unknown.tres");
    public static readonly SanicCharacter Sanic = GD.Load<SanicCharacter>("res://src/characters/C01-Sanic/Sanic.tres");
    public static readonly SanicCharacter Knackles = GD.Load<SanicCharacter>("res://src/characters/C02-Knackles/Knackles.tres");
    public static readonly SanicCharacter Taels = GD.Load<SanicCharacter>("res://src/characters/C03-Taels/Taels.tres");
    public static readonly SanicCharacter Ame = GD.Load<SanicCharacter>("res://src/characters/C04-Ame-Roes/Ame.tres");
    public static readonly SanicCharacter Shedew = GD.Load<SanicCharacter>("res://src/characters/C05-Shedew/Shedew.tres");
    public static readonly SanicCharacter Roge = GD.Load<SanicCharacter>("res://src/characters/C06-Roge-da-Bat/Roge.tres");
    public static readonly SanicCharacter Asspio = GD.Load<SanicCharacter>("res://src/characters/C07-Asspio/Asspio.tres");
    public static readonly SanicCharacter Big = GD.Load<SanicCharacter>("res://src/characters/C08-Big-da-Cat/Big.tres");
    public static readonly SanicCharacter Aggmen = GD.Load<SanicCharacter>("res://src/characters/C09-Dr.-Aggmen/Aggmen.tres");
    public static readonly SanicCharacter ChermyBee = GD.Load<SanicCharacter>("res://src/characters/C10-Chermy-Bee/Chermy.tres");
    public static readonly SanicCharacter Sulver = GD.Load<SanicCharacter>("res://src/characters/C11-Sulver/Sulver.tres");
    public static readonly SanicCharacter Bloze = GD.Load<SanicCharacter>("res://src/characters/C12-Bloze/Bloze.tres");
    public static readonly SanicCharacter Vactor = GD.Load<SanicCharacter>("res://src/characters/C13-Vactor/Vactor.tres");
    public static readonly SanicCharacter MetalSanic = GD.Load<SanicCharacter>("res://src/characters/C14-Metal-Sanic/MetalSanic.tres");
    public static readonly SanicCharacter Ogre = GD.Load<SanicCharacter>("res://src/characters/C15-Ogre/Ogre.tres");
    public static readonly SanicCharacter SupaSanic = GD.Load<SanicCharacter>("res://src/characters/C99-Supa-Sanic/SupaSanic.tres");

    public static readonly SanicCharacter[] All = [
        Sanic,
        Knackles,
        Taels,
        Ame,
        Shedew,
        Roge,
        Asspio,
        Big,
        Aggmen,
        ChermyBee,
        Sulver,
        Bloze,
        Vactor,
        MetalSanic,
        Ogre,
        SupaSanic
    ];
}