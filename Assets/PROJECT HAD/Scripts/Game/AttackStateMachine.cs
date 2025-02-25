using UnityEngine;

namespace HAD
{
    public class AttackStateMachine : StateMachineBehaviour
    {
        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            if (animator.transform.root.TryGetComponent(out PlayerCharacter character)) 
            {
                character.SetIsAttacking(true);
            }
        }
        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            if(animator.transform.root.TryGetComponent(out PlayerCharacter character))
            {
                character.SetIsAttacking(false);
            }
        }
    }
}
