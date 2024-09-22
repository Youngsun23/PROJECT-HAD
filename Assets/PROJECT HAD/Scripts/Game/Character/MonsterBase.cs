using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace HAD
{
    public enum MonsterAIState
    {
        PeaceState,
        CombatState,
        
    }

    // @ 임시 코드 - 몬스터 스폰 시스템 미구현 base @

    public class MonsterBase : CharacterBase
    {
        public static List<MonsterBase> spawnedMonsters = new List<MonsterBase>();
        public static System.Action<int, Vector3> OnSpawnedMonsterCountChanged;

        [field: SerializeField] public MonsterAIState AIState { get; protected set; }
        [field: SerializeField] public float MaxHP { get; protected set; }
        [field: SerializeField] public float MoveSpeed { get; protected set; }
        [field: SerializeField] public float AttackPossibleRange { get; protected set; }
        [field: SerializeField] public float AttackRange { get; protected set; }
        [field: SerializeField] public float AttackDashSpeed { get; protected set; }
        [field: SerializeField] public float AttackDamage { get; protected set; }
        [field: SerializeField] public float AttackCooltime { get; protected set; }

        [SerializeField] private float currentHP; // 따로 두고 사용

        protected PlayerCharacter targetPlayer;
        protected NavMeshAgent navMeshAgent;
        [SerializeField] protected MonsterStatData monsterStatData;
        public MonsterOwnedSensor monsterOwnedSensor;
        private float patrolTimer = 3f;
        private float timeSinceLastPatrolTimer;
        private float timeSinceLastAttackTimer;
        private float attackMotionTime = 2.18f;
        [SerializeField] private bool attackAvailable = false;
        private bool isAttacking = false;
        private float stunTimer = 0f;

        // public GameObject monsterPathBoxObject;
        // BoxCollider monsterPathBoxCollider;

        protected override void Awake()
        {
            base.Awake();

            spawnedMonsters.Add(this);
            // OnSpawnedMonsterCountChanged?.Invoke(spawnedMonsters.Count, this.transform.position);
            // Debug.Log($"spawnedMonsters: {spawnedMonsters.Count}");

            navMeshAgent = GetComponent<NavMeshAgent>();
            // monsterPathBoxCollider = monsterPathBoxObject.GetComponent<BoxCollider>();  

            MaxHP = monsterStatData.MaxHP;
            MoveSpeed = monsterStatData.MoveSpeed;
            AttackPossibleRange = monsterStatData.AttackPossibleRange;
            AttackRange = monsterStatData.AttackRange;
            AttackDamage = monsterStatData.AttackDamage;
            AttackCooltime = monsterStatData.AttackCooltime;
        }

        // 콜백 아닌 경우, 미리 호출하지말라고 빼줄 수도 없고...(이것땜시 콜백으로 해야하나...?)
        protected override void OnDestroy() // 이걸 오타내놓고 모른 내가 무섭다...
        {
            spawnedMonsters.Remove(this);
            
            Debug.Log($"몬스터 죽음 -> spawnedMonstersCount: {spawnedMonsters.Count}");
            // OnSpawnedMonsterCountChanged?.Invoke(spawnedMonsters.Count, this.transform.position);

            if(spawnedMonsters.Count <= 0)
            {
                if(MonsterWaveManager.Instance) // null 체크
                {
                    MonsterWaveManager.Instance.NotifyLastMonsterDead(this);
                }
            }
        }

        protected override void Start()
        {
            monsterOwnedSensor.OnDetectedTarget += OnDetectedPlayer;
            monsterOwnedSensor.OnLostTarget += OnLostPlayer;

            currentHP = MaxHP;
            timeSinceLastPatrolTimer = Time.time;
            timeSinceLastAttackTimer = Time.time;

            // navMeshAgent.updatePosition = false;
            // navMeshAgent.updateRotation = false;
        }

            void OnDrawGizmos()
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
            }

        protected override void Update()
        {
            if (stunTimer > 0)
            {
                stunTimer -= Time.deltaTime;
                return;
            }

            base.Update();

            if (Time.time >= timeSinceLastAttackTimer + attackMotionTime)
            {
                isAttacking = false;
                if (Time.time >= timeSinceLastAttackTimer + AttackCooltime)
                {
                    attackAvailable = true;
                }
            }

            // 실제 이동
            if (!isAttacking)
            {
                Vector3 desiredVelocity = navMeshAgent.desiredVelocity;
                // Debug.Log($"DesiredVelocity: {desiredVelocity}");
                //if(desiredVelocity != Vector3.zero) // if를 돌리면 idle이 막히고(ok) if를 막으면 move가 막힘(왜?)
                // if (desiredVelocity.magnitude > 0.1f)  // 일정 크기 이상인 경우에만 이동
                {
                    float inputX = Mathf.Clamp(desiredVelocity.x, -1, 1);
                    float inputZ = Mathf.Clamp(desiredVelocity.z, -1, 1);
                    Move(new Vector2(inputX, inputZ), 0f);
                }
            }

            switch (AIState)
            {
                case MonsterAIState.CombatState:
                    {
                        if(targetPlayer != null && !isAttacking)
                        {
                            float distance = Vector3.Distance(targetPlayer.transform.position, transform.position);
                            if(distance <= AttackPossibleRange && attackAvailable)
                            {
                                Attack();
                            }
                            else
                            {
                                Chase();
                            }
                        }
                    }
                    break;
                case MonsterAIState.PeaceState:
                    {
                        if (Time.time >= timeSinceLastPatrolTimer + patrolTimer)
                        {
                            Patrol();
                        }
                    }
                    break;
            }
        }

        public virtual void Patrol()
        {
            // Debug.Log("Patrol!");

            // 랜덤 좌표 목적지로 설정 -> 실제 이동은 Update에서
            Vector3 randomPosition = GetRandomPositionOnNavMesh();
            // Vector3 randomPosition = GetRandomPositionInPathBox();
            navMeshAgent.SetDestination(randomPosition);
        }

        public virtual void Attack()
        {
            // Debug.Log("Attack!");

            isAttacking = true;
            timeSinceLastAttackTimer = Time.time;
            attackAvailable = false;

            characterAnimator.SetTrigger("AttackTrigger");
        }

        public virtual void Chase()
        {
            // Debug.Log("Chase!");

            navMeshAgent.SetDestination(targetPlayer.transform.position);
            // 실제 이동은 Update에서
        }

        public virtual void OnDetectedPlayer(PlayerCharacter player) // 캐릭터 인지
        {
            // Debug.Log("Detected!"); 

            AIState = MonsterAIState.CombatState;
            targetPlayer = player;

            Chase();
        }

        public virtual void OnLostPlayer(PlayerCharacter player) // 캐릭터 인지 풀림
        {
            // Debug.Log("Lost!");

            AIState = MonsterAIState.PeaceState;
            targetPlayer = null;
        }
        
        Vector3 GetRandomPositionOnNavMesh()
        {
            Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * 10f;
            randomPosition += transform.position;
            randomPosition.y = transform.position.y;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, 10f, NavMesh.AllAreas))
            {
                timeSinceLastPatrolTimer = Time.time;

                return hit.position; 
            }
            else
            {
                timeSinceLastPatrolTimer = Time.time + 2.5f; // 0.5f간 이동x

                return transform.position; 
            }
        }

        #region 박스 랜덤 좌표 - 이상 작동으로 주석화
        //Vector3 GetRandomPositionInPathBox()
        //{
        //    Vector3 originPosition = monsterPathBoxObject.transform.position;
        //    Vector3 localCenter = monsterPathBoxCollider.center;
        //    Vector3 localSize = monsterPathBoxCollider.size;

        //    float randomX = UnityEngine.Random.Range(-localSize.x / 2, localSize.x / 2);
        //    float randomZ = UnityEngine.Random.Range(-localSize.z / 2, localSize.z / 2);

        //    Vector3 randomLocalPosition = new Vector3(randomX, 0f, randomZ);
        //    Vector3 randomWorldPosition = monsterPathBoxCollider.transform.TransformPoint(localCenter + randomLocalPosition);

        //    NavMeshHit hit;
        //    if (NavMesh.SamplePosition(randomWorldPosition, out hit, 10f, NavMesh.AllAreas))
        //    {
        //        randomWorldPosition = hit.position;
        //    }

        //    randomWorldPosition.y = transform.position.y;
        //    return randomWorldPosition;
        //}
        #endregion


        public override void TakeDamage(IActor actor, float damage)
        {
            stunTimer = 2f;

            characterAnimator.SetTrigger("HitTrigger");

            currentHP -= damage;
            // Debug.Log($"Monster Damaged! Current HP: {currentHP}");

            if(currentHP <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            characterAnimator.SetTrigger("DieTrigger");

            Destroy(gameObject, 1f);

            // ToDo
            Debug.Log($"----- {this.gameObject.name} Died -----");
        }
    }
}

