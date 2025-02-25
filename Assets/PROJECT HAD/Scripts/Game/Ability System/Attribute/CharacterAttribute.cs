namespace HAD
{
    public class CharacterAttribute
    {
        public float MaxValue => DefaultValue + BuffedValue;

        public float CurrentValue { get; set; }
        public float DefaultValue { get; set; }
        public float BuffedValue { get; set; }

        public System.Action<float, float> OnChangedEvent;
        public System.Action<float> OnChangedBuffed;

    }
}
