using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thijs.Platformer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D rigidbody;
        private Dictionary<GameObject, int> originalLayers;

        [SerializeField] private float startVelocity = 5f;
        [SerializeField] private float stopVelocity = 2f;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            originalLayers = new Dictionary<GameObject, int>();
        }

        private void Start()
        {
            CacheLayers();
        }

        private void CacheLayers()
        {
            List<Collider2D> colliders = new List<Collider2D>();
            int colliderCount = rigidbody.GetAttachedColliders(colliders);
            
            for (int i = 0; i < colliderCount; i++)
            {
                Collider2D collider = colliders[i];
                if (!collider.enabled || originalLayers.ContainsKey(collider.gameObject))
                    continue;
                
                originalLayers.Add(collider.gameObject, collider.gameObject.layer);
            }
            
        }

        private void OnValidate()
        {
            if (startVelocity < stopVelocity)
                stopVelocity = startVelocity;
        }
    }
}