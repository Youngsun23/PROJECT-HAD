//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace HAD
//{
//    [CreateAssetMenu(fileName = "CharacterActionData", menuName = "HAD/CharacterActionData")]
//    public class CharacterActionData : ScriptableObject
//    {
//        [field: SerializeField] public string ActionStateName { get; set; }

//        // <summary>
//        // 현재 이 Class의 액션이 포함된 시퀀스의 이전/다음 액션
//        // </summary>
//        [field: SerializeField] public CharacterActionData PrevAction { get; set; }
//        [field: SerializeField] public CharacterActionData NextAction { get; set; }
//        // (일단은 간단하게 단일 등록) 배열로 받거나 Sequence 구조 잡아서 쓰거나

//        // 액션의 애니메이션 클립 기준, 어느 시점까지 다음 액션에 대한 입력키가 들어와야 시퀀스 연결 인정할 것인지 NormalizedTime 기준 %
//        [field: SerializeField, Range(0.0f, 1.0f)] public float LimitInputNormalizedTime { get; set; }
//        // 다음 액션으로 넘어가기 전에 재생해야 하는 최소한의 애니메이션 시간  NormalizedTime 기준 %
//        [field: SerializeField, Range(0.0f, 1.0f)] public float MinimumPlayClipLength { get; set; }
//        // 다른 State로 빠져나가기 전에 재생해야 하는 최소한의 애니메이션 시간 NormalizedTime 기준 %
//        [field: SerializeField, Range(0.0f, 1.0f)] public float MinimumExitNormalizedTime { get; set; }
//    }
//}
