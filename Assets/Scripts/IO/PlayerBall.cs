using System;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;
using Sanicball.Gameplay;
using Sanicball.Logic;
using Sanicball.UI;
using Sanicball.Data;

namespace Sanicball.Ball
{
    public class PlayerBall : AbstractBall
    {
        //Public fields needed to discriminate
        public ControlType CtrlType { get; private set; } = ControlType.Keyboard;
        public int CharacterId { get; private set; } = 1;
        public string Nickname { get; private set; } = "Player";


        //Private fields needed to do work
        bool hasJumped = false;
        public bool LapRecordsEnabled = false;
        Vector3 velocity = Vector3.zero;
        IBallCamera ballCamera;
        Marker checkpointMarker;
        Marker[] playerMarkers;

        //TODO make this destroy the player's ball when called
        #region Factory Methods

        public static PlayerBall Spawn(Rigidbody body, ControlType ctrlType, int charId, string name, IBallCamera camera)
        {
            PlayerBall player = body.AddComponent<PlayerBall>();
            player.Body = body;
            player.CtrlType = ctrlType;
            player.CharacterId = charId;
            player.Nickname = name;
            player.ballCamera = camera;
            //player.ballCamera.SetDirection(StageReferences.Checkpoints[0].transform.rotation);
            /* player.playerUI = PlayerUI.Create(camera, player);
            (player.checkpointMarker, player.playerMarkers) = Marker.CreateAll(new(), CurrentCheckpoint, camera); */
            return player;
        }

        #endregion Factory Methods

        protected override void OnRespawn()
        {
            ballCamera.SetDirection(CurrentCheckpoint.transform.rotation);
        }

        //GO FAST
        protected override Vector3 GetAcceleration()
        {
            if (PauseMenu.GamePaused) return Vector3.zero;
            Quaternion facingRotation = ballCamera.RotateCamera(Body);
            Quaternion rotationMatrix = facingRotation * Quaternion.Euler(0f, 90f, 0);

            const float weight = 0.5f;

            //Look at where we want to go and move it by how fast we can go
            Vector3 acceleration = CtrlType.MovementVector();
            velocity = Vector3.MoveTowards(velocity, acceleration, weight);

            //Rotate out absolute velocity towards our facing angle
            return rotationMatrix * Normalize(velocity);
        }

        private static Vector3 Normalize(Vector3 vector)
        {
            //Prevent a div zero
            if (vector == Vector3.zero) return Vector3.zero;

            //Normalisation
            float vectorMagnitude = vector.magnitude;
            vector /= vectorMagnitude;

            //Not sure
            double normalizedMag = Math.Pow(Mathf.Min(1, vectorMagnitude), 2);
            vector *= (float)normalizedMag;

            return vector;
        }

        protected override bool GetJump()
        {
            if (CtrlType.IsJumping())
            {
                if (hasJumped) return false;

                hasJumped = true;
                return true;
            }

            else hasJumped = false;
            return false;
        }

        protected override bool GetBraking() => CtrlType.IsBraking();
        protected override bool GetRespawn() => CtrlType.IsRespawning();

        private void FixedUpdate()
        {
            //! Check that if we can move check for respawn
            ApplyDirection();
            ApplyJump();
            ApplyBraking();
            ApplyRespawn();
        }

        private void Update()
        {
            ApplyEffects();
            new RemoteMovement(Body, GetAcceleration(), this.CtrlType).Send();
        }

        protected override void OnTriggerEnter(Collider collider)
        {
            collider.GetComponent<BoostPad>()?.Boost(Body);
        }

        protected override void OnFinishLine()
        {
            //Every time a player passes the finish line, check if it's done
            if (Lap <= Globals.settings.Laps) return;
            new RaceManager.DoneRacingMessage(Constants.guid, CtrlType, false).Send();
            
            if (!LapRecordsEnabled) return;
            Debug.Log("Saved lap record (" + TimeSpan.FromSeconds(lapTimer.Now()) + ")");
                
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            RaceRecord.records.Add(new RaceRecord(
                character.tier,
                lapTimer.Now(),
                DateTime.Now,
                ActiveData.Stages.First(a => a.sceneName == sceneName).id,
                CharacterId,
                checkpointTimes
                ));
        }
    }
}