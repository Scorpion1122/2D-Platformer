using System;
using UnityEngine;

namespace Thijs.Platformer
{
    public class Health : MonoBehaviour, IDamageReceiver
    {
        public event Action<Health, Damage> OnDamageReceived;
        
        [SerializeField] private DamageType allowedTypes;
        [SerializeField] private float amount;

        public float Amount => amount;

        public void GetDamaged(Damage damage)
        {
            if (amount <= 0f)
                return;
            
            if (!IsMatchingDamageType(damage.type))
                return;

            amount -= damage.amount;

            OnDamageReceived?.Invoke(this, damage);
            
            if (amount <= 0f)
                Destroy(gameObject);
        }

        private bool IsMatchingDamageType(DamageType type)
        {
            return (allowedTypes & type) != 0;
        }
    }
}
