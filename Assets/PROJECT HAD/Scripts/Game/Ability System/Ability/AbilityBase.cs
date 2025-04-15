namespace HAD
{
    public abstract class AbilityBase
    {
        public abstract AbilityTag Tag { get; }
        public abstract string Name { get; }
        public CharacterBase Owner { get; private set; }

        public abstract void Execute();

        public void SetOwner(CharacterBase chracter)
        {
            Owner = chracter;   
        }
    }
}
