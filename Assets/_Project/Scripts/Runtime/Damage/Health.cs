using UnityEngine;

namespace Thijs.Platformer
{
    public class Health : MonoBehaviour, IDamageReceiver
    {
        [SerializeField] private DamageType allowedTypes;
        [SerializeField] private float amount;
        
        public void GetDamaged(DamageType type, float amount)
        {
            throw new System.NotImplementedException();
        }
    }
}