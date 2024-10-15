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
        // public float DeBuffedValue { get; set; }

        // 여기 구조 자체를 바꿔도 되나?
        // default는 GameData
        // Buffed는 UserData
        // 얘는 그 둘의 합이니 TotalValue
        // 변경사항은 UserData에 저장 & 변경값은 여기에 +-될 뿐인데 네 가지 value가 필요한가?
    }
}
