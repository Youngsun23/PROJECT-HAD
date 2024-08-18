using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                //character.ResetComboIndex();
                //animator.ResetTrigger("Attack1Trigger");
            }
        }
    }
}
