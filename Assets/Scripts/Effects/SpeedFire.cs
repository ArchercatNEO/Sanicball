using UnityEngine;

namespace Sanicball
{
    public class SpeedFire : MonoBehaviour
    {
        static SpeedFire prefab => Resources.Load<SpeedFire>("Prefabs/Instantiated/Ball objects/SpeedFire");
        public static SpeedFire Create(Rigidbody body)
        {
            SpeedFire instance = Instantiate(prefab);
            instance.target = body;
            instance.transform.localScale = body.transform.localScale;
            instance.mr = instance.GetComponent<MeshRenderer>();
            instance.asrc = instance.GetComponent<AudioSource>();
            return instance;
        }

        Rigidbody target = null!;
        MeshRenderer mr = null!;
        AudioSource asrc = null!;

        // Update is called once per frame
        void Update()
        {
            if (!target)
            {
                Destroy(gameObject);
                return;
            }

            float power = Mathf.InverseLerp(120, 500, target.velocity.magnitude);
            power *= power;

            mr.material.color = new Color(1, 1, 1, power);
            asrc.volume = Mathf.Lerp(0, 0.4f, power);
            asrc.pitch = Mathf.Lerp(1.5f, 7f, power);

            Vector3 look = target.velocity;
            if (look == Vector3.zero)
            {
                look = Vector3.forward;
            }
            Quaternion q = Quaternion.LookRotation(look);
            q = Quaternion.AngleAxis(Random.Range(0, 360), target.velocity) * q;
            transform.rotation = q;
            transform.position = target.transform.position;
        }
    }
}