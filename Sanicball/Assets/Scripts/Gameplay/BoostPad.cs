﻿using UnityEngine;

namespace Sanicball.Gameplay
{
    internal struct PosRot
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public PosRot(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }

    public class BoostPad : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private float speedLimit = 200f;
        [SerializeField] private LayerMask placementLayers;

        private float offset;

        private PosRot CalcTargetPlacement()
        {
            Ray ray = new(transform.position, transform.rotation * Vector3.down);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, 100, placementLayers.value))
                return new PosRot(transform.position, transform.rotation);

            PosRot placement = new();
            placement.Position = hit.point;

            Quaternion alongNormal = Quaternion.FromToRotation(Vector3.up, hit.normal);
            float angle = transform.rotation.eulerAngles.y;
            placement.Rotation = Quaternion.AngleAxis(angle, hit.normal) * alongNormal;

            return placement;
        }

        public void Boost(Rigidbody ball)
        {
            GetComponent<AudioSource>()?.Play();

            float magnitude = ball.velocity.magnitude;
            float boost = Mathf.Min(magnitude + speed, speedLimit);
            ball.velocity = transform.rotation * Vector3.forward * boost;
        }

        private void Start()
        {
            PosRot placement = CalcTargetPlacement();
            transform.position = placement.Position;
            transform.rotation = placement.Rotation;
        }

        private void Update()
        {
            //Animate the panel on the boost pad
            offset -= 5f * Time.deltaTime;
            if (offset <= 0f)
            {
                offset += 1f;
            }
            GetComponent<Renderer>().materials[1].SetTextureOffset("_MainTex", new Vector2(0f, offset));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            PosRot placement = CalcTargetPlacement();

            Gizmos.DrawLine(transform.position, placement.Position);

            Gizmos.matrix = Matrix4x4.TRS(placement.Position, placement.Rotation, Vector3.one);
            Gizmos.DrawCube(Vector3.zero, new Vector3(5, 1, 10));
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}