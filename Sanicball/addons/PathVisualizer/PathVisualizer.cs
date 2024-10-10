#if TOOLS

using Godot;
using Sanicball.Scenes;
using Serilog;

namespace Sanicball.Plugins;

[GodotClass(Tool = true)]
internal partial class PathVisualizer : EditorPlugin
{
    private static readonly SphereMesh sphereMesh = GD.Load<SphereMesh>("res://addons/PathVisualizer/Sphere.tres");

    PathNode gizmo = new(sphereMesh);

    protected override void _EnterTree()
    {
        AddNode3DGizmoPlugin(gizmo);
    }

    protected override void _ExitTree()
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

    protected override string _GetGizmoName() => "PathNode";
    protected override bool _HasGizmo(Node3D forNode3D)
    {
        return forNode3D is MenuPath;
    }

    protected override void _Redraw(EditorNode3DGizmo gizmo)
    {
        gizmo.Clear();

        MenuPath path = (MenuPath)gizmo.GetNode3D();

        gizmo.AddMesh(ball, path.startMat, path.startTranform);
        gizmo.AddMesh(ball, path.endMat, path.endTranform);
        gizmo.AddLines([path.Start, path.End], GetMaterial("main", gizmo));
        gizmo.AddHandles([path.Start, path.End], GetMaterial("mine", gizmo), []);
    }

    protected override Variant _GetHandleValue(EditorNode3DGizmo gizmo, int handleId, bool secondary)
    {
        MenuPath path = (MenuPath)gizmo.GetNode3D();
        if (handleId == 0) { return path.Start; }
        else { return path.End; }
    }

    protected override void _SetHandle(EditorNode3DGizmo gizmo, int handleId, bool secondary, Camera3D camera, Vector2 screenPos)
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
            Log.Warning("Too far, lost intersection");
        }
    }
}

#endif
