using System.Threading.Tasks;
using UnityEngine;

namespace HAD
{
    public class AttackCombo2Command : ICommand
    {
        private PlayerCharacter character;
        private float awaitTime;

        public AttackCombo2Command(PlayerCharacter ch) 
        {
            character = ch;
        }

        async Task ICommand.Execute()
        {
            character.PerformAttackCombo();
            awaitTime = character.CharacterAttackComboController.AttackCombo(2);

            var effect = EffectPoolManager.Singleton.GetEffect("Attack2");
            effect.gameObject.transform.position = character.transform.position;
            effect.gameObject.transform.rotation = character.transform.rotation * Quaternion.Euler(0, -90, 0);

            await Awaitable.WaitForSecondsAsync(awaitTime);
            if (character.CurrentAttackComboIndex <= 2)
            {
                character.CharacterAttackComboController.Locomotion();
                character.ResetComboIndex();
            }
        }
    }
}
