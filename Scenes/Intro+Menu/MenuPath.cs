using Godot;

namespace Sanicball.Scenes;

[Tool]
public partial class MenuPath : Node3D
{
	[Export]
	Material CharacterMat = new();

	[Export]
	Transform3D Start;

	[Export]
	Transform3D End;

	public override void _Process(double delta)
	{
		//if (!Engine.IsEditorHint()) { return; }
		
		base._Ready();

		DebugDraw.Sphere(Start, 100, new(0x00ff00));
		DebugDraw.Line(Start.Origin, End.Origin, new(0x00ff00));
		DebugDraw.Sphere(End, 100, new(0xff0000));
	}
}
