using System.Linq;
using Godot;
using Sanicball.Data;

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
[Tool]
public partial class IntroUI : Control
{
    [Export] public required Control input;
    
    public override void _Ready()
    {
        Control input = GetNode<Control>($"UsernameInput");
        LineEdit internalText = input.GetNode<LineEdit>($"TextEdit");
        internalText.TextSubmitted += (newString) =>
        {
            AccountSettings.Active.name = internalText.Text;
            input.Hide();

            AnimationPlayer[] animations = GetChildren()
                .Where(node => node is TextureRect)
                .Select(node => node.GetNode<AnimationPlayer>("AnimationPlayer"))
                .ToArray();

            animations[0].Play("FadeInOut");
            for (int i = 1; i < animations.Length; i++)
            {
                AnimationPlayer current = animations[i - 1];
                AnimationPlayer next = animations[i];

                current.AnimationFinished += (name) =>
                {
                    next.Play("FadeInOut");
                };
            }
            animations.Last().AnimationFinished += (name) =>
            {
                GetTree().ChangeSceneToFile("res://Scenes/Intro+Menu/menu.tscn");
            };
        };
    }
}


