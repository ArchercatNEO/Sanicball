using System.Linq;
using Godot;
using Sanicball.Ball;

namespace Sanicball.Scenes;

/// <summary>
/// The top level handler of the lobby scene, 
/// mainly compiles information known by other nodes for purposes of networking and scene transitions
/// <see cref="CharacterSelect"/>
/// For information on where player input is handled
/// <see cref="LobbySettings"/>
/// For information on match settings are handled
/// </summary>
public partial class LobbyManager : Node
{
    public static LobbyManager? Instance { get; private set; }
    public override void _EnterTree() { Instance = this; }
    public override void _ExitTree() { Instance = null; }

    //Sub-managers that actually do stuff
    [Export] private CharacterSelectManager characterSelectManager = null!;
    [Export] private Label countdownText = null!;

    public override void _Ready()
    {
        characterSelectManager.OnReadyPlayerChange += (sender, readyPlayers) =>
        {
            //All the players that are actually in the game
            var players = characterSelectManager.activePanels
                .Where(panel => panel.Value.Player is not null)
                .Select(panel => (panel.Key, panel.Value.Player!))
                .ToDictionary();

            countdownText.Text = $"{readyPlayers}/{players.Count} players ready: waiting for more players";

            if (readyPlayers == players.Count)
            {
                Tween tween = CreateTween();
                
                void SetCountdownText(float time) => countdownText.Text = $"{readyPlayers}/{players.Count} players ready: Match starting in {time} seconds";
                tween.TweenMethod(Callable.From<float>(SetCountdownText), 5, 0, 5);
                
                void startRaceCallback() => RaceManager.Activate(GetTree(), TrackResource.GreenHillZone, players);
                tween.TweenCallback(Callable.From(startRaceCallback));
            }
        };
    }
}