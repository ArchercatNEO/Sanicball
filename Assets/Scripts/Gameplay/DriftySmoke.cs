using UnityEngine;

namespace Sanicball.Gameplay
{
    public class PfxFactory : MonoBehaviour
    {
        private static ParticleSystem prefab => Resources.Load<ParticleSystem>("Prefabs/Instantiated/Ball objects/TireSmoke");
        private static readonly ParticleSystem pSystem;
        static PfxFactory()
        {
            pSystem = Instantiate(prefab);
        }

        #region Smoke
        private static ParticleSystem.EmitParams smokePool = new()
        {
            velocity = Vector3.zero,
            startLifetime = 5,
            startColor = Color.white
        };
        public static void TryCreateSmokePfx(Rigidbody target)
        {
            float speed = target.velocity.magnitude;
            float rot = target.angularVelocity.magnitude / 2;
            float angle = Vector3.Angle(target.velocity, Quaternion.Euler(0, -90, 0) * target.angularVelocity);

            if ((angle > 50 && (rot > 10 || speed > 10)) || (rot > 30 && speed < 30))
            {
                DriftAudio.BrakeFadeIn();
                smokePool.position = target.transform.position - new Vector3(0, +0.5f, 0) + Random.insideUnitSphere * 0.25f;
                smokePool.startSize = Random.Range(3f, 5f);
                pSystem.Emit(smokePool, 1);
            }
        }
        #endregion Smoke
        //TODO use arguments and the static pSystem instead of prefabs
        #region Removal
        //TODO: Create a special version of the particle system for Super Sanic that has a cloud of pot leaves instead. No, really.
        private static ParticleSystem removalPrefab => Resources.Load<ParticleSystem>("Prefabs/Instantiated/Ball objects/BallRemovalParticles");
        public static void CreateRemovalPfx(Transform transform)
        {
            Instantiate(removalPrefab, transform.position, transform.rotation);
        }
        #endregion Removal
    }

    public class DriftAudio : MonoBehaviour
    {
        #region Jump
        private static AudioClip Jump => Resources.Load<AudioClip>("Sound/Sfx/jump");
        public static void JumpEffect()
        {
            //I hate it too but conformity is better than rationality
            /* Jump.Play(); */
        }
        #endregion Jump

        #region Rolling
        private static AudioClip Roll => Resources.Load<AudioClip>("Sound/Sfx/rolling");
        public static void RollFadeIn(float rollSpeed)
        {
            /* Roll.pitch = Mathf.Max(rollSpeed, 0.8f);
            Roll.volume = Mathf.Min(rollSpeed, 1); */
        }
        public static void RollFadeOut()
        {
            /* Roll.volume = Mathf.Max(0, Roll.volume - 0.2f); */
        }
        #endregion Rolling

        #region Speed
        private static  AudioClip SpeedNoise => Resources.Load<AudioClip>("Sound/Sfx/speednoise");
        public static void SpeedFadeIn(float speed)
        {
            /* SpeedNoise.pitch = 0.8f + speed;
            SpeedNoise.volume = speed; */
        }
        public static void SpeedFadeOut()
        {
            /* SpeedNoise.volume = Mathf.Max(0, SpeedNoise.volume - 0.01f); */
        }
        #endregion Speed

        #region Braking
        private static AudioClip Brake => Resources.Load<AudioClip>("Sound/Sfx/brake");
        public static void BrakeFadeIn()
        {
            /* Brake.volume = Mathf.Min(Brake.volume + 0.5f, 1); */
            //aSource.pitch = 0.8f+Mathf.Min(rot/400f,1.2f);
        }
        public static void BrakeFadeOut()
        {
            /* Brake.volume = Mathf.Max(Brake.volume - 0.2f, 0); */
        }
        #endregion Braking
    }
}
