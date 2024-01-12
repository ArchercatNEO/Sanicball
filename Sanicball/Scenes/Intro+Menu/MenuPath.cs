using Godot;
using System;
using Sanicball.Characters;

namespace Sanicball.Scenes;

[Tool]
public partial class MenuPath : Node3D
{
    [Export] public Material CharacterMat = SanicCharacter.Sanic.Material;
    [Export] public Vector3 Start = Vector3.Zero;
    [Export] public Vector3 End = Vector3.Zero;

    [Export] MeshInstance3D? startNode;
    [Export] MeshInstance3D? endNode;
}

