using Godot;

namespace Sanicball.Scenes;

/// <summary>
/// A resource to represent what a stage track is and does
/// </summary>
[GodotClass]
public partial class TrackResource : Resource
{
    [BindProperty] public string name;
    //we can't use the PackedScene<T> because of the marshaller
    [BindProperty] public PackedScene RaceScene;
    [BindProperty] public Texture2D flatOverlay;
    [BindProperty] public PackedScene solidOverlay;
}
