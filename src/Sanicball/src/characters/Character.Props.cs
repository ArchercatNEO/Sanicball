using Godot;

namespace Sanicball.Characters;

partial class Character
{
    /// <summary>
    ///     Name of the skin (or character)
    /// </summary>
    public string SkinName { get; init; }

    /// <summary>
    ///     Author of all art assosiated with this skin
    /// </summary>
    public string Credits { get; init; }

    /// <summary>
    ///     The angular acceleration (in rad/s²) of this character while turning on the ground. Default is 1 rotation/s²
    /// </summary>
    protected float AngularAcceleration { get; init; } = 40;

    protected float MaxAngularSpeed { get; init; } = 100;
    protected float AirSpeed { get; init; } = 15;
    protected float JumpHeight { get; init; } = 14;

    /// <summary>
    ///     If present, this shape will be used for collision instead of a ball
    /// </summary>
    public Shape3D ShapeOverride
    {
        get;
        init
        {
            field = value;
            collider.Shape = value;
        }
    }

    /// <summary>
    ///     If present, this mesh will be used instead of a ball, Aggmen and metal Sanic use this
    /// </summary>
    public Mesh MeshOverride
    {
        get;
        init
        {
            field = value;
            renderer.Mesh = value;
        }
    }

    /// <summary>
    ///     The material that will be applied on the mesh of the ball
    /// </summary>
    public Material Material
    {
        get;
        init
        {
            field = value;
            renderer.MaterialOverride = value;
        }
    }

    /// <summary>
    ///     A colour associated with the character, for use in markers and other ui elements
    /// </summary>
    public Color Color { get; init; } = new(0, 0, 0);

    /// <summary>
    ///     The icon that will be used in the character select menu to display this character
    /// </summary>
    public Texture2D Icon { get; init; } = GD.Load<Texture2D>("res://src/characters/ball_icon.svg");

    /// <summary>
    ///     The icon displayed on a minimap during a race
    /// </summary>
    public Texture2D MinimapIcon { get; init; } =
        GD.Load<Texture2D>("res://src/characters/ball_icon.svg");
}
