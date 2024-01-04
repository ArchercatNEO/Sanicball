using System;
using System.Collections.Generic;
using UnityEngine;
using Sanicball.Data;
using Sanicball.Gameplay;
using Sanicball.Logic;
using Sanicball.UI;
using CharacterInfo = Sanicball.Data.CharacterInfo;

namespace Sanicball.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class AbstractBall : MonoBehaviour
    {
        //Checkpoint related stuff
        private static readonly Checkpoint[] checkpoints = StageReferences.Checkpoints ?? new Checkpoint[]{};
        protected int currentCheckpointIndex;
        protected int Lap => (int)Math.Floor((double)(currentCheckpointIndex / checkpoints.Length)) + 1;
        protected Checkpoint CurrentCheckpoint => checkpoints[currentCheckpointIndex % checkpoints.Length];
        protected Checkpoint NextCheckpoint => checkpoints[(currentCheckpointIndex + 1) % checkpoints.Length];
        protected float[] checkpointTimes = new float[checkpoints.Length];
        protected readonly Timer lapTimer = new();

        public PlayerUI? playerUI; //This is only has a value in PlayerBalls
        private readonly Marker checkpointMarker;
        private readonly IEnumerable<Marker> playerMarkers;
        
        
        protected bool IsGrounded { get; set; }

        readonly Timer upResetTimer = new();
        readonly Timer groundedTimer = new();

        protected Rigidbody Body { get; set; }
        public CharacterInfo character;
        readonly BallStats BallStats = new();
        public Vector3 Up;

        protected abstract Vector3 GetAcceleration();
        protected virtual bool GetJump() => false;
        protected virtual bool GetBraking() => false;
        protected virtual bool GetRespawn() => false;

        void Start()
        {
            Body.maxAngularVelocity = float.MaxValue;
        }

        /// <summary>
        /// Apply acceleration and torque
        /// </summary>
        /// <param name="ball"></param>
        protected void ApplyDirection()
        {
            Vector3 direction = GetAcceleration();
            if (direction == Vector3.zero) return;

            /* Debug.Log(direction);
            Debug.Log(BallStats.rollSpeed); */

            //Rotation speed
            Body.AddTorque(direction * BallStats.rollSpeed);

            if (IsGrounded)
            {
                //TODO
                /* Vector3 grip = -Up * BallStats.grip * (Body.velocity.magnitude/400);
                Body.AddForce(grip); */
            }
            else
            {
                //??????????????????????????????????????
                Vector3 force = Quaternion.Euler(0, -90, 0) * direction * BallStats.airSpeed;
                Body.AddForce(force);
            };
        }

        /// <summary>
        /// If applicable, add force required to jump
        /// </summary>
        /// <param name="ball"></param>
        protected void ApplyJump()
        {
            if (!GetJump()) return;
            if (!IsGrounded) return;

            Body.AddForce(Up * BallStats.jumpHeight, ForceMode.Impulse);
            DriftAudio.JumpEffect();
            IsGrounded = false;
        }

        //TODO add auto-break back in
        /// <summary>
        /// Reset angular velocity based on <c>GetBraking</c>'s truthyness 
        /// </summary>
        /// <param name="ball"></param>
        protected void ApplyBraking()
        {
            //Always brake when AutoBrake is on
            if (GetBraking())
                Body.angularVelocity = Vector3.zero;
        }

        protected void ApplyRespawn()
        {
            if (GetRespawn())
                Body.angularVelocity = Vector3.zero;
        }

        /// <summary>
        /// Apply all applicable sound and particle effects
        /// </summary>
        /// <param name="ball"></param>
        protected void ApplyEffects()
        {
            if (IsGrounded)
            {
                float rollSpd = Mathf.Clamp(Body.angularVelocity.magnitude / 230, 0, 16);
                float vel = (-128f + Body.velocity.magnitude) / 256; //Start at 128 fph, end at 256
                vel = Mathf.Clamp(vel, 0, 1);

                DriftAudio.RollFadeIn(rollSpd);
                DriftAudio.SpeedFadeIn(vel);
                if (Body.IsDrift())
                {
                    DriftAudio.BrakeFadeIn();
                    PfxFactory.CreateSmoke(transform.position);
                }
                

                if (groundedTimer.Finished(0))
                {
                    IsGrounded = false;
                    upResetTimer.Start();
                    groundedTimer.Reset();
                }
            }
            else
            {
                DriftAudio.RollFadeOut();
                DriftAudio.SpeedFadeOut();
                DriftAudio.BrakeFadeOut();

                if (upResetTimer.Finished(1))
                {
                    Up = Vector3.MoveTowards(Up, Vector3.up, Time.deltaTime * 10);
                    upResetTimer.Reset();
                }
            }
        }

        //TODO Make this a dedicated constructor
        protected void ApplyCharacter(CharacterInfo character)
        {
            Rigidbody body = Body;

            body.transform.localScale = new Vector3(character.ballSize, character.ballSize, character.ballSize);

            body.GetComponent<Renderer>().material = character.material;
            body.GetComponent<TrailRenderer>().material = character.trail;
            if (character.name == "Super Sanic" && GameSettings.Instance.eSportsReady)
            {
                body.GetComponent<TrailRenderer>().material = ESportMode.Trail;
            }
            if (character.alternativeMesh != null)
            {
                body.GetComponent<MeshFilter>().mesh = character.alternativeMesh;
            }
            //set collision mesh too
            if (character.collisionMesh == null) return;
            if (character.collisionMesh.vertexCount > 255)
            {
                Debug.LogWarning("Vertex count for " + character.name + "'s collision mesh is bigger than 255!");
                return;
            }

            Destroy(body.GetComponent<Collider>());
            MeshCollider mc = body.gameObject.AddComponent<MeshCollider>();
            mc.sharedMesh = character.collisionMesh;
            mc.convex = true;
        }

        void OnDestroy()
        {
            PfxFactory.CreateRemovalPfx(transform);
        }

        void OnCollisionStay(Collision c)
        {
            IsGrounded = true;
            Up = c.GetContact(0).normal;
        }

        //Doing this with timers makes coyote time possible
        void OnCollisionExit(Collision c)
        {
            groundedTimer.Start();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Up);
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            Checkpoint? checkpoint = collider.GetComponent<Checkpoint>();
            if (checkpoint is not null && checkpoint == NextCheckpoint)
            {
                checkpointTimes[currentCheckpointIndex] = lapTimer.Now();

                playerUI?.NextCheckpointPassed(TimeSpan.FromSeconds(lapTimer.Now()));
                playerUI?.StoreRecord(lapTimer.Now(), currentCheckpointIndex, character.tier);
            
                currentCheckpointIndex++;
                //Toggle markers instead of changing the target
                checkpointMarker.Target = CurrentCheckpoint.transform;

                //if (FinishReport != null || Lap <= Globals.settings.Laps) return;
                if (currentCheckpointIndex != 0) return;


                OnFinishLine();

                //Reset lap time
                lapTimer.Reset();
                lapTimer.Start();
                checkpointTimes = new float[checkpoints.Length];
            }

            TriggerRespawn? triggerRespawn = collider.GetComponent<TriggerRespawn>();
            if (triggerRespawn is not null)
            {
                playerUI?.Respawned();

                transform.position = CurrentCheckpoint.GetRespawnPoint() + Vector3.up * transform.localScale.x * 0.5f;
                Body.velocity = Vector3.zero;
                Body.angularVelocity = Vector3.zero;

                //Time penalty
                lapTimer.AddTime(5);
            }
        }

         /// <summary>
        /// This function returns race progress as laps done (1..*) + progress to next lap (0..1)
        /// </summary>
        /// <returns></returns>
        public float CalculateRaceProgress()
        {
            //If the race is finished, ignore lap progress and just return the current lap (Which would be 1 above the number of laps in the race)
            //if (RaceFinished) { return Lap; }

            Vector3 currentCheckpointPos = CurrentCheckpoint.transform.position;
            float progPerCheckpoint = 1f / checkpoints.Length;

            Vector3 nextPos = NextCheckpoint.transform.position;
            float ballToNext = Vector3.Distance(transform.position, nextPos);
            float currToNext = Vector3.Distance(currentCheckpointPos, nextPos);

            float distToNextPercentile = 1f - Mathf.Clamp(ballToNext / currToNext, 0f, 1f);

            float distToNextProg = distToNextPercentile * progPerCheckpoint;
            float lapProg = currentCheckpointIndex * progPerCheckpoint + distToNextProg;

            return Lap + lapProg;
        }

        protected abstract void OnFinishLine();
        protected virtual void OnRespawn() {}

        public void FinishRace()
        {
            //if (FinishReport != null) throw new InvalidOperationException("RacePlayer tried to finish a race twice for some reason");

            //Set layer to Racer Ghost to block collision with racing players
            gameObject.layer = LayerMask.NameToLayer("Racer Ghost");
            

            return ;
        }

        public virtual void OnFinishRace() {}
    }
}