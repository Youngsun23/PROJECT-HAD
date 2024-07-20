using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.HID;

namespace HAD
{
    // 인풋 입력 -> 애니메이션, 이동
    public class CharacterBase : MonoBehaviour, IDamage, IActor
    {
        public bool IsDashing => isDashing;
        public bool IsAttacking => isAttacking;
        public bool IsMagicAiming => isMagicAiming;

        [Title("Components")]
        public UnityEngine.CharacterController characterController;
        public Animator characterAnimator;

        [Title("Character Movement")]
        public float moveSpeed;
        public float RotationSmoothTime = 0.12f;
        // public LayerMask groundLayer;

        [Title("Character Attack")]
        public int comboIndex = 0;
        public bool isAttacking = false;
        private float attakRadius = 2.5f;
        public GameObject magicArrowPrefab;
        public int curMagicArrow;
        private int maxMagicArrow = 1;
        private bool isMagicAiming;
        private bool isCombo2ing = false;
        private float combo2MoveSpeed = 5f;

        [Title("Character Dash")]
        public float dashCooltime = 1f;
        public float dashDuration = 1f;
        public float dashSpeed = 10f;
        private bool dashAvailable = true;
        private bool isDashing = false;
        private float dashTimer = 0f;

        private float rotationVelocity;
        private float verticalVelocity;
        private float targetRotation;
        private float targetMoveSpeed;
        private Vector2 targetMoveInput;
        private Vector2 targetMoveInputBlend;
        private Vector3 lastPosition;
        private bool isGrounded = false;

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

            if (isDashing)
            {
                if (Time.time > dashTimer) // 대쉬 지속시간이 지났다는 뜻이다.
                {
                    DashRestore();
                }
                else
                {
                    DashMove();
                }
            }

            if(isCombo2ing)
            {
                Combo2Move();
            }

            if(isMagicAiming)
            {
                MouseRotate();
                if(characterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.35f)
                characterAnimator.speed = 0f;
                // 날아갈 경로 보여주기
            }
        }

        public void Move(Vector2 input, float yAxis)
        {
            //if (isAttacking)
            //    return;

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

        public void DashMove()
        {
            characterController.Move(transform.forward * dashSpeed * Time.deltaTime);
        }

        private void Combo2Move()
        {
            characterController.Move(transform.forward * combo2MoveSpeed * Time.deltaTime);
        }

        // 마우스커서 위치 받아서 해당 방향으로 회전
        // 아무것도 없는 곳은 눌러도 충돌 x 회전 x
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
            if(!isDashing)
                MouseRotate(); // ToDo: 콤보 막타 중 회전 막기
            characterAnimator.SetTrigger("AttackTrigger");
            comboIndex++;
            characterAnimator.SetInteger("AttackComboCount", comboIndex);
        }

        public void ExecuteAttackLogic(string parameter)
        {
            // parameter => "BasicAttack"

            Collider[] overlapObjects = Physics.OverlapSphere(transform.position, attakRadius);
            if (overlapObjects == null || overlapObjects.Length == 0)
            {
                // To do : 공격대상이 아무것도 없다는 뜻
                // To do : 공격 모션이나 이펙트만 출력해주고 빠져나감

                return;
            }

            // 한 옵젝에 여러 콜라이더 있는 경우 - 중복 처리 피하기 위해
            List<Transform> checkedObjects = new List<Transform>();
            // To do : 실제 공격 로직을 구현한다.
            for (int i = 0; i < overlapObjects.Length; i++)
            {
                if (checkedObjects.Exists(x => x == overlapObjects[i].transform.root))
                {
                    continue;
                }
                checkedObjects.Add(overlapObjects[i].transform.root);

                Vector3 position = overlapObjects[i].transform.root.position;
                Vector3 direction = (position - transform.position).normalized;
                float dotAngle = Vector3.Dot(transform.forward, direction); // Dot 함수 - -1~1 / Angle 함수 - 0~360
                // 점A 위치(점A에서 forward로의 방향), 점A에서 점B로의 방향 둘 사이의 내적
                
                if (dotAngle > 0.5f)
                {
                    Debug.DrawLine(transform.position + Vector3.up, overlapObjects[i].transform.root.position + Vector3.up, Color.red, 1f);

                    var damageInterface = overlapObjects[i].transform.root.GetComponent<IDamage>();
                    if (damageInterface != null)
                    {
                        damageInterface.TakeDamage(this, 10f);
                        Debug.Log($"{overlapObjects[i].transform.root.name} 공격에 피격!");
                    }
                }
            }
        }

        // 범위: 0.2~0.3 전방 찌르기
        // overlapshere? raycast? boxcast?
        // ray 쏘는 길이 = attack의 radius
        public void ExecuteCombo1Logic()
        {
            RaycastHit[] rayCastHits = Physics.RaycastAll(transform.position + Vector3.up, transform.forward, attakRadius);
            if (rayCastHits == null || rayCastHits.Length == 0)
            {
                // To do : 공격 모션이나 이펙트만 출력해주고 빠져나감

                return;
            }

            // To do : 실제 공격 로직을 구현한다.
            for (int i = 0; i < rayCastHits.Length; i++)
            {
                var damageInterface = rayCastHits[i].collider.transform.root.GetComponent<IDamage>();
                if (damageInterface != null)
                {
                    damageInterface.TakeDamage(this, 10f);
                    Debug.DrawLine(transform.position + Vector3.up, rayCastHits[i].collider.transform.root.position + Vector3.up, Color.green, 1f);
                    Debug.Log($"{rayCastHits[i].collider.transform.root.name} 콤보1에 피격!");
                }
            }
        }

        // 범위: 0.2~0.3 전방 찌르며 전진(찌르는 순간부터 전진 마치는 순간까지 지속? 아니면 길이만 그만큼 길게?)
        // overlapshere? raycast? boxcast?
        // ray 쏘는 길이 = attack의 radius * 2
        public void ExecuteCombo2Logic()
        {
            RaycastHit[] rayCastHits = Physics.RaycastAll(transform.position + Vector3.up, transform.forward, attakRadius * 2);
            if (rayCastHits == null || rayCastHits.Length == 0)
            {
                // To do : 공격 모션이나 이펙트만 출력해주고 빠져나감

                return;
            }

            // To do : 실제 공격 로직을 구현한다.
            for (int i = 0; i < rayCastHits.Length; i++)
            {
                var damageInterface = rayCastHits[i].collider.transform.root.GetComponent<IDamage>();
                if (damageInterface != null)
                {
                    damageInterface.TakeDamage(this, 10f);
                    Debug.DrawLine(transform.position + Vector3.up, rayCastHits[i].collider.transform.root.position + Vector3.up, Color.black, 1f);
                    Debug.Log($"{rayCastHits[i].collider.transform.root.name} 콤보2에 피격!");
                }
            }
        }

        // 범위: 360도 넓은 원(파동 퍼지기 시작~소멸까지 지속?)
        public void ExecuteSpecialAttackLogic()
        {
            Collider[] overlapObjects = Physics.OverlapSphere(transform.position, attakRadius);
            if (overlapObjects == null || overlapObjects.Length == 0)
            {
                // To do : 공격 모션이나 이펙트만 출력해주고 빠져나감

                return;
            }

            List<Transform> checkedObjects = new List<Transform>();
            for (int i = 0; i < overlapObjects.Length; i++)
            {
                if (checkedObjects.Exists(x => x == overlapObjects[i].transform.root))
                {
                    continue;
                }
                checkedObjects.Add(overlapObjects[i].transform.root);

                var damageInterface = overlapObjects[i].transform.root.GetComponent<IDamage>();
                if (damageInterface != null)
                {
                    damageInterface.TakeDamage(this, 10f);
                    Debug.Log($"{overlapObjects[i].transform.root.name} 특수공격에 피격!");
                }
            }
        }

        public void ExecuteCombo2Move()
        {
            isCombo2ing = !isCombo2ing;
        }

        public void MagicAim()
        {
            if (curMagicArrow <= 0)
                return;

            isMagicAiming = true;
            // MouseRotate();
            characterAnimator.SetTrigger("MagicTrigger");

            // 현재, 키다운 -> 애니메이션, 생성 한 번에
            // 수정, 누르는 동안 애니메이션 멈추고 날아가는 경로 보여주면서 마우스 회전
        }

        public void MagicShot()
        {
            // 떼는 순간(혹은 시간 초과) 던지는 애니메이션 나가면서 Instantiate 해야 함
            characterAnimator.speed = 1f;
            isMagicAiming = false;
            Instantiate(magicArrowPrefab, transform.position + transform.forward + transform.up, transform.rotation * Quaternion.Euler(90, 0, 0));
            // curMagicArrow--;
        }
        
        public void SpecialAttack()
        {
            MouseRotate();
            characterAnimator.SetTrigger("SpecialAttackTrigger");

            // 원형 충격파
        }

        public void Dash()
        {
            if (dashAvailable)
            {
                // To do : Dash 이동 구현
                dashAvailable = false;
                isDashing = true;
                dashTimer = Time.time + dashDuration;

                Vector3 currentPosition = transform.position;
                Vector3 currentForward = transform.forward;
                Vector3 predictPosition = currentPosition + (currentForward * dashSpeed * dashDuration);

                Ray ray = new Ray(predictPosition + Vector3.up, Vector3.down);
                // Raycast로 default, groundlayer 둘 다 감지 -> groundlayer 있으므로 T 반환 => 원하는 처리와 다른 결과
                // 위에서 아래로 쏘는 ray에 가장 먼저 충돌한 옵젝의 layer를 기준으로 해야 한다.
                RaycastHit[] rayHits = Physics.RaycastAll(ray, 100f);
                if(rayHits.Length > 0)
                {
                    System.Array.Sort(rayHits, (a, b) => a.distance.CompareTo(b.distance));
                    // Debug.Log($"여러 레이어 발견, 정렬했습니다. 최상위 레이어: {rayHits[0].collider.gameObject.layer}");
                }
                // 도착 지점이 * layer일 경우 레이어 변경 - *은 floor 하나면 됨
                if (rayHits[0].collider.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_GROUND))
                {
                    // To do : predictPosition 에서 아래로 발사한 레이캐스팅이 성공했다
                    // => 최종 예측지점이 GroundLayer에 속해있다 => 땅이다.
                    // => predictPosition 으로 이동하는 대쉬를 실행한다 => 캐릭터의 레이어를 안보이는 벽에 충돌이 안되도록 바꿔준다.
                    gameObject.layer = LayerMask.NameToLayer(Constant.LAYER_NAME_CHARACTER_IN_DASH);
                }

                characterAnimator.SetBool("IsDashing", true);
                characterAnimator.SetTrigger("DashTrigger");
            }
        }

        private void DashRestore()
        {
            isDashing = false;
            dashAvailable = true;
            characterAnimator.SetBool("IsDashing", false);
            gameObject.layer = LayerMask.NameToLayer(Constant.LAYER_NAME_DEFAULT);
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
            if (!isGrounded && !isDashing)
                verticalVelocity = -9.8f;
            else
                verticalVelocity = 0;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + (Vector3.up * characterController.radius), characterController.radius);
        }

        public void TakeDamage(IActor actor, float damage)
        {
            // 피격 이펙트 출력
            // 체력 감소 - 사망 체크

            // Debug.Log($"Take Damage - Attacker : {actor.GetActor().name}, Damage : {damage}");
        }

        public GameObject GetActor()
        {
            return gameObject;
        }
    }
}