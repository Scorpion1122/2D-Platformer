using System;

namespace _Project.Scripts.Runtime
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