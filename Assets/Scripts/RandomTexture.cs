using UnityEngine;

namespace Sanicball
{
    public class RandomTexture : MonoBehaviour
    {
        public Texture[] textures;

        private void Start()
        {
            GetComponent<Renderer>().material.mainTexture = textures.RandomItem(out int _);
        }
    }

    public static class RandomExtension
    {
        public static T RandomItem<T>(this T[] array, out int index)
        {
            index = Random.Range(0, array.Length);
            return array[index];
        }
    }
}