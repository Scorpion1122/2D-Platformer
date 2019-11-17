using System;

namespace Thijs.Platformer.Characters
{
    [Flags]
    public enum ActionIntent
    {
        None = 0,
        Jump = 1,
        Attack = 2,
    }
}