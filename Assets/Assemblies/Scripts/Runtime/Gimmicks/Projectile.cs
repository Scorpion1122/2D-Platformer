using System.Collections.Generic;
using Thijs.Platformer.Utility;
using UnityEngine;

namespace Thijs.Platformer.Gimmicks
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D rigidbody;
        private Dictionary<GameObject, int> colliderObjects;
        private bool isProjectile;

        [SerializeField] private float startVelocity = 5f;
        [SerializeField] private float stopVelocity = 2f;
        [SerializeField] private float damageOnHit = 1f;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            colliderObjects = new Dictionary<GameObject, int>();
        }

        private void Start()
        {
            CacheLayers();
        }

        private void FixedUpdate()
        {
            float velocity = rigidbody.linearVelocity.magnitude;
            if (isProjectile && velocity < stopVelocity)
            {
                RevertToNonProjectile();
            }
            else if (!isProjectile && velocity >= startVelocity)
            {
                SetToProjectile();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (isProjectile)
                return;
            
            IDamageReceiver[] damageReceivers = GetDamageReceivers(other);

            ContactPoint2D contact = Physics2DUtility.GetBiggestImpactContact(other);
            Damage damage = new Damage()
            {
                amount = damageOnHit,
                normal = contact.normal,
                point = contact.point,
                type = DamageType.Impact,
            };
            
            for (int i = 0; i < damageReceivers.Length; i++)
            {
                damageReceivers[i].GetDamaged(damage);
            }
        }

        private IDamageReceiver[] GetDamageReceivers(Collision2D other)
        {
            GameObject root = other.gameObject;
            if (other.otherRigidbody != null)
                root = other.otherRigidbody.gameObject;

            return root.GetComponentsInChildren<IDamageReceiver>();
        }

        private void CacheLayers()
        {
            List<Collider2D> colliders = new List<Collider2D>();
            int colliderCount = rigidbody.GetAttachedColliders(colliders);
            
            for (int i = 0; i < colliderCount; i++)
            {
                Collider2D collider = colliders[i];
                if (!collider.enabled || colliderObjects.ContainsKey(collider.gameObject))
                    continue;
                
                colliderObjects.Add(collider.gameObject, collider.gameObject.layer);
            }
        }

        private void SetToProjectile()
        {
            foreach (GameObject gameObject in colliderObjects.Keys)
            {
                gameObject.layer = Layers.PROJECTILE;
            }
            isProjectile = true;
        }

        private void RevertToNonProjectile()
        {
            foreach (var pair in colliderObjects)
            {
                pair.Key.layer = pair.Value;
                gameObject.layer = Layers.PROJECTILE;
            }
            isProjectile = false;
        }

        private void OnValidate()
        {
            if (startVelocity < stopVelocity)
                stopVelocity = startVelocity;
        }
    }
}
