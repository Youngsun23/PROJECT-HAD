using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    [System.Flags]
    // 
    public enum AbilityTag
    {
        None = 0,
        Reflection = 1 << 0,
        Push = 1 << 1,
        Revenge = 1 << 2,

        Attack = 1 << 26,
        SpecialAttack = 1 << 27,
        Magic = 1 << 28,
        Dash = 1 << 29,
        Gauge = 1 << 30,
        Keepsake = 1 << 31,
    }
}
