namespace HAD
{
    // 예시: 캐릭터의 공격, 마법, 대시 등의 행동을 정의하는 클래스
    public abstract class AbilityBase
    {
        public abstract AbilityTag Tag { get; }
        public CharacterBase Owner { get; set; } 
        
        public abstract string Name { get; }    

        public abstract void Execute();
    }
}
