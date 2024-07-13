using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace HAD
{
    // 인풋 입력 -> 애니메이션, 이동
    public class CharacterBase : MonoBehaviour
    {
        public UnityEngine.CharacterController characterController;
        public Animator characterAnimator;
        public float moveSpeed;
        public float RotationSmoothTime = 0.12f;

        private float rotationVelocity;
        private float verticalVelocity;

        private float targetRotation;
        private float targetMoveSpeed;
        private Vector2 targetMoveInput;
        private Vector2 targetMoveInputBlend;

        private Vector3 lastPosition;

        public int comboIndex = 0;

        public bool isAttacking = false;
        private bool isGrounded = false;

        private int maxMagicArrow = 1;
        public int curMagicArrow;
        public float dashCooltime = 1f;
        private bool dashAvailable = true;

        private void Start()
        {
            characterController = GetComponent<UnityEngine.CharacterController>(); // 콜라이더 기능 포함

            curMagicArrow = maxMagicArrow;
        }

        private void Update()
        {
            targetMoveInputBlend = Vector2.Lerp(targetMoveInputBlend, targetMoveInput, Time.deltaTime * 10);

            characterAnimator.SetFloat("Horizontal", targetMoveInputBlend.x);
            characterAnimator.SetFloat("Vertical", targetMoveInputBlend.y);

            IsGround();

            FreeFall();
        }

        public void Move(Vector2 input, float yAxis)
        {
            if (isAttacking)
                return;

            targetMoveSpeed = input != Vector2.zero ? moveSpeed : 0.0f;

            Vector3 inputDirection = new Vector3(input.x, 0.0f, input.y).normalized;
            if (input != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + yAxis;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 deltaPosition = transform.InverseTransformDirection(transform.position - lastPosition).normalized;
            targetMoveInput = new Vector2(deltaPosition.x, deltaPosition.z);
            lastPosition = transform.position;

            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
            characterController.Move(
                targetDirection.normalized * (targetMoveSpeed * Time.deltaTime)     // 인풋이 있다면 움직이는곳 XZ 축으로 이동
                + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);      // 인풋이 있던 없던 중력 Y 축을 이동
        }

        // 마우스커서 위치 받아서 해당 방향으로 회전
        // 되다말다? 안 되는 때가 있는데 왜지?
        private void MouseRotate()
        {
            RaycastHit mouseHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out mouseHit, 100f))
            {
                Vector3 destPos = new Vector3(mouseHit.point.x, transform.position.y, mouseHit.point.z);
                Vector3 dir = destPos - transform.position;
                Quaternion lookTarget = Quaternion.LookRotation(dir);
                transform.rotation = lookTarget;
            }
        }

        public void Attack()
        {
            MouseRotate(); // ToDo: 콤보 막타 중 회전 막기
            characterAnimator.SetTrigger("AttackTrigger");
            comboIndex++;
            characterAnimator.SetInteger("AttackComboCount", comboIndex);
        }

        public void Magic()
        {
            if (curMagicArrow <= 0)
                return;

            MouseRotate();
            characterAnimator.SetTrigger("MagicTrigger");

            // Instantiate magicArrow 
        }

        public void SpecialAttack()
        {
            MouseRotate();
            characterAnimator.SetTrigger("SpecialAttackTrigger");

            // 원형 충격파
        }

        public void Dash()
        {
            if(dashAvailable)
            {
                dashAvailable = false;
                characterAnimator.SetTrigger("DashTrigger");

                // ToDo 대시 이동거리 끝났는데 옵젝콜라이더 중간 지점에 멈추는 상황 막기

                Invoke("DashRestore", 1f);
            }
        }

        private void DashRestore()
        {
            dashAvailable = true;
        }

        private void IsGround()
        {
            RaycastHit groundHit;
            isGrounded = Physics.SphereCast(transform.position + (Vector3.up * characterController.radius), characterController.radius, Vector3.down, out groundHit, 0.1f, 1 << 3);
            // 적당히 0.1f(t/f 오가면서 지면에서 과하게 떨어지지 않음) 했는데...
        }
        // 일정거리 이하로 캐릭터의 y가 내려가지 않음. (일부 애니메이션의 발 위치가 Idle보다 낮아진다는 걸 생각하면 괜찮은데,
        // 문제는 이유를 모르겠음) (공중에서는 무한 낙하하고, collider 위치도 문제 없어보이는데 왜? height 조절해도 비슷...)
        // ToDo: 핑크영역(맵 외곽 배경부분)은 collider 있는 것처럼 이동 불가하도록

        private void FreeFall()
        {
            if (!isGrounded)
                verticalVelocity = -9.8f;
            else
                verticalVelocity = 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + (Vector3.up * characterController.radius), characterController.radius);
        }
    }
}