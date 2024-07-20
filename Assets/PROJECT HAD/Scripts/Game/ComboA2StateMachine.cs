//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace HAD
//{
//    public class ComboA2StateMachine : StateMachineBehaviour
//    {
//        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//        {
//            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.8f)
//            animator.GetComponent<UnityEngine.CharacterController>().Move(animator.transform.forward * Time.deltaTime * 2f);
//        }
//    }
//}
