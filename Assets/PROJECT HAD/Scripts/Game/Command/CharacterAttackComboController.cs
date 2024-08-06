using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace HAD
{
    public class CharacterAttackComboController : MonoBehaviour
    {
        private Animator animator;
        private float crossFadeTime = 0.1f;

        static readonly int Locomotion_Hash = Animator.StringToHash("Locomotion");
        static readonly int AttackCombo_1_Hash = Animator.StringToHash("AttackCombo_1");
        static readonly int AttackCombo_2_Hash = Animator.StringToHash("AttackCombo_2");
        static readonly int AttackCombo_3_Hash = Animator.StringToHash("AttackCombo_3");

        //private float AttackCombo_1_ClipLength;
        //private float AttackCombo_2_ClipLength;
        //private float AttackCombo_3_ClipLength;

        //private void Start()
        //{
        //     AttackCombo_1_ClipLength = GetAnimationClipActualDuration("AttackCombo_1");
        //     AttackCombo_2_ClipLength = GetAnimationClipActualDuration("AttackCombo_2");
        //     AttackCombo_3_ClipLength = GetAnimationClipActualDuration("AttackCombo_3");
        //    animationDurations.Add(AttackCombo_1_Hash, AttackCombo_1_ClipLength);
        //    animationDurations.Add(AttackCombo_2_Hash, AttackCombo_2_ClipLength);
        //    animationDurations.Add(AttackCombo_3_Hash, AttackCombo_3_ClipLength);
        //}

        //// 위의 시도 -> 제대로 작동하는 방식 (그냥 알아만두자~ 실무에서 이렇게는 안하지~)
        //// 직접 적으면 컴퓨터가 일 안 해도 되죠? 어차피 애니메이션 클립은 한 번 정해지면 가변적이지도 않죠?
        //public SerializableDictionary<string, AnimationClip> animationData = new SerializableDictionary<string, AnimationClip>(); // 인스펙터에서 애니메이션 클립 등록
        //private Dictionary<int, float> animationDurations2 = new Dictionary<int, float>();
        //private void Awake()
        //{
        //    animator = GetComponent<Animator>();

        //    foreach(var data in animationData)
        //    {
        //        int hashKey = Animator.StringToHash(data.Key);
        //        float animeTime = animationData[data.Key].length;
        //        animationDurations2.Add(hashKey, animeTime);
        //    }
        //}
        // return을 animationDurations2[]로 하면 된다.

        static readonly Dictionary<int, float> animationDurations = new Dictionary<int, float>()
        {
            { Locomotion_Hash, 0f },        // Length
            { AttackCombo_1_Hash, 1.015f }, // 2.033
            { AttackCombo_2_Hash, 0.9835f }, // 1.967
            { AttackCombo_3_Hash, 1.2665f }  // 2.533
            // 애니메이션 스피드, 총 길이 확정 x
            // 직접 작성 대신 길이 받아와서 쓰고 싶은데,,
        };

        private void Awake() => animator = GetComponent<Animator>();

        public float Locomotion() => PlayAnimation(Locomotion_Hash);

        public float AttackCombo(int combo)
        {
            switch(combo)
            {
                case 1:
                    return PlayAnimation(AttackCombo_1_Hash);
                case 2:
                    return PlayAnimation(AttackCombo_2_Hash);
                case 3:
                    return PlayAnimation(AttackCombo_3_Hash);
                default:
                    return 0f;
            }
        }

        public float PlayAnimation(int animationHash)
        {
            animator.CrossFade(animationHash, crossFadeTime);
            return animationDurations[animationHash];
            // return animator.GetCurrentAnimatorClipInfo(0).Length;
        }
        //private float GetAnimationClipActualDuration(string stateName)
        //{
        //    AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
        //    foreach (var layer in animatorController.layers)
        //    {
        //        foreach (var state in layer.stateMachine.states)
        //        {
        //            if (state.state.name == stateName)
        //            {
        //                AnimationClip clip = state.state.motion as AnimationClip;
        //                if (clip != null)
        //                {
        //                    AnimatorState animatorState = state.state;
        //                    float speed = animatorState.speed;
        //                    float actualDuration = clip.length / speed;
        //                    return actualDuration;
        //                }
        //            }
        //        }
        //    }

        //    // 해당 상태를 찾지 못했을 경우 기본값 반환
        //    return 0f;
        //}
    }
}
