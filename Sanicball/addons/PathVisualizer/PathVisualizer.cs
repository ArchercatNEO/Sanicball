using Godot;
using Sanicball.Scenes;

namespace Sanicball.Plugins;

[Tool]
internal partial class PathVisualizer : EditorPlugin
{
    private static readonly SphereMesh sphereMesh = GD.Load<SphereMesh>("res://addons/PathVisualizer/Sphere.tres");

    PathNode gizmo = new(sphereMesh);

    public override void _EnterTree()
    {
        AddNode3DGizmoPlugin(gizmo);
    }

    public override void _ExitTree()
    {
        RemoveNode3DGizmoPlugin(gizmo);
    }
}

internal partial class PathNode : EditorNode3DGizmoPlugin
{
    private Mesh ball;

    public PathNode(Mesh sphereMesh)
    {
        CreateMaterial("main", new Color(1, 0, 0));
        CreateHandleMaterial("mine");
        ball = sphereMesh;
    }

    public override string _GetGizmoName() => "PathNode";
    public override bool _HasGizmo(Node3D forNode3D)
    {
        Variant maybeScript = forNode3D.GetScript();
        if (maybeScript.VariantType == Variant.Type.Nil)
        {
            return false;
        }

        if (maybeScript.As<Script>().ResourcePath == "res://scenes/S2-Menu/MenuPath.cs")
        {
            GD.Print("Menu path found");
            GD.Print(forNode3D.GetType());
        }

        if (forNode3D is MenuPath)
        {
            GD.Print("Also it's known as the type");
        }

        return forNode3D is MenuPath;
    }

    public override void _Redraw(EditorNode3DGizmo gizmo)
    {
        gizmo.Clear();

        MenuPath path = (MenuPath)gizmo.GetNode3D();

        gizmo.AddMesh(ball, path.startMat, path.startTranform);
        gizmo.AddMesh(ball, path.endMat, path.endTranform);
        gizmo.AddLines([path.Start, path.End], GetMaterial("main", gizmo));
        gizmo.AddHandles([path.Start, path.End], GetMaterial("mine", gizmo), []);
    }

    public override Variant _GetHandleValue(EditorNode3DGizmo gizmo, int handleId, bool secondary)
    {
        MenuPath path = (MenuPath)gizmo.GetNode3D();
        if (handleId == 0) { return path.Start; }
        else { return path.End; }
    }

    public override void _SetHandle(EditorNode3DGizmo gizmo, int handleId, bool secondary, Camera3D camera, Vector2 screenPos)
    {
        Vector3 origin = camera.ProjectRayOrigin(screenPos);
        Vector3 normal = camera.ProjectRayNormal(screenPos);
        Vector3? maybePosition = Plane.PlaneXY.IntersectsRay(origin, normal);
        if (maybePosition is Vector3 position)
        {
            MenuPath path = (MenuPath)gizmo.GetNode3D();
            if (handleId == 0) { path.Start = position; }
            else { path.End = position; }
        }
        else
        {
            GD.Print("Too far, lost intersection");
        }
    }
}
