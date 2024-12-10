namespace HAD
{
    public class CharacterAttribute
    {
        // Default + Buffed - DeBuffed
        public float MaxValue => DefaultValue + BuffedValue;

        public float CurrentValue { get; set; }
        public float DefaultValue { get; set; }
        public float BuffedValue { get; set; }
        // public float DeBuffedValue { get; set; }

        // 이벤트
        public System.Action<float, float> OnChangedEvent;
        public System.Action<float> OnChangedBuffed;

        // 여기 구조 자체를 바꿔도 되나?
        // default는 GameData
        // Buffed는 UserData
        // 얘는 그 둘의 합이니 TotalValue
        // 변경사항은 UserData에 저장 & 변경값은 여기에 +-될 뿐인데 네 가지 value가 필요한가?

        // float, int 구분해서 사용하고 싶음
    }
}
