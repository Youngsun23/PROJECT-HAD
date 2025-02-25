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

        [SerializeField] private float currentHP;

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
        private Coroutine hitCoroutine;

        protected override void Awake()
        {
            base.Awake();

            spawnedMonsters.Add(this);

            navMeshAgent = GetComponent<NavMeshAgent>();

            MaxHP = monsterStatData.MaxHP;
            MoveSpeed = monsterStatData.MoveSpeed;
            AttackPossibleRange = monsterStatData.AttackPossibleRange;
            AttackRange = monsterStatData.AttackRange;
            AttackDamage = monsterStatData.AttackDamage;
            AttackCooltime = monsterStatData.AttackCooltime;
        }

        protected override void OnDestroy() 
        {
            spawnedMonsters.Remove(this);
            
            if(spawnedMonsters.Count <= 0)
            {
                if(MonsterWaveManager.Instance) 
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

            if (!isAttacking)
            {
                Vector3 desiredVelocity = navMeshAgent.desiredVelocity;
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
            Vector3 randomPosition = GetRandomPositionOnNavMesh();
            navMeshAgent.SetDestination(randomPosition);
        }

        public virtual void Attack()
        {
            isAttacking = true;
            timeSinceLastAttackTimer = Time.time;
            attackAvailable = false;

            characterAnimator.SetTrigger("AttackTrigger");
        }

        public virtual void Chase()
        {
            navMeshAgent.SetDestination(targetPlayer.transform.position);
        }

        public virtual void OnDetectedPlayer(PlayerCharacter player) 
        {
            AIState = MonsterAIState.CombatState;
            targetPlayer = player;

            Chase();
        }

        public virtual void OnLostPlayer(PlayerCharacter player) 
        {
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
                timeSinceLastPatrolTimer = Time.time + 2.5f;

                return transform.position; 
            }
        }

        public override void TakeDamage(IActor actor, float damage)
        {
            if(hitCoroutine != null)
            {
                StopCoroutine(hitCoroutine);
            }
            hitCoroutine = StartCoroutine(HitCoroutine(actor.GetActor().transform.forward));

            characterAnimator.SetTrigger("HitTrigger");

            var effect = EffectPoolManager.Singleton.GetEffect("MonsterTakeDamage");
            effect.gameObject.transform.position = transform.position + Vector3.up;

            currentHP -= damage;

            if(currentHP <= 0)
            {
                Die();
            }
        }

        private IEnumerator HitCoroutine(Vector3 direction)
        {
            stunTimer = 2f;
            float knockbackDuration = stunTimer / 2;
            float knockbackSpeed = MoveSpeed * 1.2f;
            float elapsedTime = 0f;

            navMeshAgent.isStopped = true;

            while (elapsedTime < knockbackDuration)
            {
                float normalizedTime = elapsedTime / knockbackDuration; 
                float easeOutFactor = 1f - Mathf.Pow(normalizedTime, 2);
                transform.position += direction.normalized * knockbackSpeed * easeOutFactor * Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }


            navMeshAgent.isStopped = false;

            hitCoroutine = null;
        }

        public void Die()
        {
            characterAnimator.SetTrigger("DieTrigger");
            stunTimer = 2f;

            Destroy(gameObject, 2f);
        }
    }
}

