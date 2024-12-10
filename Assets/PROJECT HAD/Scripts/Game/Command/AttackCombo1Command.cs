using System.Threading.Tasks;

// @ Command : 레시피 1 (이 재료로 이렇게 조리해라)
namespace HAD
{
    public class AttackCombo1Command : ICommand
    {
        private PlayerCharacter character;
        private float awaitTime;

        //// : AttackComboCommands
        //public AttackCombo1Command(CharacterBase ch)
        //    : base(ch) { }
        public AttackCombo1Command(PlayerCharacter ch)      
        {
            character = ch;
        }

        //public void Execute()
        //{
        //    character.PerformAttackCombo();
        //}

        // public override async Task Execute()
        async Task ICommand.Execute()
        {
            character.PerformAttackCombo();

            awaitTime = character.CharacterAttackComboController.AttackCombo(1);
            var effect = EffectPoolManager.Singleton.GetEffect("Attack1");
            effect.gameObject.transform.position = character.transform.position;
            effect.gameObject.transform.rotation = character.transform.rotation;

            await Awaitable.WaitForSecondsAsync(awaitTime);
            // 다음 콤보로 연결 안 되면 아래 코드 실행 (await는 일시중단일뿐임)
            if (character.CurrentAttackComboIndex <= 1) // 여기 조건을 대체 뭘로 해야하나
            {
                //Debug.Log("콤보1에서 Loco로");
                character.CharacterAttackComboController.Locomotion();
                character.ResetComboIndex();
            }
        }
    }
}

// 문제 1
// 공격1 실행 중 공격2 키입력 -> 공격1 종료까지 기다렸다가 공격2 실행으로 넘어가야 함
// 현재 흐름 : 공격1 중 공격2 입력 -> await 하는 동안 공격2는 바로 실행됨
// 문제 1 해결 필요...
// 커맨드를 넣자마자 실행할 게 아니라 큐에 쌓아뒀다가 그 이전 애니메이션 빠져나갈 쯤에 확인해서 큐에 뭔가 있으면 재생하고 아니면 리셋(로코모션)해야 함

// 문제 2
// 공격3의 duration만큼 기다리기가 원하는 대로 작동하지 않음
// 공격3의 애니메이션 이벤트로 조절되는 불값
// 애니메이션 0.8 쯤의 이벤트가 호출되기 전에 Locomotion으로 넘어가서 불값이 계속 on에 머문다.
// ! 공격1,2의 await 다음 코드를 없애니 문제 해결됨 = 즉, 공격12의 await가 끝나면서 Locomotion으로 넘겨버리는 것
// 해결
// 비슷한 이름의 다른 변수 사용으로 if문 무단통과 (내 1시간 돌려내)

