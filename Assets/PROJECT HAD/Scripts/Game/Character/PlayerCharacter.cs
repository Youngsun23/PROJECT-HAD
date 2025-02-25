using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace HAD
{
    public class PlayerCharacter : CharacterBase
    {
        public static PlayerCharacter Instance { get; private set; }

        public float MaxHP => characterAttributeComponent.GetAttribute(AttributeTypes.HealthPoint).MaxValue;
        public float CurHP => characterAttributeComponent.GetAttribute(AttributeTypes.HealthPoint).CurrentValue;
        public float MaxArrow => characterAttributeComponent.GetAttribute(AttributeTypes.MagicArrowCount).MaxValue;
        public float CurArrow => characterAttributeComponent.GetAttribute(AttributeTypes.MagicArrowCount).CurrentValue;

        public bool IsDashing => isDashing;
        public bool IsAttacking => isAttacking;
        public bool IsMagicAiming => isMagicAiming;

        public LayerMask ignoreLayerMask;
        public LayerMask sensorLayerMask;

        private CharacterAbilityComponent characterAbilityComponent;
        public CharacterAbilityComponent CharacterAbilityComponent => characterAbilityComponent;
        public CharacterAttackComboController CharacterAttackComboController => characterAttackComboController;
        private CharacterAttackComboController characterAttackComboController;
        private CharacterCommandManager characterCommandManager;

        [Title("Character Attack")]
        public GameObject magicArrowPrefab;
        private bool isMagicAiming;
        private bool isCombo2ing = false;

        [Title("Character Dash")]
        private bool dashAvailable = false;
        private bool isDashing = false;
        private float dashTimer = 0f;

        [SerializeField] private float dashSpeed;
        [SerializeField] private float combo2MoveSpeed;
        [SerializeField] private float attackRadius;
        [SerializeField] private float dashDuration;
        public int CurrentAttackComboIndex => attackComboIndex;
        private int attackComboIndex = 0;
        public bool isAttacking = false;
        private bool isNeedExecuteNextCombo = false;
        public bool IsNeedExecuteNextCombo => isNeedExecuteNextCombo;


        protected override void Awake()
        {
            base.Awake();

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            characterAbilityComponent = GetComponent<CharacterAbilityComponent>();
            characterAttackComboController = GetComponent<CharacterAttackComboController>();
            characterCommandManager = GetComponent<CharacterCommandManager>();
            characterAttributeComponent = GetComponent<CharacterAttributeComponent>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (Instance == this)
            {
                Instance = null;
            }
        }

        protected override void Start()
        {
            base.Start();

            characterAttributeComponent.RegisterEvent(AttributeTypes.HealthPoint, (float max, float cur) => HUDUI.Instance.UpdateHUDUIHP(max, cur));
            characterAttributeComponent.RegisterEvent(AttributeTypes.MagicArrowCount, (float max, float cur) => HUDUI.Instance.UpdateHUDUIMagic(max, cur));
        }

        protected override void Update()
        {
            base.Update();

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
            }

        }

        public void PerformAttackCombo()
        {
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
        }

        public void ResetComboIndex()
        {
            isAttacking = false;
            isNeedExecuteNextCombo = false;
            attackComboIndex = 0;
        }

        public void InitializeCharacter(CharacterGameData characterData)
        {
            int mirrorIncreamentHP = GameDataModel.Singleton.GetMirrorGameData(1).IncreamentAtLevel[UserDataManager.Singleton.UserData.mirrorDic[1]];
            characterAttributeComponent.SetAttribute(AttributeTypes.HealthPoint, characterData.MaxHP, mirrorIncreamentHP);
            characterAttributeComponent.SetAttribute(AttributeTypes.MagicArrowCount, characterData.MaxArrow);
            characterAttributeComponent.SetAttribute(AttributeTypes.DashCooltime, characterData.DashCooltime);
            characterAttributeComponent.SetAttribute(AttributeTypes.MoveSpeed, characterData.MoveSpeed);
            characterAttributeComponent.SetAttribute(AttributeTypes.AttackDamage, characterData.AttackDamage);
            characterAttributeComponent.SetAttribute(AttributeTypes.MagicDamage, characterData.MagicDamage);
            characterAttributeComponent.SetAttribute(AttributeTypes.SpecialAttackDamage, characterData.SpecialAttackDamage);
            combo2MoveSpeed = characterData.Combo2MoveSpeed;
            attackRadius = characterData.AttakRadius;
            dashSpeed = characterData.DashSpeed;
            dashDuration = characterData.DashDuration;

            HUDUI.Instance.UpdateHUDUIHP(MaxHP, CurHP);
            HUDUI.Instance.UpdateHUDUIMagic(MaxArrow, CurArrow);
            HUDUI.Instance.UpdateHUDUICoin(UserDataManager.Singleton.UserData.coin);
            HUDUI.Instance.UpdateHUDUIDarkness(UserDataManager.Singleton.UserData.darkness);
        }



        public override void TakeDamage(IActor actor, float damage)
        {
            characterAnimator.SetTrigger("HitTrigger");
            var effect = EffectPoolManager.Singleton.GetEffect("PCTakeDamage");
            effect.gameObject.transform.position = transform.position + Vector3.up;
            var bloodFrameUI = UIManager.Singleton.GetUI<BloodFrameUI>(UIList.BloodFrame);
            bloodFrameUI.Show();
            CameraSystem.Instance.ShakeCamera();

            float currentHP = characterAttributeComponent.GetAttribute(AttributeTypes.HealthPoint).CurrentValue;
            characterAttributeComponent.SetAttributeCurrentValue(AttributeTypes.HealthPoint, currentHP - damage);
            
            if (characterAttributeComponent.GetAttribute(AttributeTypes.HealthPoint).CurrentValue <= 0f)
            {
                Die();
                return; 
            }
        }

        public void Die()
        {
            characterAnimator.SetTrigger("DieTrigger");
            UserDataManager.Singleton.ResetTempUserData();
            HUDUI.Instance.Hide();
            DashAbility dashAbil = new DashAbility();
            characterAbilityComponent.AddAbility(dashAbil);

            GameManager.Instance.LoadLevel("Entrance");

            characterAttributeComponent.SetAttributeCurrentValue(AttributeTypes.HealthPoint, MaxHP);
            HUDUI.Instance.Show();
            InitializeCharacter(GameDataModel.Singleton.GetPlayerCharacterGameData("Default"));

        }

        public override void AddBuffed(AttributeTypes type, float buffedValue)
        {
            base.AddBuffed(type, buffedValue);

            characterAttributeComponent.SetAttributeBuffedValue(type, buffedValue);

        }

        public void DashMove()
        {
            characterController.Move(transform.forward * dashSpeed * Time.deltaTime);
        }

        private void Combo2Move()
        {
            characterController.Move(transform.forward * combo2MoveSpeed * Time.deltaTime);
        }

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
            else
            {
                Vector3 destPos = ray.GetPoint(100f);
                destPos.y = transform.position.y;
                Vector3 dir = destPos - transform.position;
                Quaternion lookTarget = Quaternion.LookRotation(dir);
                transform.rotation = lookTarget;
            }
        }

        public void ExecuteAttackLogic(string parameter)
        {
            Collider[] overlapObjects = Physics.OverlapSphere(transform.position, attackRadius, ~sensorLayerMask);

            if (overlapObjects == null || overlapObjects.Length == 0)
            {
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

                Vector3 position = overlapObjects[i].transform.root.position;
                Vector3 direction = (position - transform.position).normalized;
                float dotAngle = Vector3.Dot(transform.forward, direction); 
                                                                            

                if (dotAngle > 0.5f)
                {

                    if (overlapObjects[i].transform.root.gameObject.CompareTag("Monster"))
                    {
                        Debug.DrawLine(transform.position + Vector3.up, overlapObjects[i].transform.root.position + Vector3.up, Color.red, 1f);
                        
                        var damageInterface = overlapObjects[i].transform.root.GetComponent<IDamage>();
                        ExecuteDamage(damageInterface, 10f);
                    }

                    if (overlapObjects[i].transform.root.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_MONPROJECTILE))
                    {
                        if (characterAbilityComponent.GetAbility(AbilityTag.Reflection, out ReflectionAbility abil))
                        {
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

        public void ExecuteCombo1Logic()
        {
            RaycastHit[] sphereCastHits = Physics.SphereCastAll(transform.position + Vector3.up, attackRadius, transform.forward, attackRadius, ~sensorLayerMask);

            if (sphereCastHits == null || sphereCastHits.Length == 0)
            {
                return;
            }

            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                if (sphereCastHits[i].transform.root.gameObject.CompareTag("Monster"))
                {
                    var damageInterface = sphereCastHits[i].collider.transform.root.GetComponent<IDamage>();
                    ExecuteDamage(damageInterface, 10f);
                    Debug.DrawLine(transform.position + Vector3.up, sphereCastHits[i].collider.transform.root.position + Vector3.up, Color.green, 1f);
                }

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

        public void ExecuteCombo2Logic()
        {
            RaycastHit[] sphereCastHits = Physics.SphereCastAll(transform.position + Vector3.up, attackRadius, transform.forward, attackRadius * 2, ~sensorLayerMask);
            if (sphereCastHits == null || sphereCastHits.Length == 0)
            {
                return;
            }

            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                if (sphereCastHits[i].transform.root.gameObject.CompareTag("Monster"))
                {
                    var damageInterface = sphereCastHits[i].collider.transform.root.GetComponent<IDamage>();
                    ExecuteDamage(damageInterface, 10f);
                    Debug.DrawLine(transform.position + Vector3.up, sphereCastHits[i].collider.transform.root.position + Vector3.up, Color.black, 1f);
                }

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

        public void ExecuteSpecialAttackLogic()
        {
            Collider[] overlapObjects = Physics.OverlapSphere(transform.position, attackRadius, ~sensorLayerMask);
            if (overlapObjects == null || overlapObjects.Length == 0)
            {
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

                if(overlapObjects[i].transform.root.gameObject.CompareTag("Monster"))
                {
                    var damageInterface = overlapObjects[i].transform.root.GetComponent<IDamage>();
                    ExecuteDamage(damageInterface, 10f);
                }

                if (overlapObjects[i].transform.root.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_MONSTER))
                {
                    if (characterAbilityComponent.GetAbility(AbilityTag.Push, out PushAbility abil))
                    {
                        abil.SetTargetMon(overlapObjects[i].transform.root.gameObject);
                        abil.Execute();
                    }
                    GameObject targetMon = overlapObjects[i].transform.root.gameObject;
                    Vector3 pushDir = (transform.position - targetMon.transform.position).normalized;
                    targetMon.transform.Translate(pushDir);
                }
            }
        }

        public void ExecuteCombo2Move()
        {
            isCombo2ing = !isCombo2ing;
        }

        public void MagicAim()
        {
            if (characterAttributeComponent.GetAttribute(AttributeTypes.MagicArrowCount).CurrentValue <= 0)
                return;

            isMagicAiming = true;
            characterAnimator.SetTrigger("MagicAimTrigger");
        }

        public void MagicShot()
        {
            if (!isMagicAiming)
                return;

            isMagicAiming = false;
            characterAnimator.SetTrigger("MagicShotTrigger");
            Instantiate(magicArrowPrefab, transform.position + transform.forward + transform.up, transform.rotation * Quaternion.Euler(90, 0, 0));
            float currentArrow = characterAttributeComponent.GetAttribute(AttributeTypes.MagicArrowCount).CurrentValue;
            characterAttributeComponent.SetAttributeCurrentValue(AttributeTypes.MagicArrowCount, currentArrow - 1f);
        }

        public void SpecialAttack()
        {
            MouseRotate();
            characterAnimator.SetTrigger("SpecialAttackTrigger");
        }

        public void Dash()
        {
            if (dashAvailable && (characterAbilityComponent.GetAbility(AbilityTag.Dash, out DashAbility abil)))
            {
                dashAvailable = false;
                isDashing = true;
                dashTimer = Time.time;

                Vector3 currentPosition = transform.position;
                Vector3 currentForward = transform.forward;
                Vector3 predictPosition = currentPosition + (currentForward * dashSpeed * dashDuration);

                Ray ray = new Ray(predictPosition + Vector3.up, Vector3.down);
                RaycastHit rayHit;
                Physics.Raycast(ray, out rayHit, 100f);

                if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer(Constant.LAYER_NAME_GROUND))
                {
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
        }

        public void SetIsAttacking(bool tf)
        {
            isAttacking = tf;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + (Vector3.up * characterController.radius), characterController.radius);
        }
    }
}
