using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class SpiderCharacter : MonsterBase
    {
        private bool isAttackDashing = false;
        private Vector3 targetDirection;
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
            targetDirection = targetDestination - transform.position;

            isAttackDashing = true;
        }

        public void AttackDashMove()
        {
            characterController.Move(targetDirection * attackDashSpeed * Time.deltaTime);
        }

        public void AttackExecute()
        {
            isAttackDashing = false;

            Collider[] overlapObjects = Physics.OverlapSphere(transform.position, AttackRange, ~sensorLayerMask);
            if (overlapObjects == null || overlapObjects.Length == 0)
            {
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 forward = transform.forward;
            Vector3 leftBoundary = Quaternion.Euler(0, -60 * 0.5f, 0) * forward; // 60도가 대략 0.5
            Vector3 rightBoundary = Quaternion.Euler(0, 60 * 0.5f, 0) * forward;

            Gizmos.DrawLine(transform.position, transform.position + leftBoundary * AttackRange);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary * AttackRange);

            int segments = 20;
            Vector3 previousPoint = transform.position + leftBoundary * AttackRange;
            for (int i = 1; i <= segments; i++)
            {
                float angle = -60 * 0.5f + (60 / segments) * i;
                Vector3 point = Quaternion.Euler(0, angle, 0) * forward * AttackRange + transform.position;
                Gizmos.DrawLine(previousPoint, point);
                previousPoint = point;
            }
        }
    }
}
