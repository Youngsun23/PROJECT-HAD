using System.Threading.Tasks;
using UnityEngine;

namespace HAD
{
    public class AttackCombo2Command : ICommand
    {
        private PlayerCharacter character;
        private float awaitTime;

        //public AttackCombo2Command(CharacterBase ch)
        //    : base(ch) { }

        public AttackCombo2Command(PlayerCharacter ch) 
        {
            character = ch;
        }

        async Task ICommand.Execute()
        {
            // 지연시키기 -> 여기서 콤보1커맨드 종료를 기다리게 하면 안 되나?
            character.PerformAttackCombo();

            // 기존 코드
            // await Awaitable.WaitForSecondsAsync(character.CharacterAttackComboController.AttackCombo(2));

            awaitTime = character.CharacterAttackComboController.AttackCombo(2);
            var effect = EffectPoolManager.Singleton.GetEffect("Attack2");
            effect.gameObject.transform.position = character.transform.position;
            effect.gameObject.transform.rotation = character.transform.rotation * Quaternion.Euler(0, -90, 0);

            await Awaitable.WaitForSecondsAsync(awaitTime);
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
