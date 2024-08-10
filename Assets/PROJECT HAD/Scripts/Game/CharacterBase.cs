using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    // 인풋 입력 -> 애니메이션, 이동
    public class CharacterBase : MonoBehaviour, IDamage, IActor
    {
        public bool IsDashing => isDashing;
        public bool IsAttacking => isAttacking;
        public bool IsMagicAiming => isMagicAiming;

        public LayerMask ignoreLayerMask;

        // Class Try 2 _ Command Pattern
        public CharacterAttackComboController CharacterAttackComboController => characterAttackComboController;
        // Attribute
        public CharacterAttributeComponent CharacterAttributeComponent => characterAttributeComponent;

        [Title("Components")]
        public UnityEngine.CharacterController characterController;
        public Animator characterAnimator;

        [Title("Character Movement")]
        // public LayerMask groundLayer;
        private float rotationSmoothTime = 0.12f;

        [Title("Character Attack")]
        // public int comboIndex = 0;
        public GameObject magicArrowPrefab;
        // public int curMagicArrow;
        private bool isMagicAiming;
        private bool isCombo2ing = false;

        [Title("Character Dash")]
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

        public float curHP;

        public GameObject reflectionAbilFX;

        private CharacterAbilityComponent characterAbilityComponent;
        // Attribute
        private CharacterAttributeComponent characterAttributeComponent;

        #region Class Try 1 _ Command Pattern
        //// Class Try 1 _ Command Pattern
        //private CharacterActionController characterActionController;
        //private CharacterActionData currentAction;
        #endregion

        // Class Try 2 _ Command Pattern
        private CharacterAttackComboController characterAttackComboController;
        // 지연시키기
        private CharacterCommandManager characterCommandManager;

        // DataBase로 빼놓은 애들
        // Attribute 구조로 바꾸며 필요 없어짐 -> 주석처리
        //[SerializeField] private int level;
        //[SerializeField] private float maxHP;
        //[SerializeField] private int maxMagicArrow;
        //[SerializeField] private float moveSpeed;
        //[SerializeField] private float dashCoolTime;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float combo2MoveSpeed;
        [SerializeField] private float attackRadius;
        [SerializeField] private float dashDuration;

        // 이 방식은 폐기
        //[Title("Character Setting")]
        //public CharacterGameData characterData;

        #region Command Combo _ PerformAttackCombo()
        //// 커맨드 패턴 시도
        //private bool isAttack1ing;
        //private bool isAttack2ing;
        //// 트리거 조건 O / 조건 X 트랜지션 중 전자가 우선 확인되어야 하는 거 아닌가??
        //private bool combo1On;
        //private bool combo2On;

        //// @ Receiver's the work : 주방의 요리사가 부여받은 주문서의 레시피대로 요리함
        //public void PerformAttackCombo()
        //{
        //    // attackName 받아오는 대신 여기서 불값 확인해서
        //    // 공격0->1, 1->2, 2->3으로 각기 다른 트리거 set해주는 방식으로 구현해볼것
        //    if(isAttack2ing)
        //    {
        //        characterAnimator.SetTrigger("Attack3Trigger");
        //        combo2On = true;
        //        characterAnimator.SetBool("Combo2", combo2On);
        //        // Debug.Log("트리거3 On!");
        //    }
        //    else if(isAttack1ing)
        //    {
        //        characterAnimator.SetTrigger("Attack2Trigger");
        //        combo1On = true;
        //        characterAnimator.SetBool("Combo1", combo1On);
        //        // Debug.Log("트리거2 On!");
        //    }
        //    else
        //    {
        //        combo1On = false;
        //        combo2On = false;
        //        characterAnimator.SetBool("Combo1", combo1On);
        //        characterAnimator.SetBool("Combo2", combo2On);
        //        characterAnimator.SetTrigger("Attack1Trigger");
        //        // Debug.Log("트리거1 On!");
        //    }
        //    // _isAttacking = true;
        //}

        //public void ReverseIsAttack1ing()
        //{
        //    isAttack1ing = !isAttack1ing;
        //    // Debug.Log($"Attack1ing: {isAttack1ing}");
        //}
        //public void ReverseIsAttack2ing()
        //{
        //    isAttack2ing = !isAttack2ing;
        //    // Debug.Log($"Attack2ing: {isAttack2ing}");
        //}
        //// -- 커맨드 패턴
        #endregion

        #region Class Try 1 _ Command Pattern
        //// Class Try 1 _ Command Pattern
        //private bool isNeedToExecuteNextAction = false;
        //public void PerformAttackCombo()
        //{
        //    if(currentAction == null)
        //    {
        //        isNeedToExecuteNextAction = false;
        //        var firstActionData = characterActionController.GetActionData(null);
        //        characterAnimator.Play(firstActionData.ActionStateName);
        //        currentAction = firstActionData;
        //    }
        //    else
        //    {
        //        if(currentAction.NextAction != null)
        //        {
        //            isNeedToExecuteNextAction = true;
        //        }
        //    }
        //}
        #endregion

        // Class Try 2 _ Command Pattern
        // 일단 하드코딩 -> 점점 바꾸기
        public int CurrentAttackComboIndex => attackComboIndex;
        private int attackComboIndex = 0;
        public bool isAttacking = false;
        private bool isNeedExecuteNextCombo = false;
        public bool IsNeedExecuteNextCombo => isNeedExecuteNextCombo;
        //private bool isNextComboReady = false;
        //public void SetNextComboReady(bool tr) { isNextComboReady = tr; }
        //public bool IsNextComboReady => isNextComboReady;
        public void PerformAttackCombo()
        {
            // 기존 Attack() // ToDo: 콤보 막타 중 회전 막기
            if (!isDashing)
                MouseRotate();

            if (!isAttacking)
            {
                isAttacking = true;
                attackComboIndex++;
            }
            else
            {
                isNeedExecuteNextCombo = true;
                attackComboIndex++;
            }
            //Debug.Log("증가"+attackComboIndex);
        }
        public void ResetComboIndex()
        {
            //Debug.Log($"ResetCombo 호출됨");
            isAttacking = false;
            isNeedExecuteNextCombo = false;
            attackComboIndex = 0;
            //Debug.Log("리셋"+attackComboIndex);
        }
        //

        //// Attribute
        //// CharacterAttributeComponent에서 모든 Type 도는 대신에
        //public List<AttributeTypes> attributes = new List<AttributeTypes>();

        public void InitializeCharacter(CharacterGameData characterData)
        {
            //// Attribute
            //// 이런 방법도 가능
            //for(int i = 0; i < attributes.Count; i++)
            //{
            //    characterAttributeComponent.attributes.Add((AttributeTypes)i, new CharacterAttribute());
            //}

            // level = characterData.Level;

            // Attribute
            // 이 코드를
            // maxHP = characterData.MaxHP;
            // 이렇게 변경
            // 스탯 변경은 모두 characterAttributeComponent로 접근해서 값 변경 
            // public float CurrentHP => characterAttribute.GetAttribute(Attributetypes.HealthPoint).CurrentValue; // 이렇게 사용도 가능

            characterAttributeComponent.SetAttribute(AttributeTypes.HealthPoint, characterData.MaxHP);
            characterAttributeComponent.SetAttribute(AttributeTypes.MagicArrowCount, characterData.MaxArrow);
            characterAttributeComponent.SetAttribute(AttributeTypes.DashCoolTime, characterData.DashCooltime);
            characterAttributeComponent.SetAttribute(AttributeTypes.MoveSpeed, characterData.MoveSpeed);
            characterAttributeComponent.SetAttribute(AttributeTypes.AttackPower, characterData.AttackPower);
            characterAttributeComponent.SetAttribute(AttributeTypes.MagicPower, characterData.MagicPower);
            characterAttributeComponent.SetAttribute(AttributeTypes.SpecialAttackPower, characterData.SpecialAttackPower);

            // 아래는 벞/디벞/강화 안 하는 요소
            // 그대로 써도 되나?
            combo2MoveSpeed = characterData.Combo2MoveSpeed;
            attackRadius = characterData.AttakRadius;
            dashSpeed = characterData.DashSpeed;
            dashDuration = characterData.DashDuration;
        }

        //// Attributes
        //// 이렇게 사용도 가능
        //public float CurrentHP
        //{
        //    get => characterAttributeComponent.GetAttribute(AttributeTypes.HealthPoint).CurrentValue;
        //    set => characterAttributeComponent.SetAttributeCurrentValue(AttributeTypes.HealthPoint, value);
        //}

        private void Awake()
        {
            characterAbilityComponent = GetComponent<CharacterAbilityComponent>();

            #region Class Try 1 _ Command Pattern
            //// Class Try 1 _ Command Pattern
            //characterActionController = GetComponent<CharacterActionController>();  
            #endregion

            // Class Try 2 _ Command Pattern
            characterAttackComboController = GetComponent<CharacterAttackComboController>();
            // 지연시키기
            characterCommandManager = GetComponent<CharacterCommandManager>();

            // Attribute
            characterAttributeComponent = GetComponent<CharacterAttributeComponent>();    
        }
    
        private void Start()
        {
            characterController = GetComponent<UnityEngine.CharacterController>();
        }

        private void Update()
        {
            targetMoveInputBlend = Vector2.Lerp(targetMoveInputBlend, targetMoveInput, Time.deltaTime * 10);

            characterAnimator.SetFloat("Horizontal", targetMoveInputBlend.x);
            characterAnimator.SetFloat("Vertical", targetMoveInputBlend.y);

            IsGround();
            FreeFall();

            if (!dashAvailable)
            {
                // 대쉬 쿨타임 체크
                if (Time.time > dashTimer + characterAttributeComponent.GetAttribute(AttributeTypes.DashCoolTime).CurrentValue)
                {
                    dashAvailable = true;
                }
                else if (Time.time > dashTimer + dashDuration) // 대쉬 지속시간이 지났다는 뜻이다.
                {
                    DashEnd();
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
                //if(characterAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.35f)
                //characterAnimator.speed = 0f;
                // 날아갈 경로 보여주기
            }

            #region Class Try 1 _ Command Pattern
            //// Class Try 1 _ Command Pattern
            //if(currentAction != null)
            //{
            //    var stateInfo = characterAnimator.GetCurrentAnimatorStateInfo(0);
            //    if(currentAction.NextAction != null
            //        && stateInfo.normalizedTime <= currentAction.LimitInputNormalizedTime
            //        && stateInfo.normalizedTime >= currentAction.MinimumPlayClipLength)
            //    {
            //        if(isNeedToExecuteNextAction)
            //        {
            //            characterAnimator.CrossFade(currentAction.NextAction.ActionStateName, 0.25f);
            //            currentAction = currentAction.NextAction;

            //            PerformAttackCombo();
            //        }
            //    }
            //    // 다음 액션으로 연결 실패 -> 초기화
            //    if(stateInfo.normalizedTime >= currentAction.MinimumExitNormalizedTime)
            //    {
            //        currentAction = null;
            //        isNeedToExecuteNextAction = false;
            //        characterAnimator.CrossFade("Locomotion", 1.0f - currentAction.MinimumExitNormalizedTime);
            //    }
            //}
            #endregion
        }

        public void Move(Vector2 input, float yAxis)
        {
            //if (isAttacking)
            //    return;

            targetMoveSpeed = input != Vector2.zero ? characterAttributeComponent.GetAttribute(AttributeTypes.MoveSpeed).CurrentValue : 0.0f;

            Vector3 inputDirection = new Vector3(input.x, 0.0f, input.y).normalized;
            if (input != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + yAxis;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);
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
            if (Physics.Raycast(ray, out mouseHit, 100f, ~ignoreLayerMask))
            {
                Vector3 destPos = new Vector3(mouseHit.point.x, transform.position.y, mouseHit.point.z);
                Vector3 dir = destPos - transform.position;
                Quaternion lookTarget = Quaternion.LookRotation(dir);
                transform.rotation = lookTarget;
            }
        }
        // ??? 얘는 왜 갑자기 고장남??
        // BoundingVolume 옵젝들에 먼저 걸려서 그런가보다...레이어로 빼서 무시

        private void ExecuteDamage(IDamage damageInterface, float damage)
        {
            damageInterface?.TakeDamage(this, damage);
        }

        public void ExecuteAttackLogic(string parameter)
        {
            // parameter => "BasicAttack"

            Collider[] overlapObjects = Physics.OverlapSphere(transform.position, attackRadius);
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
                    ExecuteDamage(damageInterface, 10f);
                    // Debug.Log($"{overlapObjects[i].transform.root.name} 공격에 피격!");

                    // @ Ability System
                    // 타격 유효 대상이 적의 투사체인 경우
                    // 반사 스킬 유무 확인
                    // 있으면, 반사 스킬로 넘기기
                    // 없으면, 투사체 파괴만
                    if (overlapObjects[i].transform.root.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_MONPROJECTILE))
                    {
                        if (characterAbilityComponent.GetAbility(AbilityTag.Reflection, out ReflectionAbility abil))
                        {
                            // 여기?
                            // reflectionAbilFX.SetActive(true);
                            abil.SetTargetProjectile(overlapObjects[i].transform.root.gameObject);
                            abil.Execute();
                        }
                        else
                        {
                            Destroy(overlapObjects[i].transform.root.gameObject);
                        }
                    }
                }
            }
        }

        // 범위: 0.2~0.3 너비, 길이 전방 찌르기
        public void ExecuteCombo1Logic()
        {
            // RaycastHit[] rayCastHits = Physics.RaycastAll(transform.position + Vector3.up, transform.forward, attakRadius);
            RaycastHit[] sphereCastHits = Physics.SphereCastAll(transform.position + Vector3.up, attackRadius, transform.forward, attackRadius);
            
            if (sphereCastHits == null || sphereCastHits.Length == 0)
            {
                // To do : 공격 모션이나 이펙트만 출력해주고 빠져나감

                return;
            }

            // To do : 실제 공격 로직을 구현한다.
            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                var damageInterface = sphereCastHits[i].collider.transform.root.GetComponent<IDamage>();
                ExecuteDamage(damageInterface, 10f);
                Debug.DrawLine(transform.position + Vector3.up, sphereCastHits[i].collider.transform.root.position + Vector3.up, Color.green, 1f);
                // Debug.Log($"{sphereCastHits[i].collider.transform.root.name} 콤보1에 피격!");

                // @ Ability System
                if (sphereCastHits[i].transform.root.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_MONPROJECTILE))
                {
                    if (characterAbilityComponent.GetAbility(AbilityTag.Reflection, out ReflectionAbility abil))
                    {
                        abil.SetTargetProjectile(sphereCastHits[i].transform.root.gameObject);
                        abil.Execute();
                    }
                    else
                    {
                        Destroy(sphereCastHits[i].transform.root.gameObject);
                    }
                }
            }
        }

        // 범위: 0.2~0.3 너비, 두 배 길이 전방 찌르며 전진 (찌르는 순간부터 전진 마치는 순간까지 지속? 아니면 길이만 그만큼 길게?)
        public void ExecuteCombo2Logic()
        {
            RaycastHit[] sphereCastHits = Physics.SphereCastAll(transform.position + Vector3.up, attackRadius, transform.forward, attackRadius * 2);
            if (sphereCastHits == null || sphereCastHits.Length == 0)
            {
                // To do : 공격 모션이나 이펙트만 출력해주고 빠져나감

                return;
            }

            // To do : 실제 공격 로직을 구현한다.
            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                var damageInterface = sphereCastHits[i].collider.transform.root.GetComponent<IDamage>();
                ExecuteDamage(damageInterface, 10f);
                Debug.DrawLine(transform.position + Vector3.up, sphereCastHits[i].collider.transform.root.position + Vector3.up, Color.black, 1f);
                // Debug.Log($"{sphereCastHits[i].collider.transform.root.name} 콤보2에 피격!");

                // @ Ability System
                if (sphereCastHits[i].transform.root.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_MONPROJECTILE))
                {
                    if (characterAbilityComponent.GetAbility(AbilityTag.Reflection, out ReflectionAbility abil))
                    {
                        abil.SetTargetProjectile(sphereCastHits[i].transform.root.gameObject);
                        abil.Execute();
                    }
                    else
                    {
                        Destroy(sphereCastHits[i].transform.root.gameObject);
                    }
                }
            }
        }

        // 범위: 360도 넓은 원(파동 퍼지기 시작~소멸까지 지속?)
        public void ExecuteSpecialAttackLogic()
        {
            Collider[] overlapObjects = Physics.OverlapSphere(transform.position, attackRadius);
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
                ExecuteDamage(damageInterface, 10f);
                // Debug.Log($"{overlapObjects[i].transform.root.name} 특수공격에 피격!");

                // @ Ability System
                if(overlapObjects[i].transform.root.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_MONSTER))
                {
                    if (characterAbilityComponent.GetAbility(AbilityTag.Push, out PushAbility abil))
                    {
                        abil.SetTargetMon(overlapObjects[i].transform.root.gameObject);
                        abil.Execute();
                    }
                    // Test용
                    GameObject targetMon = overlapObjects[i].transform.root.gameObject;
                    Vector3 pushDir = (transform.position - targetMon.transform.position).normalized;
                    targetMon.transform.Translate(pushDir);
                }
            }
        }

        public void ExecuteCombo2Move()
        {
            //Debug.Log($"콤보3Bool이벤트: {DateTime.Now}");
            isCombo2ing = !isCombo2ing;
        }

        public void MagicAim()
        {
            if (characterAttributeComponent.GetAttribute(AttributeTypes.MagicArrowCount).CurrentValue <= 0)
                return;

            // ToDo: 칼 비활성화

            isMagicAiming = true;
            characterAnimator.SetTrigger("MagicAimTrigger");
        }

        public void MagicShot()
        {
            isMagicAiming = false;
            characterAnimator.SetTrigger("MagicShotTrigger");
            Instantiate(magicArrowPrefab, transform.position + transform.forward + transform.up, transform.rotation * Quaternion.Euler(90, 0, 0));
            // ToDo: 여기서 캐릭터 정면 = 화살 정면되게 로테이션
            // 테스트 위해 일단 주석
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
                dashTimer = Time.time;
                // 쿨타임 적용해야함~~

                Vector3 currentPosition = transform.position;
                Vector3 currentForward = transform.forward;
                Vector3 predictPosition = currentPosition + (currentForward * dashSpeed * dashDuration);

                Ray ray = new Ray(predictPosition + Vector3.up, Vector3.down);
                // Raycast로 default, groundlayer 둘 다 감지 -> groundlayer 있으므로 T 반환 => 원하는 처리와 다른 결과
                // 위에서 아래로 쏘는 ray에 가장 먼저 충돌한 옵젝의 layer를 기준으로 해야 한다.
                // RaycastHit[] rayHits = Physics.RaycastAll(ray, 100f); // All 안 하면 맨 처음 것만 되는 것
                RaycastHit rayHit;
                Physics.Raycast(ray, out rayHit, 100f);
                //if(rayHits.Length > 0) // 알아서 순서대로 해줌
                //{
                //    System.Array.Sort(rayHits, (a, b) => a.distance.CompareTo(b.distance));
                //    // Debug.Log($"여러 레이어 발견, 정렬했습니다. 최상위 레이어: {rayHits[0].collider.gameObject.layer}");
                //}
                // 도착 지점이 * layer일 경우 레이어 변경 - *은 floor 하나면 됨
                if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_GROUND))
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

        private void DashEnd()
        {
            isDashing = false;
            characterAnimator.SetBool("IsDashing", false);
            gameObject.layer = LayerMask.NameToLayer(Constant.LAYER_NAME_DEFAULT);
            // dashAvailable = true;
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

        public void SetIsAttacking(bool tf)
        {
            isAttacking = tf;
        }

        //public void ResetComboIndex()
        //{
        //    comboIndex = 0;
        //}

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + (Vector3.up * characterController.radius), characterController.radius);
        }

        public void TakeDamage(IActor actor, float damage)
        {
            // 피격 이펙트 출력
            // 체력 감소 - 사망 체크
            curHP -= damage;
            // Debug.Log($"Take Damage - Attacker : {actor.GetActor().name}, Damage : {damage}");

            // @ Ability System
            // Revenge Ability 유무 체크
            // 있으면, 받은 damage의 1/4을 actor에 돌려줌
            //if (characterAbilityComponent.GetAbility(AbilityTag.Revenge, out RevengeAbility abil))
            //{
            //    abil.Init(this, actor, damage);
            //    abil.Execute();
            //}
            //// 테스트용
            //IDamage damageInterface = actor.GetActor().GetComponent<IDamage>();
            //ExecuteDamage(damageInterface, damage * 0.25f);
        }

        // @ Ability System
        //public void RevengeAttack(IActor actor, float damage)
        //{
        //    // actor(공격자)의 IDamage 가져와서 ExecuteDamage
        //    IDamage damageInterface = actor.GetActor().GetComponent<IDamage>();
        //    ExecuteDamage(damageInterface, damage * 0.25f);
        //} 

        public GameObject GetActor()
        {
            return gameObject;
        }

        // Attribute
        public void AddBuffed(AttributeTypes type, float buffedValue)
        {
            characterAttributeComponent.SetAttributeBuffedValue(type, buffedValue);
        }
    }
}