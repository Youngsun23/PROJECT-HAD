namespace HAD
{
    public abstract class AbilityBase
    {
        public abstract AbilityTag Tag { get; }
        public CharacterBase Owner { get; set; } 
        
        public abstract string Name { get; }    

        public abstract void Execute();
    }
}
