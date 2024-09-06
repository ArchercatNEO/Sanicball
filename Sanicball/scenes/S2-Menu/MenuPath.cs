using Godot;

namespace Sanicball.Scenes;


//TODO Add animation paths and remove dependence on StandardMaterial3Ds
[GodotClass]
public partial class MenuPath : Node3D
{
    private StandardMaterial3D characterMat = new();
    [BindProperty]
    public StandardMaterial3D CharacterMat
    {
        get => characterMat;
        set
        {
            characterMat = value;

            startMat = (StandardMaterial3D)value.Duplicate(true);
            startMat.AlbedoColor = startColor;

            endMat = (StandardMaterial3D)value.Duplicate(true);
            endMat.AlbedoColor = endColor;
        }
    }

    public Transform3D startTranform = Transform3D.Identity;
    private Vector3 start = Vector3.Zero;
    [BindProperty]
    public Vector3 Start
    {
        get => start;
        set
        {
            start = value;
            startTranform.Origin = value;
        }
    }

    public  StandardMaterial3D startMat;
    private Color startColor = new(0, 1, 0, 1);
    [BindProperty]
    private Color StartColor
    {
        get => startColor;
        set
        {
            startColor = value;
            startMat.AlbedoColor = value;
        }
    }

    public Transform3D endTranform = Transform3D.Identity;
    private Vector3 end = Vector3.Zero;
    [BindProperty]
    public Vector3 End
    {
        get => end;
        set
        {
            end = value;
            endTranform.Origin = value;
        }
    }

    public  StandardMaterial3D endMat;
    private Color endColor = new(1, 0, 0, 1);
    [BindProperty]
    private Color EndColor
    {
        get => endColor;
        set
        {
            endColor = value;
            endMat.AlbedoColor = value;
        }
    }
}
