using Godot;
using Sanicball.Scenes;
using System;

namespace Sanicball.Tools;

[Tool]
internal partial class PathVisualizer : EditorPlugin
{
	PathNode gizmo = new();

    public override void _EnterTree()
    {
        base._EnterTree();

        AddNode3DGizmoPlugin(gizmo);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        RemoveNode3DGizmoPlugin(gizmo);
    }
}

internal partial class PathNode : EditorNode3DGizmoPlugin
{
    private Mesh ball = null!;
    
    public PathNode()
    {
        CreateMaterial("main", new Color(1, 0, 0));
        CreateHandleMaterial("mine");
        Callable.From(() => { ball = GD.Load<SphereMesh>("res://Scenes/Intro+Menu/ball.tres"); }).CallDeferred();
    }

    public override string _GetGizmoName() => "PathNode";
    public override bool _HasGizmo(Node3D forNode3D) => forNode3D is MenuPath;

    public override void _Redraw(EditorNode3DGizmo gizmo)
    {
        base._Redraw(gizmo);

        gizmo.Clear();

        MenuPath path = (MenuPath)gizmo.GetNode3D();

        Transform3D startNode = Transform3D.Identity with { Origin = path.Start };
        gizmo.AddMesh(ball, path.CharacterMat, startNode);

        Transform3D endNode = Transform3D.Identity with { Origin = path.End };
        gizmo.AddMesh(ball, path.CharacterMat, endNode);
    }
}
