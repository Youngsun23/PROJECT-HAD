using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class PlayerCharacter : CharacterBase
    {
        public bool IsDashing => isDashing;
        public bool IsAttacking => isAttacking;
        public bool IsMagicAiming => isMagicAiming;

        public LayerMask ignoreLayerMask;
        public LayerMask sensorLayerMask;

        private CharacterAbilityComponent characterAbilityComponent;
        public CharacterAbilityComponent CharacterAbilityComponent => characterAbilityComponent;
        public CharacterAttackComboController CharacterAttackComboController => characterAttackComboController;

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

        // public GameObject reflectionAbilFX;

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
        //[SerializeField] private float dashCooltime;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float combo2MoveSpeed;
        [SerializeField] private float attackRadius;
        [SerializeField] private float dashDuration;

        // ====   함수   ====

        protected override void Awake()
        {
            base.Awake();

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

        protected override void Update()
        {
            base.Update();

            // 대시 쿨타임 체크해서 DashEnd, DashMove 해주는 부분 어디갔냐;;
            if (!dashAvailable)
            {
                if (Time.time > dashTimer + characterAttributeComponent.GetAttribute(AttributeTypes.DashCooltime).CurrentValue)
                {
                    dashAvailable = true;
                }
                else if (Time.time > dashTimer + dashDuration)
                {
                    DashEnd();
                }
                else
                {
                    DashMove();
                }
            }

            if (isCombo2ing)
            {
                Combo2Move();
            }

            if (isMagicAiming)
            {
                MouseRotate();
                // ToDo: 날아갈 경로 보여주기
            }

        }

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

            // GameData의 초기값 + UserData의 변화 정보&변화 내용의 설정 변화값(ex.켄타심장1개-+25HP) => Component
            characterAttributeComponent.SetAttribute(AttributeTypes.HealthPoint, characterData.MaxHP /*+ UserDataManager.Singleton.CalUserDataMaxHP()*/);
            characterAttributeComponent.SetAttribute(AttributeTypes.MagicArrowCount, characterData.MaxArrow);
            characterAttributeComponent.SetAttribute(AttributeTypes.DashCooltime, characterData.DashCooltime);
            characterAttributeComponent.SetAttribute(AttributeTypes.MoveSpeed, characterData.MoveSpeed);
            characterAttributeComponent.SetAttribute(AttributeTypes.AttackDamage, characterData.AttackDamage);
            characterAttributeComponent.SetAttribute(AttributeTypes.MagicDamage, characterData.MagicDamage);
            characterAttributeComponent.SetAttribute(AttributeTypes.SpecialAttackDamage, characterData.SpecialAttackDamage);

            // 아래는 벞/디벞/강화 안 하는 요소
            // 그대로 써도 되나?
            combo2MoveSpeed = characterData.Combo2MoveSpeed;
            attackRadius = characterData.AttakRadius;
            dashSpeed = characterData.DashSpeed;
            dashDuration = characterData.DashDuration;
        }

        public override void TakeDamage(IActor actor, float damage)
        {
            // base.TakeDamage(actor, damage);

            // ToDo: 피격 이펙트 출력

            // float currentHP = characterAttributeComponent.GetAttribute(AttributeTypes.HealthPoint).CurrentValue;
            // characterAttributeComponent.SetAttributeCurrentValue(AttributeTypes.HealthPoint, currentHP - damage);
            // Debug.Log($"Take Damage - Attacker : {actor.GetActor().name}, Damage : {damage}");
            characterAttributeComponent.DecreaseCurrentHP(damage);
            // # ToDo: 이 부분

            // Debug.Log($"캐릭터가 공격받았다! by {actor.GetActor().name}");
            // Debug.Log($"캐릭터 체력: {characterAttributeComponent.GetAttribute(AttributeTypes.HealthPoint).CurrentValue}");

            // 체력 감소 - 사망 체크
            if (characterAttributeComponent.GetAttribute(AttributeTypes.HealthPoint).CurrentValue <= 0)
            {
                Die();
            }

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

        public void Die()
        {
            // ToDo
            Debug.Log("--- Player Died ---");
            UserDataManager.Singleton.ResetTempUserData();

            // 죽음 -> 부활 연출

            // 하데스의집 씬으로 이동

        }

        public override void AddBuffed(AttributeTypes type, float buffedValue)
        {
            base.AddBuffed(type, buffedValue);

            characterAttributeComponent.SetAttributeBuffedValue(type, buffedValue);

        }

        //// Attributes
        //// 이렇게 사용도 가능
        //public float CurrentHP
        //{
        //    get => characterAttributeComponent.GetAttribute(AttributeTypes.HealthPoint).CurrentValue;
        //    set => characterAttributeComponent.SetAttributeCurrentValue(AttributeTypes.HealthPoint, value);
        //}

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
            else // ray 충돌 대상 없는 빈 곳의 경우
            {
                Vector3 destPos = ray.GetPoint(100f);
                destPos.y = transform.position.y;
                Vector3 dir = destPos - transform.position;
                Quaternion lookTarget = Quaternion.LookRotation(dir);
                transform.rotation = lookTarget;
            }
        }
        // ??? 얘는 왜 갑자기 고장남??
        // BoundingVolume 옵젝들에 먼저 걸려서 그런가보다...레이어로 빼서 무시

        public void ExecuteAttackLogic(string parameter)
        {
            // parameter => "BasicAttack"

            Collider[] overlapObjects = Physics.OverlapSphere(transform.position, attackRadius, ~sensorLayerMask);
            // Debug.Log($"attackRadius: {attackRadius}"); // attackRadius가 몇이든 그려지는 Ray 같음
            // 몬스터가 들고 있는 센서 콜라이더때문일듯???
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

                    if (overlapObjects[i].transform.root.gameObject.CompareTag("Monster"))
                    {
                        Debug.DrawLine(transform.position + Vector3.up, overlapObjects[i].transform.root.position + Vector3.up, Color.red, 1f);
                        
                        var damageInterface = overlapObjects[i].transform.root.GetComponent<IDamage>();
                        ExecuteDamage(damageInterface, 10f);
                        // Debug.Log($"{overlapObjects[i].transform.root.name} 공격에 피격!");
                    }

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
            RaycastHit[] sphereCastHits = Physics.SphereCastAll(transform.position + Vector3.up, attackRadius, transform.forward, attackRadius, ~sensorLayerMask);

            if (sphereCastHits == null || sphereCastHits.Length == 0)
            {
                // To do : 공격 모션이나 이펙트만 출력해주고 빠져나감

                return;
            }

            // To do : 실제 공격 로직을 구현한다.
            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                if (sphereCastHits[i].transform.root.gameObject.CompareTag("Monster"))
                {
                    var damageInterface = sphereCastHits[i].collider.transform.root.GetComponent<IDamage>();
                    ExecuteDamage(damageInterface, 10f);
                    Debug.DrawLine(transform.position + Vector3.up, sphereCastHits[i].collider.transform.root.position + Vector3.up, Color.green, 1f);
                    // Debug.Log($"{sphereCastHits[i].collider.transform.root.name} 콤보1에 피격!");
                }

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
            RaycastHit[] sphereCastHits = Physics.SphereCastAll(transform.position + Vector3.up, attackRadius, transform.forward, attackRadius * 2, ~sensorLayerMask);
            if (sphereCastHits == null || sphereCastHits.Length == 0)
            {
                // To do : 공격 모션이나 이펙트만 출력해주고 빠져나감

                return;
            }

            // To do : 실제 공격 로직을 구현한다.
            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                if (sphereCastHits[i].transform.root.gameObject.CompareTag("Monster"))
                {
                    var damageInterface = sphereCastHits[i].collider.transform.root.GetComponent<IDamage>();
                    ExecuteDamage(damageInterface, 10f);
                    Debug.DrawLine(transform.position + Vector3.up, sphereCastHits[i].collider.transform.root.position + Vector3.up, Color.black, 1f);
                    // Debug.Log($"{sphereCastHits[i].collider.transform.root.name} 콤보2에 피격!");
                }

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
            Collider[] overlapObjects = Physics.OverlapSphere(transform.position, attackRadius, ~sensorLayerMask);
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

                // 체크를 안 해주면 캐릭터 자신의 콜라이더도 걸려서 스스로 피해입는듯?
                if(overlapObjects[i].transform.root.gameObject.CompareTag("Monster"))
                {
                    var damageInterface = overlapObjects[i].transform.root.GetComponent<IDamage>();
                    ExecuteDamage(damageInterface, 10f);
                    // Debug.Log($"{overlapObjects[i].transform.root.name} 특수공격에 피격!");
                }

                // @ Ability System
                if (overlapObjects[i].transform.root.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_MONSTER))
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

            // Test용 - Abil 추가 UI
            //PushAbility pushAbil = new PushAbility();
            //characterAbilityComponent.AddAbility(pushAbil);
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

        // @ Ability System
        //public void RevengeAttack(IActor actor, float damage)
        //{
        //    // actor(공격자)의 IDamage 가져와서 ExecuteDamage
        //    IDamage damageInterface = actor.GetActor().GetComponent<IDamage>();
        //    ExecuteDamage(damageInterface, damage * 0.25f);
        //} 
    }
}
