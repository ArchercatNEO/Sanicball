using Godot;
using Serilog;

namespace Sanicball.Scenes;

//TODO: Add animation paths and remove dependence on StandardMaterial3Ds
//TODO: Find the motivation to port debug draw 3D
[GodotClass]
public partial class MenuPath : Node3D
{
    private Material? characterMat;

    [BindProperty]
    public Material? CharacterMat
    {
        get => characterMat;
        set
        {
            characterMat = value;
            if (value == null)
            {
                Log.Error("A null character material will break the camera");
            }
        }
    }

    [BindProperty]
    public Vector3 Start { get; set; } = Vector3.Zero;

    [BindProperty]
    public Vector3 End { get; set; } = Vector3.Zero;
}
