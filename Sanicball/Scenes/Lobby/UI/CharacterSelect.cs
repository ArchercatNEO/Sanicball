using Godot;

namespace Sanicball.Scenes;

public partial class CharacterSelect : Control
{
    private static readonly PackedScene prefab = GD.Load<PackedScene>("res://Scenes/Lobby/UI/CharacterSelect.tscn");
    
    public static CharacterSelect Create()
    {
        return prefab.Instantiate<CharacterSelect>();
    }
}