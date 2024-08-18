using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class SpiderCharacter : MonsterBase
    {
        private bool isAttackDashing = false;
        private Vector3 targetDirection;
        // AttackPossibleRange를 25초 동안 이동하도록 속도 계산
        float attackDashSpeed;
        public LayerMask sensorLayerMask;

        protected override void Start()
        {
            base.Start();

            attackDashSpeed = AttackPossibleRange / 20f;
        }

        protected override void Update()
        {
            base.Update();

            if(isAttackDashing)
            {
                AttackDashMove();
            }
        }

        public override void Attack()
        {
            base.Attack();

            Vector3 targetDestination = targetPlayer.transform.position;
            targetDirection = targetDestination - transform.position; // transform.position에서 targetDestination 방향 (평생 헷갈릴듯)

            isAttackDashing = true;
        }

        public void AttackDashMove()
        {
            // 공격 시작 시점의 캐릭터 위치로 이동
            characterController.Move(targetDirection * attackDashSpeed * Time.deltaTime);
        }

        // 전방 부채꼴 범위 공격 <- 애니메이션 이벤트로 걸기
        public void AttackExecute()
        {
            isAttackDashing = false;

            Collider[] overlapObjects = Physics.OverlapSphere(transform.position, AttackRange, ~sensorLayerMask);
            if (overlapObjects == null || overlapObjects.Length == 0)
            {
                // To do : 공격 이펙트

                return;
            }

            List<Transform> checkedObjects = new List<Transform>();
            for (int i = 0; i < overlapObjects.Length; i++)
            { 
                Vector3 position = overlapObjects[i].transform.root.position;
                Vector3 direction = (position - transform.position).normalized;
                float dotAngle = Vector3.Dot(transform.forward, direction);

                if (dotAngle > 0.5f)
                {
                    Debug.DrawLine(transform.position + Vector3.up, overlapObjects[i].transform.root.position + Vector3.up, Color.red, 1f);

                    if(overlapObjects[i].transform.root.gameObject.CompareTag("Player"))
                    { 
                        var damageInterface = overlapObjects[i].transform.root.GetComponent<IDamage>();
                        ExecuteDamage(damageInterface, AttackDamage);
                        return;
                    }
                }
            }
        }

    }
}
