using Godot;

namespace Sanicball;

public partial class Environment : WorldEnvironment
{
    public static Environment? Instance { get; private set; } = null;

    public static void FadeIn()
    {
        if (Instance is null) { return; }

        Instance.animationPlayer.Play("AutoloadAnims/FadeInScene");
    }

    public static void FadeOut()
    {
        if (Instance is null) { return; }

        Instance.animationPlayer.Play("AutoloadAnims/FadeOutScene");
    }

    public AnimationPlayer animationPlayer = new();

    public override void _EnterTree() { Instance = this; }
    public override void _ExitTree() { Instance = null; }

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
}