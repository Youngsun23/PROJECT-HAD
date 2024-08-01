using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class CharacterAttribute
    {
        public float MaxValue => DefaultValue + BuffedValue;

        public float CurrentValue { get; set; }
        public float DefaultValue { get; set; }
        public float BuffedValue { get; set; }
    }
}
