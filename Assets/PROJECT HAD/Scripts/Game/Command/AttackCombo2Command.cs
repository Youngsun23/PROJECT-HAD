using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HAD
{
    public class AttackCombo2Command : ICommand
    {
        private CharacterBase character;

        public AttackCombo2Command(CharacterBase ch)
        {
            character = ch;
        }

        async Task ICommand.Execute()
        {
            character.PerformAttackCombo();
            await Awaitable.WaitForSecondsAsync(character.CharacterAttackComboController.AttackCombo(3));
            //
            character.CharacterAttackComboController.Locomotion();
            character.ResetComboIndex();

        }
    }
}
