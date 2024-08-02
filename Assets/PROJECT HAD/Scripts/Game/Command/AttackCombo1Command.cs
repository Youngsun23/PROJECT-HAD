using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static System.Activator;

// @ Command : 레시피 1 (이 재료로 이렇게 조리해라)
namespace HAD
{
    public class AttackCombo1Command : ICommand
    {
        private CharacterBase character;

        public AttackCombo1Command(CharacterBase ch)
        {
            character = ch;
        }

        //public void Execute()
        //{
        //    character.PerformAttackCombo();
        //}

        // Class Try 2 _ Command Pattern
        async Task ICommand.Execute()
        {
            character.PerformAttackCombo();
            await Awaitable.WaitForSecondsAsync(character.CharacterAttackComboController.AttackCombo(1));
            // 
            character.CharacterAttackComboController.Locomotion();
            character.ResetComboIndex();
        }
    }
}
