using System.Collections;
using System.Collections.Generic;
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

        static readonly Dictionary<int, float> animationDurations = new Dictionary<int, float>()
        {
            { Locomotion_Hash, 0f },
            { AttackCombo_1_Hash, 1.5f },
            { AttackCombo_2_Hash, 1.5f },
            { AttackCombo_3_Hash, 1.5f }
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
        }
    }
}
