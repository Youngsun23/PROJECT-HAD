using System.Threading.Tasks;

namespace HAD
{
    public class AttackCombo3Command : ICommand
    {
        private PlayerCharacter character;
        private float awaitTime;

        public AttackCombo3Command(PlayerCharacter ch) 
        {
            character = ch;
        }

        async Task ICommand.Execute()
        {
            character.PerformAttackCombo();
            awaitTime = character.CharacterAttackComboController.AttackCombo(3);

            var effect = EffectPoolManager.Singleton.GetEffect("Attack3");
            effect.gameObject.transform.position = character.transform.position;
            effect.gameObject.transform.rotation = character.transform.rotation;

            await Awaitable.WaitForSecondsAsync(awaitTime);

            character.CharacterAttackComboController.Locomotion();
            character.ResetComboIndex();
        }
    }
}
