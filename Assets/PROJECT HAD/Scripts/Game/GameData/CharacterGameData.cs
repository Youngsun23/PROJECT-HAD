using Sirenix.OdinInspector;
using UnityEngine;

namespace HAD
{
    [CreateAssetMenu(fileName = "Character GameData", menuName = "HAD/Character Data")]
    public class CharacterGameData : GameDataBase // 하데스는 레벨 개념 x -> 게임시작 초기값으로만 사용
    {
        [field: Title("Character Setting")]
        [field: SerializeField] public string Option { get; protected set; }
        [field: SerializeField] public float MaxHP { get; protected set; } = 50f;
        [field: SerializeField] public float RecoveryHP { get; protected set; } = 0;
        [field: SerializeField] public int RevivalCount { get; protected set; } = 0;
        [field: SerializeField] public int MaxArrow { get; protected set; } = 1;

        [field: Title("Movement Setting")]
        [field: SerializeField] public float MoveSpeed { get; protected set; } 

        [field: Title("Attack Setting")]
        [field: SerializeField] public float AttakRadius { get; protected set; } = 2.5f;
        [field: SerializeField] public float AttackDamage { get; protected set; } = 20f; // 콤보 20-25-30
        [field: SerializeField] public float MagicDamage { get; protected set; } = 50f;
        [field: SerializeField] public float SpecialAttackDamage { get; protected set; } = 15f;

        //# 기본치+증가치에서 이 GameData는 "기본치" 부분만. 기본치가 0인 부분도 항목은 두고 0+?로 사용할 것인가 아예 항목도 뺄 것인가는 선택
        //[field: SerializeField] public float BonusAttackDamagePercent { get; protected set; }
        //[field: SerializeField] public float BonusCriticalAttackDamage { get; protected set; }
        [field: SerializeField] public float AttackDamageResistance { get; protected set; }
        [field: SerializeField] public float TrapDamageResistance { get; protected set; }
        // 대미지 계산 공식 : 기본 피해량 * (1 + 추가 피해량 총합) * (1 + 치명타 추가 피해량) * ((1 - 피해 감소량)들의 곱)

        [field: Title("Combo Setting")]
        [field: SerializeField] public float Combo2MoveSpeed { get; protected set; } = 5f;

        [field: Title("Dash Setting")]
        [field: SerializeField] public int MaxDashCount { get; protected set; } = 1;
        [field: SerializeField] public float DashCooltime { get; protected set; } = 1f;
        [field: SerializeField] public float DashDuration { get; protected set; } = 0.4f;
        [field: SerializeField] public float DashSpeed { get; protected set; } = 10f;

        [field: Title("Resource Setting")]
        [field: SerializeField] public int Coin { get; protected set; } = 0;
        [field: SerializeField] public int Key { get; protected set; } = 0;
        [field: SerializeField] public int Darkness { get; protected set; } = 0;

    }
}