using UnityEngine;

namespace Thijs.Platformer.Characters
{
    public class PlayerInput : CharacterComponent
    {
        
        private void Update()
        {
            Vector2 movementIntent = Vector2.zero;
            movementIntent.x = Input.GetAxis("Horizontal");
            movementIntent.y = Input.GetAxis("Vertical");
            Character.MovementIntent = movementIntent;

            if (Input.GetKeyDown(KeyCode.Space))
                Character.AddActionIntent(ActionIntent.Jump);
            
            if (Input.GetKeyDown(KeyCode.C))
                Character.AddActionIntent(ActionIntent.Attack);
        }
    }
}
