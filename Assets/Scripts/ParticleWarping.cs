using UnityEngine;

namespace Sanicball
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleWarping : MonoBehaviour
    {
        public Vector3 limit;

        private ParticleSystem ps;

        private void Start()
        {
            ps = GetComponent<ParticleSystem>();
        }

        private void FixedUpdate()
        {
            if (Camera.main is null) { return; }

            Vector3 tpos = Camera.main.transform.position;
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[10000];
            int plength = ps.GetParticles(particles);
            
            for (int a = 0; a < plength; a++)
            {
                Vector3 f = particles[a].position;
                
                float x = f.x;
                if (f.x > tpos.x + limit.x) { x = f.x - limit.x * 2; }
                if (f.x < tpos.x + limit.x) { x = f.x + limit.x * 2; }

                float y = f.y;
                if (f.y > tpos.y + limit.y) { y = f.y - limit.y * 2; }
                if (f.y < tpos.y + limit.y) { y = f.y + limit.y * 2; }

                float z = f.z;
                if (f.z > tpos.z + limit.z) { z = f.z - limit.z * 2; }
                if (f.z < tpos.z + limit.z) { z = f.z + limit.z * 2; }

                particles[a].position = new Vector3(x, y, z);
            }
            
            ps.SetParticles(particles, plength);
        }
    }
}
