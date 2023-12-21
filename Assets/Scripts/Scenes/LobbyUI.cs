using UnityEngine;
using UnityEngine.SceneManagement;
using Sanicball.Ball;
using Sanicball.Data;
using Sanicball.Logic;
using Sanicball.Gameplay;

namespace Sanicball.Scenes
{
    public class LobbyUI : MonoBehaviour
    {
        public static Timer lobbyTimer = new();
        public static Timer autoStartTimer = new();
        public static float autoStartTimeout;

        public static void Load(float autoStart)
        {
            SceneManager.LoadScene(Constants.lobbyName);
            if (autoStart != 0)
            {
                autoStartTimer.Start();
                autoStartTimeout = autoStart;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            foreach (var (guid, client) in Client.clients)
            {
                foreach (var (control, player) in client.players)
                {
                    string name = $"{client.name} ({control.AsName()})";
                    var ball = LobbyBallSpawner.SpawnBall();
                    var body = ball.GetComponent<Rigidbody>();
                    if (guid == Constants.guid)
                    {
                        PlayerBall.Spawn(body, control, player.charId, name, LobbyCamera.Instance);
                    }
                    else
                    {
                        RemoteBall.LobbySpawn(body);
                    }
                }
            }
        }

        // Update is called once per frame
        private void Update()
        {
            ServerRelay.NextMessage(Scene.Lobby);
            
            float timer = Constants.StartRaceTimeout;
            LobbyReferences.CountdownField.text = "Match starts in " + Mathf.Max(1f, Mathf.Ceil(timer - lobbyTimer.Now()));

            //LoadRaceMessages will only be sent by the server in online mode
            if (ServerRelay.OnlineMode) return;
            if (!lobbyTimer.Finished(timer)) return;
            new LoadRaceMessage().Send();
        }

        public record LoadRaceMessage : Packet
        {
            public override void Consume()
            {
                lobbyTimer.Reset();
                CameraFade.StartAlphaFade(Color.black, false, 0.3f, 0.05f, () => {
                    foreach (var (_, client) in Client.clients)
                        foreach (var (_, player) in client.players)
                            player.readyToRace = false;

                    string targetStage = ActiveData.Stages[Globals.settings.StageId].sceneName;
                    SceneManager.LoadScene(targetStage);
                });
            }
        }

        public record AutoStartTimerMessage : Packet
        {
            private readonly float time; //! time of message 
            public override void Consume()
            {
                autoStartTimeout = Globals.settings.AutoStartTime - time;
            }
        }
    }

    public record LoadLobbyMessage : Packet
    {
        public override void Consume()
        {
            //!GotoLobby()
        }
    }
}
