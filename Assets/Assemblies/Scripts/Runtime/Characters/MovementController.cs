using UnityEngine;

namespace Thijs.Platformer.Characters
{
    public class MovementController : CharacterComponent
    {
        private static readonly int ANIM_KEY_WALK = Animator.StringToHash("IsRunning");
        private static readonly int ANIM_KEY_JUMPING = Animator.StringToHash("IsJumping");
        
        [SerializeField] private float groundSpeed = 10f;
        [SerializeField] private float airSpeed = 5f;
        [SerializeField] private float maxHorizontalSpeed = 1f;
        [SerializeField] private float stoppingDrag = 0.9f;
        
        [SerializeField] private float jumpForce = 100f;
        [SerializeField] private float maxJumpDelay = 0.33f;
        public bool IsJumping { get; private set; }
        public bool WasJumping { get; private set; }

        private void FixedUpdate()
        {
            Vector2 input = Character.MovementIntent;

            UpdateHorizontalVelocity(input);
            UpdateJumpingVelocity();
            LimitVelocity(input);
            UpdateVisuals(input);
            
            Character.UseActionIntent(ActionIntent.Jump);
        }

        private void UpdateHorizontalVelocity(Vector2 input)
        {
            Vector2 groundNormal = Character.GroundNormal;
            Vector2 velocity = Character.Rigidbody.linearVelocity;
            Vector2 targetVelocity = velocity;
            
            Vector2 horizontalMovement = new Vector2(input.x * GetMovementSpeed() * Time.fixedDeltaTime, 0f);
//            float angle = Vector2.SignedAngle(Vector2.up, groundNormal);
//            horizontalMovement = Quaternion.Euler(0, 0, angle) * horizontalMovement;

            targetVelocity += horizontalMovement;
            
            //Set the velocity
            Character.Rigidbody.linearVelocity = targetVelocity;
        }

        private void UpdateJumpingVelocity()
        {
            if (IsJumping && Character.Rigidbody.linearVelocity.y <= 0f)
            {
                IsJumping = false;
                WasJumping = true;
                return;
            }

            if (Character.IsGrounded && WasJumping)
            {
                WasJumping = false;
            }
            
            if (IsJumping || WasJumping || !Character.HasActionIntent(ActionIntent.Jump))
                return;

            float timeSinceGrounded = Time.time - Character.LastGroundTime;
            if (!Character.IsGrounded && timeSinceGrounded > maxJumpDelay)
                return;

            Vector2 force = Vector2.up * jumpForce;
            Character.Rigidbody.AddForce(force);
            IsJumping = true;
        }

        private void LimitVelocity(Vector2 input)
        {
            Vector2 velocity = Character.Rigidbody.linearVelocity;
            
            //Limit horizontal Speed
            velocity.x = Mathf.Clamp(velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed);
            if (input.x == 0f && Character.IsGrounded)
                velocity.x *= stoppingDrag;
            
            Character.Rigidbody.linearVelocity = velocity;
        }

        private void UpdateVisuals(Vector2 input)
        {
            FacingDirection facingDirection = Character.FacingDirection;
            if (input.x < 0f)
                facingDirection = FacingDirection.Left;
            if (input.x > 0f)
                facingDirection = FacingDirection.Right;
            Character.SetFacingDirection(facingDirection);
            
            Character.Animator.SetBool(ANIM_KEY_WALK, input.x != 0);
            Character.Animator.SetBool(ANIM_KEY_JUMPING, IsJumping);
        }

        private float GetMovementSpeed()
        {
            return Character.IsGrounded ? groundSpeed : airSpeed;
        }
    }
}
