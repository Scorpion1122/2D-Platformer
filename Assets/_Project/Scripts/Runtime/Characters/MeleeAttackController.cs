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
        [SerializeField] private float minForceAngle = 0f;
        [SerializeField] private float maxForceAngle = 45f;

        private Dictionary<Rigidbody2D, Collider2D> rigidbodyCache;
        private Collider2D[] colliderCache;
        private float lastAttackTime;
        
        protected override void Awake()
        {
            base.Awake();
            
            
            hitCollider.enabled = false;
            rigidbodyCache = new Dictionary<Rigidbody2D, Collider2D>();
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

            Dictionary<Rigidbody2D, Collider2D> rigidbodies = GetRigidbodiesInHitCollider();
            foreach (var hit in rigidbodies)
            {
                HitRigidbody(hit.Key, hit.Value);
            }
            
            Character.Animator.SetTrigger(ANIM_KEY_ATTACK);
            lastAttackTime = Time.time;
        }

        private void HitRigidbody(Rigidbody2D rigidbody, Collider2D collider)
        {
            Vector2 center = Character.GetCenter();
            Vector2 toOther = rigidbody.position - center;
            
            Vector2 forceDirection = new Vector2(toOther.x, 0f).normalized;
            Quaternion rotation = Quaternion.Euler(0, 0, forceDirection.x * Random.Range(minForceAngle, maxForceAngle));
            forceDirection = rotation * forceDirection;

            Vector2 forcePosition = collider.bounds.ClosestPoint(center);
            
            rigidbody.AddForceAtPosition(forceDirection * hitForce, forcePosition);
            Debug.DrawRay(forcePosition, forceDirection * hitForce, Color.red, 1f);
        }

        private Dictionary<Rigidbody2D, Collider2D> GetRigidbodiesInHitCollider()
        {
            Vector2 center = hitCollider.transform.TransformPoint(hitCollider.offset);

            int count = Physics2D.OverlapBoxNonAlloc(center, hitCollider.size, 0f, colliderCache);
            
            rigidbodyCache.Clear();
            for (int i = 0; i < count; i++)
            {
                Collider2D collider = colliderCache[i];
                Rigidbody2D rigidbody = collider.attachedRigidbody;
                if (rigidbody == null)
                    continue;

                if (rigidbody == Character.Rigidbody)
                    continue;

                if (rigidbodyCache.ContainsKey(rigidbody))
                    continue;
                
                rigidbodyCache.Add(rigidbody, collider);
            }

            return rigidbodyCache;

        }
    }
}