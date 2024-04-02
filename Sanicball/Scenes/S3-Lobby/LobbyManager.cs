using Godot;

namespace Sanicball.Scenes;

public partial class LobbyManager : Node
{
    public static LobbyManager? Instance { get; private set; }
    public override void _EnterTree() { Instance = this; }
    public override void _ExitTree() { Instance = null; }

    [Export] private Label countdownText = null!;

    private int _readyPlayers = 0;
    public int ReadyPlayers
    {
        get => _readyPlayers;
        set
        {
            _readyPlayers = value;
            if (_readyPlayers > 0)
            {
                Tween tween = CreateTween();
                
                Callable setCoundownCallback = Callable.From<float>(SetCountdownText);
                tween.TweenMethod(setCoundownCallback, 5, 0, 5);

                tween.TweenCallback(Callable.From(() => TrackResource.GreenHillZone.Activate(GetTree())));
            }
        }
    }

    private void SetCountdownText(float time) => countdownText.Text = $"1/1 players ready: Match starting in {time} seconds";
}