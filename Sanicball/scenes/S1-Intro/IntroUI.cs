using System.Linq;
using Godot;
using Sanicball.Account;
using Sanicball.Plugins;

namespace Sanicball.Scenes;

/// <summary>
/// <list type="bullet">
/// 	<listheader>
/// 	This scene serves three purposes.
/// 	</listheader>
/// 	<item>
/// 		<term> Account Selection </term>
/// 		<description> Once we load the save files </description>
/// 	</item>
/// 	<item>
/// 		<term> Account Creation </term>
/// 		<description> If there's someone new </description>
/// 	</item>
/// 	<item>
/// 		<term> Account Instantiation </term>
/// 		<description> Setting the default values on new accounts </description>
/// 	</item>
/// </list>
/// </summary>
//TODO Implement account selection (guest/named account)
public partial class IntroUI : Control
{
    private static readonly PackedScene Scene = GD.Load<PackedScene>("res://scenes/S1-Intro/intro.tscn");

    [BindProperty] public required Control inputUi;
    [BindProperty] public required LineEdit usernameInput;
    [BindProperty] public required Control credits;

    protected override void _Ready()
    {
        var images = credits.GetChildren().OfType<TextureRect>();

        usernameInput.TextSubmitted += (newString) =>
        {
            AccountSettings.Active.name = usernameInput.Text;
            inputUi.Hide();

            Tween tween = CreateTween();

            foreach (TextureRect image in images)
            {
                tween.TweenProperty(image, new NodePath(":modulate"), new Color(1, 1, 1, 1), 0.6); //fade in
                tween.TweenInterval(0.6);
                tween.TweenProperty(image, new NodePath(":modulate"), new Color(1, 1, 1, 0), 0.6); //fade out
            }

            Callable switchToMenu = Callable.From(() =>
            {
                SceneTree tree = GetTree();
                MenuUI.Activate(tree);
            });

            tween.TweenCallback(switchToMenu);
        };
    }
}