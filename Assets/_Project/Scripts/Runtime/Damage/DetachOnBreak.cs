using System;
using UnityEngine;

namespace Thijs.Platformer
{
    [RequireComponent(typeof(Health))]
    public class DetachOnBreak : MonoBehaviour
    {
        [SerializeField] private float detachForce = 100f;
        [SerializeField] private Rigidbody2D[] rigidbodies;
        private Health health;

        private void OnEnable()
        {
            health = GetComponent<Health>();
            health.OnDamageReceived += OnDamageReceived;
        }

        private void OnDisable()
        {
            health.OnDamageReceived -= OnDamageReceived;
        }

        private void OnDamageReceived(Health health, Damage damage)
        {
            if (health.Amount <= 0f)
                DetachObjects(damage);
        }

        private void DetachObjects(Damage damage)
        {
            for (int i = 0; i < rigidbodies.Length; i++)
            {
                Rigidbody2D rigidbody = rigidbodies[i];
                rigidbody.transform.parent = null;
                rigidbody.gameObject.SetActive(true);
                rigidbody.AddForceAtPosition(damage.normal * detachForce, damage.point);
            }
        }
    }
}