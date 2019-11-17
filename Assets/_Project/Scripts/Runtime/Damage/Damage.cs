using UnityEngine;

namespace Thijs.Platformer
{
    public struct Damage
    {
        public DamageType type;
        public float amount;
        public Vector2 point;
        public Vector2 normal;
    }
}