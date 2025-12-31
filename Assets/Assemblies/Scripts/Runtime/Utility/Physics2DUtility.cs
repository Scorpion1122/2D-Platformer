using UnityEngine;

namespace Thijs.Platformer.Utility
{
    public static class Physics2DUtility
    {
        public static ContactPoint2D GetBiggestImpactContact(Collision2D other)
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