using UnityEngine;

namespace Thijs.Platformer.Characters
{
    public class MovementController : CharacterComponent
    {
        [SerializeField] private float groundSpeed = 10f;
        [SerializeField] private float airSpeed = 5f;
        [SerializeField] private float maxHorizontalSpeed = 1f;
        [SerializeField] private float stoppingDrag = 0.9f;
        
        private void FixedUpdate()
        {
            Vector2 input = Character.MovementIntent;

            UpdateVelocity(input);
            UpdateVisuals(input);
        }

        private void UpdateVelocity(Vector2 input)
        {
            Vector2 groundNormal = Character.GroundNormal;
            Vector2 velocity = Character.Rigidbody.velocity;
            Vector2 targetVelocity = velocity;
            
            Vector2 horizontalMovement = new Vector2(input.x * GetMovementSpeed() * Time.fixedDeltaTime, 0f);
//            float angle = Vector2.SignedAngle(Vector2.up, groundNormal);
//            horizontalMovement = Quaternion.Euler(0, 0, angle) * horizontalMovement;

            targetVelocity += horizontalMovement;
            
            //Limit horizontal Speed
            targetVelocity.x = Mathf.Clamp(targetVelocity.x, -maxHorizontalSpeed, maxHorizontalSpeed);
            if (input.x == 0f && Character.IsGrounded)
                targetVelocity.x *= stoppingDrag;
            
            //Set the velocity
            Character.Rigidbody.velocity = targetVelocity;
        }

        private void UpdateVisuals(Vector2 input)
        {
            FacingDirection facingDirection = Character.FacingDirection;
            if (input.x < 0f)
                facingDirection = FacingDirection.Left;
            if (input.x > 0f)
                facingDirection = FacingDirection.Right;
            Character.SetFacingDirection(facingDirection);
        }

        private float GetMovementSpeed()
        {
            return Character.IsGrounded ? groundSpeed : airSpeed;
        }
    }
}
