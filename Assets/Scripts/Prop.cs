﻿using SanicballCore;
using UnityEngine;

namespace Sanicball
{
    public class Prop : MonoBehaviour
    {
        [SerializeField] private Vector3 maxRandomRotation = new(20, 360, 20);
        [SerializeField] private float maxRandomScale = 0.4f;

        // Use this for initialization
        private void Start()
        {
            System.Random random = new(GetInstanceID());

            float randX = random.NextFloatUniform() * maxRandomRotation.x;
            float randY = random.NextFloatUniform() * maxRandomRotation.y;
            float randZ = random.NextFloatUniform() * maxRandomRotation.z;
            transform.rotation = Quaternion.Euler(randX, randY, randZ);

            Vector3 scale = transform.localScale;
            float randScale = random.NextFloatUniform() * maxRandomScale;

            transform.localScale = new Vector3(scale.x + randScale, scale.y + randScale, scale.x + randScale);
        }
    }
}