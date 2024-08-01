using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class PushAbility : AbilityBase
    {
        public override AbilityTag Tag => AbilityTag.Push;

        private GameObject targetMon;

        public override void Execute()
        {
            // 적을 Player->적 방향으로 일정 거리 이동시킴
            Vector3 pushDir = (Owner.transform.position - targetMon.transform.position).normalized;
            targetMon.transform.Translate(pushDir);
            // ToDo: 현재는 1프레임 텔레포트 -> MonController 생기면 그 쪽의 강제이동 함수 발동하게 해서 Update에서 돌려야함
        }

        public void SetTargetMon(GameObject target)
        {
            targetMon = target;
        }
    }
}
