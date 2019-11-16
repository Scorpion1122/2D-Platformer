using UnityEngine;

namespace Thijs.Platformer.Characters
{
    public class PlayerInput : CharacterComponent
    {
        
        private void Update()
        {
            Vector2 movementIntent = Vector2.zero;

            movementIntent.x = Input.GetAxis("Horizontal");

            if (Input.GetKey(KeyCode.Space))
                movementIntent.y = 1f;

            Character.MovementIntent = movementIntent;
        }
    }
}
