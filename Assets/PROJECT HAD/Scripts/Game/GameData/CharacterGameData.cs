using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    [CreateAssetMenu(fileName = "Character GameData", menuName = "HAD/Game Data/Character Data")]
    public class CharacterGameData : GameDataBase
    {
        [field: Title("Character Setting")]
        [field: SerializeField] public int Level { get; protected set; }
        [field: SerializeField] public float MaxHP { get; protected set; }
        [field: SerializeField] public int MaxArrow { get; protected set; } 

        [field: Title("Movement Setting")]
        [field: SerializeField] public float MoveSpeed { get; protected set; }

        [field: Title("Attack Setting")]
        [field: SerializeField] public float AttakRadius { get; protected set; } = 2.5f;
        [field: SerializeField] public float AttackPower { get; protected set; } = 10f;
        [field: SerializeField] public float MagicPower { get; protected set; } = 10f;
        [field: SerializeField] public float SpecialAttackPower { get; protected set; } = 15f;
        
        [field: Title("Combo Setting")]
        [field: SerializeField] public float Combo2MoveSpeed { get; protected set; } = 5f;

        [field: Title("Dash Setting")]
        [field: SerializeField] public float DashCooltime { get; protected set; } = 1f;
        [field: SerializeField] public float DashDuration { get; protected set; } = 0.4f;
        [field: SerializeField] public float DashSpeed { get; protected set; } = 10f;
    }
}