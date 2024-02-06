using System.Diagnostics.CodeAnalysis;
using Godot;
using SourceGenerator;

namespace Sanicball.Characters;

public partial class SanicCharacter : Resource
{
    [SetsRequiredMembers]
    public SanicCharacter() {}

    //Useful metadata
    [Export] public required string Name;
    [Export] public required string Credits;

    //Colours and renderers
    [Export] public required Color Color;
    [Export] public required StandardMaterial3D Material;
    [Export] public Material? Trail;
    [Export] public required Texture Icon;
    [Export] public required Texture MinimapIcon;

    //These are for deformed characters (eggmen, metal sanic)
    [Export] public Mesh? AlternativeMesh;
    [Export] public Shape3D? CollisionMesh;

    //Overwrite physics stats (big, bee)
    [Export] public float RollSpeed = 100;
    [Export] public float AirSpeed = 15;
    [Export] public float JumpHeight = 14;
    [Export] public float Grip = 20;
    [Export] public float BallSize = 1;

    [Preload("characters/C01-Sanic/Sanic.tres")] public static readonly SanicCharacter Sanic;
    //public static readonly SanicCharacter Knackles = GD.Load<SanicCharacter>("characters/C02-Knackles/Knackles.tres");
    //public static readonly SanicCharacter Taels = GD.Load<SanicCharacter>("characters/C03-Taels/Taels.tres");
}