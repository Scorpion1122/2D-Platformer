using UnityEngine;

namespace Thijs.Platformer
{
    public class DamageOnImpact : MonoBehaviour
    {
        private IDamageReceiver[] damageReceivers;

        [SerializeField] private LayerMask selfDamageMask;
        
        [SerializeField] private float damageModifier = 1f;
        [SerializeField] private float minImpulse = 10f;

        private void Awake()
        {
            damageReceivers = GetComponents<IDamageReceiver>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            ContactPoint2D contact = GetBestContactPoint(other);
            if (contact.normalImpulse < minImpulse)
                return;
            HandleSelfDamage(contact);
        }

        private void HandleSelfDamage(ContactPoint2D contact)
        {
            int layer = contact.otherCollider.gameObject.layer;

            if ((layer & 1 << selfDamageMask.value) == 0)
                return;
            
            Damage damage = new Damage()
            {
                type = DamageType.Impact,
                amount = damageModifier * contact.normalImpulse,
                normal = contact.normal,
                point = contact.point,
            };
            
            for (int i = 0; i < damageReceivers.Length; i++)
                damageReceivers[i].GetDamaged(damage);
        }

        private ContactPoint2D GetBestContactPoint(Collision2D other)
        {
            ContactPoint2D contact = other.contacts[0];

            for (int i = 1; i < other.contacts.Length; i++)
            {
                ContactPoint2D otherContact = other.contacts[i];
                if (contact.normalImpulse < otherContact.normalImpulse)
                    contact = otherContact;
            }

            return contact;
        }
    }
}