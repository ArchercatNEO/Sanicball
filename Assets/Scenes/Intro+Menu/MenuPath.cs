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
}
