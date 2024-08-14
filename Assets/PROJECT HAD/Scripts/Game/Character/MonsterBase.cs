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
        [field: SerializeField] public MonsterAIState AIState { get; protected set; }
        [field: SerializeField] public float AttackRange { get; protected set; }

        protected PlayerCharacter targetPlayer;
        protected NavMeshAgent navMeshAgent;
        [SerializeField] protected MonsterStatData monsterStatData;

        protected override void Awake()
        {
            base.Awake();
            navMeshAgent = GetComponent<NavMeshAgent>();

            AttackRange = monsterStatData.AttackRange;
        }

        protected override void Update()
        {
            base.Update();

            switch(AIState)
            {
                case MonsterAIState.CombatState:
                    {
                        if(targetPlayer != null)
                        {
                            float distance = Vector3.Distance(targetPlayer.transform.position, transform.position);
                            if(distance <= AttackRange)
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
            }
        }

        public virtual void Attack()
        {
            characterAnimator.SetTrigger("Attack");
        }

        public virtual void Chase()
        {
            navMeshAgent.SetDestination(targetPlayer.transform.position);
        }

        public virtual void OnDetectedPlayer(PlayerCharacter player) // 캐릭터 인지
        {
            AIState = MonsterAIState.CombatState;
            targetPlayer = player;

            Chase();
        }

        public virtual void OnLostPlayer(PlayerCharacter player) // 캐릭터 인지 풀림
        {
            AIState = MonsterAIState.PeaceState;
            targetPlayer = null;
        }
    }
}

//// Move() 힌트 (주의! desiredVelocity 로컬, 월드따라 변환 필요할 수도...)
//Vector3 desiredVelocity = navMeshAgent.desiredVelocity;
//float inputX = Mathf.Clamp(desiredVelocity.x, -1, 1); // Velocity에서 크기는 빼고 방향만
//float inputZ = Mathf.Clamp(desiredVelocity.y, -1, 1);
//Move(new Vector2(inputX, inputZ), 0);