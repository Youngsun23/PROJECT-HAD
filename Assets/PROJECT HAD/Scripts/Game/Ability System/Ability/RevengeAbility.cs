namespace HAD
{
    public class RevengeAbility : AbilityBase
    {
        public override AbilityTag Tag => AbilityTag.Revenge;
        public override string Name => "Revenge";
        private IActor attacker;
        private float originDamage;

        public override void Execute()
        {

        }

        public void Init(IActor abilityOwner, IActor actor, float damage)
        {
            actor.GetActor().GetComponent<CharacterBase>().TakeDamage(abilityOwner, damage * 0.25f);
        }
    }
}
