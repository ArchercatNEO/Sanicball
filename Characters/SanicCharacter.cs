using Godot;

namespace Sanicball.Characters;

public partial record class SanicCharacter
{
    //Useful metadata
    public required string Name { get; init; }
    public required string Credits { get; init; }

    //Colours and renderers
    public required Color Color { get; init; }
    public required Material Material { get; init; }
    public Material? Trail { get; init; } //TODO
    public required Texture Icon { get; init; }
    public required Texture MinimapIcon { get; init; }

    //These are for deformed characters (eggmen, metal sanic)
    public Mesh? AlternativeMesh { get; init; }
    public Mesh? CollisionMesh { get; init; }

    //Overwrite physics stats (big, bee)
    public float RollSpeed { get; init; } = 100;
    public float AirSpeed { get; init; } = 15;
    public float JumpHeight { get; init; } = 14;
    public float Grip { get; init; } = 20;
    public float BallSize { get; init; } = 1;
}

public static class CharacterExtension
{
    public static void SetCharacter(this RigidBody3D mesh, SanicCharacter character)
    {

    }
}