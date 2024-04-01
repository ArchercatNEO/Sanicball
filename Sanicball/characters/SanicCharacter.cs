using Godot;

namespace Sanicball.Characters;

[GlobalClass]
public partial class SanicCharacter : Resource
{
    //Useful metadata
    [Export] public required string Name;
    [Export] public required string Credits;

    //Colours and renderers
    [Export] public required Material Material;
    [Export] public required Color Color;
    [Export] public required Texture2D Icon;
    [Export] public required Texture2D MinimapIcon;

    //Rendering and collision overrrides (eggman, metal)
    [Export] public Mesh? MeshOverride;
    [Export] public Shape3D? CollisionOverride;

    //Physics stats overrides (big, bee)
    [Export] public float RollSpeed = 100;
    [Export] public float AirSpeed = 15;
    [Export] public float JumpHeight = 14;
    [Export] public float Grip = 20;
    [Export] public float BallSize = 1;
}