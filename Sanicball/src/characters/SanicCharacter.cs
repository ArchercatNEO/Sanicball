using Godot;

namespace Sanicball.Characters;


[GodotClass]
public partial class SanicCharacter : Resource
{
    #region Names and credits

    /// <summary>
    /// The full name of the character, distinct from the name of the controller
    /// </summary>
    [BindProperty] public  string Name;

    /// <summary>
    /// The creator of all the art associated with the character
    /// </summary>
    [BindProperty] public  string Credits;

    #endregion Names and credits

    #region Materials, Colours and Icons

    /// <summary>
    /// The material that will be applied on the mesh of the ball
    /// </summary>
    [BindProperty] public  Material Material;

    /// <summary>
    /// A colour associated with the character, for use in markers and other ui elements
    /// </summary>
    [BindProperty] public  Color Color;

    /// <summary>
    /// The icon that will be used in the character select menu to display this character
    /// </summary>
    [BindProperty] public  Texture2D Icon;

    /// <summary>
    /// The icon displayed on a minimap during a race
    /// </summary>
    [BindProperty] public  Texture2D MinimapIcon;

    #endregion Materials, Colours and Icons

    #region Rendering and collision overrides

    /// <summary>
    /// If present, this mesh will be used instead of a ball, Aggmen and metal Sanic use this
    /// </summary>
    [BindProperty] public Mesh? MeshOverride;

    /// <summary>
    /// If present, this shape will be used for collision instead of a ball
    /// </summary>
    [BindProperty] public Shape3D? CollisionOverride;

    #endregion Rendering and collision overrides

    #region Physics stats

    /// <summary>
    /// The angular acceleration (in rad/s²) of this character while turning on the ground. Default is 1 rotation/s²
    /// </summary>
    [BindProperty] public float AngularAcceleration = 5 * Mathf.Tau;
    [BindProperty] public float MaxAngularSpeed = 10 * Mathf.Tau;
    [BindProperty] public float AirSpeed = 15;
    [BindProperty] public float JumpHeight = 14;
    [BindProperty] public float Grip = 20;
    [BindProperty] public float BallSize = 1;

    #endregion Physics stats
}
