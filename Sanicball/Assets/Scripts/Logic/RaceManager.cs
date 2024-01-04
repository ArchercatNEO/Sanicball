using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sanicball.Ball;
using Sanicball.Data;
using Sanicball.Gameplay;
using Sanicball.UI;
using Object = UnityEngine.Object;

namespace Sanicball.Logic
{
    /// <summary>
    /// A tuple only useful to act as a key for RacePlayers.
    /// Not to be confused with <c>Sanicball.Data.Player</c>
    /// </summary>
    public readonly struct Player
    {
        public readonly Guid guid;
        public readonly ControlType ctrlType;

        public Player(Guid Guid, ControlType CtrlType)
        {
            guid = Guid;
            ctrlType = CtrlType;
        }
    }

    public class RaceManager : MonoBehaviour
    {
        //Race state
        public static List<AbstractBall> players = new();
        public static List<PlayerPortrait> entries = new();
        public static Dictionary<Player, AbstractBall> playerDict = new();

        public static List<RaceFinishReport> finishReports = new();
        private static bool isLoading = false;

        //Misc
        private static readonly Timer raceTimer = new();
        private static WaitingCamera? waitingCamera;
        private static WaitingUI? waitingUI;

        public static void StartRace()
        {
            raceTimer.Start();
            MusicPlayer.Play();
            foreach (AbstractBall player in players) { player.enabled = true; }
        }

        public static void Load(bool raceInProgress)
        {
            //Globals.settings.StageId
            Debug.Log("Loading");
            StageInfo targetStage = ActiveData.Stages[1];
            SceneManager.LoadScene(targetStage.sceneName);
            RaceManager.raceInProgress = raceInProgress;
        }

        private static bool raceInProgress;

        private void Awake()
        {
            Debug.Log("Waking up");
            //In online mode, send a RaceStartMessage as soon as the track is loaded (which is now)
            if (ServerRelay.OnlineMode) new StartRaceMessage().Send();

            //Otherwise shortcut to loading the race
            foreach (var (_, client) in Client.clients)
            {
                foreach (var (_, player) in client.players)
                {
                    player.readyToRace = false;
                }
            }

            if (raceInProgress)
            {
                Debug.Log("Starting race in progress");
                AudioListener.volume = GameSettings.Instance.soundVolume;

                raceTimer.Start();
                MusicPlayer.Play();
                CreateBallObjects();
                SpectatorView.Create();
                foreach (AbstractBall player in players) { player.enabled = true; }
            }
            else
            {
                isLoading = true;
                waitingCamera = WaitingCamera.Create();
                waitingUI = WaitingUI.Create();
            }

        }


        public static void DoneRacingInner(AbstractBall rp, double raceTime, bool disqualified)
        {
            int pos = RaceFinishReport.DISQUALIFIED_POS;
            if (!disqualified)
            {
                pos = playerDict
                    .Values
                    .OrderByDescending(a => a.CalculateRaceProgress())
                    .ToList()
                    .IndexOf(rp) + 1;
            }

            finishReports.Add(new RaceFinishReport(pos, TimeSpan.FromSeconds(raceTime)));
            //rp.playerPortrait.FinishRace(pos);
            rp.FinishRace();

            //Display scoreboard when all players have finished
            //TODO: Make proper scoreboard and have it trigger when only local players have finished
            if (players.Where(a => a is PlayerBall or RemoteBall).Count() == finishReports.Count)
            {
                //players.ForEach(player => player.Camera?.Remove());
                foreach (PlayerUI ui in FindObjectsOfType<PlayerUI>())
                {
                    Destroy(ui.gameObject);
                }
                
                EndOfMatch.Activate(finishReports);
            }
        }

        public static void DoneRacingInner(AbstractBall rp)
        {
            DoneRacingInner(rp, raceTimer.Now(), false);
        }

        #region Unity event functions


        public static void Update()
        {
            //In offline mode, send a RaceStartMessage once Space (Or A on any joystick) is pressed
            if (!ServerRelay.OnlineMode && isLoading && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)))
            {
                new StartRaceMessage().Send();
            }

            //Order player list by position
            entries = entries.OrderByDescending(a => a.CalculateRaceProgress()).ToList();
            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].ChangeUI(i + 1);
            }
        }

        //Call the Destroy method on all players to properly dispose them
        private static void OnDestroy()
        {
            players.Clear();
        }

        public static void CreateBallObjects()
        {
            //Enable lap records if there is only one local player.
            bool enableLapRecords = Client.clients[Constants.guid].players.Count == 1;
            int nextBallPosition = 0;
            
            //Create all player balls
            foreach (var (guid, client) in Client.clients)
            {
                //Create remote players then shortcircuit
                if (guid != Constants.guid)
                {
                    foreach (var (control, player) in client.players)
                    {
                        var chara = ActiveData.Characters[player.charId];
                        float size = chara.ballSize;
                        var ball = RaceBallSpawner.SpawnBall(size, nextBallPosition);
                        var body = ball.GetComponent<Rigidbody>();
                        RemoteBall remoteBall = RemoteBall.LobbySpawn(body);
                        players.Add(remoteBall);
                        nextBallPosition++;
                    }
                    continue;
                }

                //Create local players
                foreach (var (control, player) in client.players)
                {                
                    string name = $"{client.name} ({control.AsName()})";
                    var chara = ActiveData.Characters[player.charId];
                    float size = chara.ballSize;
                    var ball = RaceBallSpawner.SpawnBall(size, nextBallPosition);
                    var body = ball.GetComponent<Rigidbody>();
                    var camera = IBallCamera.Create(control);
                    //CameraSplitter.SetSpliScreen(camera, nextBallPosition);
                    
                    PlayerBall? racePlayer = PlayerBall.Spawn(
                        body,
                        control,
                        player.charId,
                        name,
                        camera
                        );

                    //Connect UI to camera (when the camera has been instanced)
                    racePlayer.LapRecordsEnabled = enableLapRecords;

                    //TODO implement for all balls
                    var entry = PlayerPortrait.Create(name, chara.color, racePlayer.CalculateRaceProgress);
                    
                    players.Add(racePlayer);
                    entries.Add(entry);
                    nextBallPosition++;
                }
            }

            //Create all AI balls (In local play only)
            if (ServerRelay.OnlineMode) return;
            for (int i = 0; i < Globals.settings.AICount; i++)
            {
                //Create ball
                int charId = Globals.settings.GetAICharacter(i);
                float size = ActiveData.Characters[charId].ballSize;
                var ball = RaceBallSpawner.SpawnBall(size, nextBallPosition);
                var body = ball.GetComponent<Rigidbody>();
                
                AIBall aiPlayer = AIBall.Spawn(body, charId);
                players.Add(aiPlayer);
                nextBallPosition++;
            }
        }


        #endregion Unity event functions

        public record DoneRacingMessage : Packet
        {
            private readonly Guid guid;
            private readonly ControlType ctrlType;
            private readonly bool disqualified;
            private readonly double raceTime;

            public DoneRacingMessage(Guid guid, ControlType ctrlType, bool disqualified)
            {
                this.guid = guid;
                this.ctrlType = ctrlType;
                this.disqualified = disqualified;
                this.raceTime = raceTimer.Now();
            }

            public override void Consume()
            {
                Player player = new(guid, ctrlType);
                AbstractBall rp = playerDict[player];

                DoneRacingInner(rp, raceTime, disqualified);
            }
        }

        public record RaceTimeoutMessage : Packet
        {
            private readonly Guid guid;
            private readonly ControlType ctrlType;
            private readonly double time;

            public override void Consume()
            {
                //! Player[guid, ctrlType].Timeout = (float)(argument.Time - time);
            }
        }

        public record StartRaceMessage : Packet
        {
            private readonly double time; //! The time it got sent

            public override void Consume()
            {                
                isLoading = false;

                AudioListener.volume = GameSettings.Instance.soundVolume;
                Object.Destroy(waitingCamera);
                Object.Destroy(waitingUI);

                //Create countdown
                RaceCountdown.Create((float) time);
                
                //Create all balls
                CreateBallObjects();

                //If there are no local players, create a spectator camera
                if (Client.clients[Constants.guid].players.Count < 1)
                {
                    SpectatorView.Create();
                }
            }
        }
    }
}