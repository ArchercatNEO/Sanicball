using Godot;

namespace Sanicball.Characters;

[GlobalClass]
public partial class SanicCharacter : Resource
{
    #region Names and credits

    /// <summary>
    /// The full name of the character, distinct from the name of the controller
    /// </summary>
    [Export] public required string Name;

    /// <summary>
    /// The creator of all the art associated with the character
    /// </summary>
    [Export] public required string Credits;

    #endregion Names and credits

    #region Materials, Colours and Icons

    /// <summary>
    /// The material that will be applied on the mesh of the ball
    /// </summary>
    [Export] public required Material Material;

    /// <summary>
    /// A colour associated with the character, for use in markers and other ui elements
    /// </summary>
    [Export] public required Color Color;

    /// <summary>
    /// The icon that will be used in the character select menu to display this character
    /// </summary>
    [Export] public required Texture2D Icon;

    /// <summary>
    /// The icon displayed on a minimap during a race
    /// </summary>
    [Export] public required Texture2D MinimapIcon;

    #endregion Materials, Colours and Icons

    #region Rendering and collision overrides

    /// <summary>
    /// If present, this mesh will be used instead of a ball, Aggmen and metal Sanic use this
    /// </summary>
    [Export] public Mesh? MeshOverride;

    /// <summary>
    /// If present, this shape will be used for collision instead of a ball
    /// </summary>
    [Export] public Shape3D? CollisionOverride;

    #endregion Rendering and collision overrides

    #region Physics stats

    /// <summary>
    /// The angular acceleration (in rad/s²) of this character while turning on the ground
    /// </summary>
    [Export] public float AngularAcceleration = 100;
    [Export] public float AirSpeed = 15;
    [Export] public float JumpHeight = 14;
    [Export] public float Grip = 20;
    [Export] public float BallSize = 1;

    #endregion Physics stats
}