using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HAD
{
    public class AttackCombo2Command : ICommand
    {
        private CharacterBase character;

        //public AttackCombo2Command(CharacterBase ch)
        //    : base(ch) { }

        public AttackCombo2Command(CharacterBase ch) 
        {
            character = ch;
        }

        async Task ICommand.Execute()
        {
            // 지연시키기 -> 여기서 콤보1커맨드 종료를 기다리게 하면 안 되나?
            character.PerformAttackCombo();
            await Awaitable.WaitForSecondsAsync(character.CharacterAttackComboController.AttackCombo(2));
            //Debug.Log($"콤보2 실행 -> {character.CurrentAttackComboIndex}");
            if (character.CurrentAttackComboIndex <= 2)
            {
                //Debug.Log("콤보2에서 Loco로");
                character.CharacterAttackComboController.Locomotion();
                character.ResetComboIndex();
            }
        }
    }
}
