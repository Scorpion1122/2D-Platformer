using UnityEngine;

namespace Thijs.Platformer.Characters
{
    [RequireComponent(typeof(Character))]
    public abstract class CharacterComponent : MonoBehaviour
    {
        public Character Character { get; private set; }

        protected virtual void Awake()
        {
            Character = GetComponent<Character>();
        }
    }
}