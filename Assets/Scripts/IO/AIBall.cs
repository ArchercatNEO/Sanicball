using UnityEngine;
using Sanicball.Data;
using Sanicball.Logic;
using Sanicball.Gameplay;

namespace Sanicball.Ball
{
    public class AIBall : AbstractBall
    {
        public int charId;
        public bool canMove = false;
        public float targetPointMaxOffset;

        private AINode target = StageReferences.Checkpoints[0].FirstAINode;
        private Vector3 targetOffset = Vector3.zero;
        private Vector3 targetOffsetGoal = Vector3.zero;

        private const float AUTO_RESPAWN_TIME = 6.66f;
        private readonly Timer autoRespawn = new();

        private const float TARGET_OFFSET_TIME = 3.33f;
        private readonly Timer offsetReset = new();

        AIBall()
        {
            targetPointMaxOffset = Globals.settings.AISkill switch
            {
                SanicballCore.AISkillLevel.Retarded => 200,
                SanicballCore.AISkillLevel.Average => 20,
                SanicballCore.AISkillLevel.Dank => 0,
                _ => throw new System.InvalidOperationException("Invalid AI skill level")
            };

            autoRespawn.Start();
        }

        public static AIBall Spawn(Rigidbody body, int charId)
        {
            AIBall ball = body.gameObject.AddComponent<AIBall>();
            ball.name = "AI #" + charId;
            ball.charId = charId;
            return ball;
        }

        protected override Vector3 GetAcceleration()
        {
            targetOffset = Vector3.Lerp(targetOffset, targetOffsetGoal, Time.deltaTime);

            if (offsetReset.Finished(TARGET_OFFSET_TIME))
            {
                offsetReset.Reset();
                offsetReset.Start();
                targetOffsetGoal = Random.insideUnitSphere * Random.Range(0, targetPointMaxOffset);
            }

            if (!target) return Vector3.zero;

            Vector3 velocity = GetComponent<Rigidbody>().velocity;
            Quaternion towardsVelocity = (velocity != Vector3.zero) ? Quaternion.LookRotation(velocity) : Quaternion.LookRotation(target.transform.position);

            Ray ray = new(transform.position, towardsVelocity * Vector3.forward);

            float maxDist = Mathf.Max(0, Mathf.Min(velocity.magnitude * 1f, Vector3.Distance(transform.position, target.transform.position) - 35));

            Vector3 point = transform.position + (ray.direction * maxDist);

            /*RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDist, LayerMask.GetMask("Terrain")))
            {
                point = hit.point;
            }*/

            Vector3 targetPoint = target.transform.position + targetOffset;
            Debug.DrawLine(point, targetPoint, Color.white);

            Quaternion directionToGo = Quaternion.LookRotation(point - targetPoint);
            return directionToGo * Vector3.left;
        }

        protected override bool GetRespawn()
        {
            if (canMove && autoRespawn.Finished(AUTO_RESPAWN_TIME))
            {
                autoRespawn.Reset();
                autoRespawn.Start();
                return true;
            }

            return false;
        }

        protected override void OnRespawn()
        {
            base.OnRespawn();

            target = CurrentCheckpoint.FirstAINode;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            
            AINode? node = other.GetComponent<AINode>();
            if (node is not null && node == target && target.NextNode)
            {
                target = target.NextNode;
                autoRespawn.Reset();
            }

            Checkpoint? checkpoint = other.GetComponent<Checkpoint>();
            if (checkpoint is not null && checkpoint == NextCheckpoint)
            {
                target = checkpoint.FirstAINode;
            }
        }

        protected override void OnFinishLine()
        {
            
        }

        public override void OnFinishRace()
        {
            RaceManager.DoneRacingInner(this);
            enabled = false;
        }
    }
}