using System.Collections.Generic;
using UnityEngine;

namespace Thijs.Platformer.Characters
{
    public class MeleeAttackController : CharacterComponent
    {
        private static readonly int ANIM_KEY_ATTACK = Animator.StringToHash("Attack");
        
        [SerializeField] private BoxCollider2D hitCollider;
        [SerializeField] private float attackDelay = 0.3f;
        
        [SerializeField] private float hitForce = 500f;

        private List<Rigidbody2D> rigidbodyCache;
        private Collider2D[] colliderCache;
        private float lastAttackTime;
        
        protected override void Awake()
        {
            base.Awake();
            
            
            hitCollider.enabled = false;
            rigidbodyCache = new List<Rigidbody2D>();
            colliderCache = new Collider2D[20];
        }

        private void FixedUpdate()
        {
            if (!Character.HasActionIntent(ActionIntent.Attack))
                return;

            Character.UseActionIntent(ActionIntent.Attack);
            
            float timeSinceLastAttack = Time.time - lastAttackTime;
            if (timeSinceLastAttack < attackDelay)
                return;

            List<Rigidbody2D> rigidbodies = GetRigidbodiesInHitCollider();
            for (int i = 0; i < rigidbodies.Count; i++)
            {
                HitRigidbody(rigidbodies[i]);
            }
            
            Character.Animator.SetTrigger(ANIM_KEY_ATTACK);
            lastAttackTime = Time.time;
        }

        private void HitRigidbody(Rigidbody2D rigidbody)
        {
            Vector2 center = Character.GetCenter();
            Vector2 toOther = rigidbody.position - center;
            
            rigidbody.AddForce(toOther.normalized * hitForce);
        }

        private List<Rigidbody2D> GetRigidbodiesInHitCollider()
        {
            Vector2 center = hitCollider.transform.position;
            center += hitCollider.offset;

            int count = Physics2D.OverlapBoxNonAlloc(center, hitCollider.size, 0f, colliderCache);
            
            rigidbodyCache.Clear();
            for (int i = 0; i < count; i++)
            {
                Collider2D collider = colliderCache[i];
                Rigidbody2D rigidbody = collider.attachedRigidbody;
                if (rigidbody != null && rigidbody != Character.Rigidbody)
                    rigidbodyCache.Add(rigidbody);
            }

            return rigidbodyCache;

        }
    }
}