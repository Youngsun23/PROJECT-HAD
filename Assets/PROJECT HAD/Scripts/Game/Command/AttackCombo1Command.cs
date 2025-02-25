using System.Threading.Tasks;

// @ Command : 레시피 1 (이 재료로 이렇게 조리해라)
namespace HAD
{
    public class AttackCombo1Command : ICommand
    {
        private PlayerCharacter character;
        private float awaitTime;

        public AttackCombo1Command(PlayerCharacter ch)      
        {
            character = ch;
        }

        async Task ICommand.Execute()
        {
            character.PerformAttackCombo();

            awaitTime = character.CharacterAttackComboController.AttackCombo(1);
            var effect = EffectPoolManager.Singleton.GetEffect("Attack1");
            effect.gameObject.transform.position = character.transform.position;
            effect.gameObject.transform.rotation = character.transform.rotation;

            await Awaitable.WaitForSecondsAsync(awaitTime);
            if (character.CurrentAttackComboIndex <= 1) 
            {
                character.CharacterAttackComboController.Locomotion();
                character.ResetComboIndex();
            }
        }
    }
}