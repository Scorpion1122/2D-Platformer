
using System;
using UnityEngine;

namespace Thijs.Platformer.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Character : MonoBehaviour
    {
        private const string ANIM_KEY_GROUNDED = "IsGrounded";
        
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private Collider2D[] colliders;
        private ActionIntent actionIntent;

        [Header("Visuals")] 
        [SerializeField] private Animator animator;
        [SerializeField] private Transform visualsRoot;
        private Vector2 visualsOffset;

        [Header("Grounded")]
        [SerializeField] private LayerMask groundedLayer;
        
        public Rigidbody2D Rigidbody => rigidbody;
        
        public FacingDirection FacingDirection { get; private set; }
        public Animator Animator => animator;
        
        public bool IsGrounded { get; private set; }
        public Vector2 GroundNormal { get; private set; }
        public float LastGroundTime { get; private set; }
        public Vector2 MovementIntent { get; set; }

        private void Awake()
        {
            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody2D>();

            if (colliders == null || colliders.Length == 0)
                colliders = GetComponentsInChildren<Collider2D>();

            visualsOffset = visualsRoot.localPosition;
        }

        private Bounds GetBounds()
        {
            bool first = true;
            Bounds bounds = new Bounds();
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider2D collider = colliders[i];
                if (!collider.enabled)
                    continue;

                if (first)
                    bounds = collider.bounds;
                else
                    bounds.Encapsulate(collider.bounds);
                first = false;
            }
            return bounds;
        }

        private void FixedUpdate()
        {
            UpdateGroundedState();
            
            Animator.SetBool(ANIM_KEY_GROUNDED, IsGrounded);
        }

        private void UpdateGroundedState()
        {
            Bounds bounds = GetBounds();
            
            Vector2 centerBottom = new Vector2(bounds.center.x, bounds.min.y);
            float width = bounds.extents.x * 0.9f;

            Vector2 origin = centerBottom + Vector2.up * width;

            RaycastHit2D hit = Physics2D.CircleCast(
                origin, 
                width, 
                Vector2.down, 
                0.05f, 
                groundedLayer.value);

            if (hit.collider == null)
            {
                IsGrounded = false;
                return;
            }

            IsGrounded = true;
            LastGroundTime = Time.time;
            GroundNormal = hit.normal;
        }

        public void SetFacingDirection(FacingDirection facingDirection)
        {
            if (facingDirection == FacingDirection)
                return;

            Vector3 scale = Vector3.one;
            if (facingDirection == FacingDirection.Left)
                scale.x = -1f;
            visualsRoot.localScale = scale;
            visualsRoot.localPosition = scale.x * visualsOffset;

            FacingDirection = facingDirection;
        }

        public void AddActionIntent(ActionIntent actionIntent)
        {
            this.actionIntent |= actionIntent;
        }

        public void UseActionIntent(ActionIntent actionIntent)
        {
            if (HasActionIntent(actionIntent))
                this.actionIntent ^= actionIntent;
        }

        public bool HasActionIntent(ActionIntent actionIntent)
        {
            return (this.actionIntent & actionIntent) != 0;
        }

        public Vector3 GetCenter()
        {
            return GetBounds().center;
        }
    }
}
