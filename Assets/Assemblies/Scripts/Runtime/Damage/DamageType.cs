using System;

namespace Thijs.Platformer
{
    [Flags]
    public enum DamageType
    {
        None,
        Impact = 1,
        Bludgeoning = 2,
        Explosion = 4,
    }
}