using UnityEngine;

namespace Sanicball
{
    public class RandomTexture : MonoBehaviour
    {
        public Texture[] textures;
        private int current;
        private int Current
        {
            get => current;
            set 
            {
                GetComponent<Renderer>().material.mainTexture = textures[value];
                current = value;
            }
        }

        private void Start()
        {
            int m = Random.Range(0, textures.Length);
            Current = m;
        }
    }
}