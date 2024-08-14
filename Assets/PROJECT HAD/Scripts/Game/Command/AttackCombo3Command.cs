using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HAD
{
    public class AttackCombo3Command : ICommand
    {
        private CharacterBase character;

        //public AttackCombo2Command(CharacterBase ch)
        //    : base(ch) { }

        public AttackCombo3Command(CharacterBase ch) 
        {
            character = ch;
        }

        async Task ICommand.Execute()
        {
            //Debug.Log("콤보3진입");
            character.PerformAttackCombo();
            // 위까지는 실행되는데 아래로 넘어가지를 않는다?? (그래서 isAttacking T 상태로 캐릭터 굳음)
            // 콤보1->2->3의 순차입력에서는 문제 없음
            // 콤보1에서 왼클릭 연타 시 2->3 진행에 문제가 생김. 와이?
            // 콤보1 하는 중 콤보2 커맨드가 수십개가 쌓이는 게 문제
            // 그렇다면?
            //Debug.Log($"콤보3실행");
            await Awaitable.WaitForSecondsAsync(character.CharacterAttackComboController.AttackCombo(3));
            //Debug.Log($"콤보3종료: {DateTime.Now}");
            //Debug.Log("콤보3에서 Loco로");
            character.CharacterAttackComboController.Locomotion();
            character.ResetComboIndex();
        }
    }
}
