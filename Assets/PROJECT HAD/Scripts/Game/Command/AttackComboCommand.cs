using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @ Command : 레시피 1 (이 재료로 이렇게 조리해라)
namespace HAD
{
    public class AttackComboCommand : ICommand
    {
        private CharacterBase character;

        public AttackComboCommand(CharacterBase ch)
        {
            character = ch;
        }

        public void Execute()
        {
            character.PerformAttackCombo();
        }
    }
}
