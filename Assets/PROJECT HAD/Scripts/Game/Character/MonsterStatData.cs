using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    [CreateAssetMenu(fileName ="MonsterStateData", menuName = "HAD/MonsterStateData", order = 0)]
    public class MonsterStatData : ScriptableObject
    {
        [field: SerializeField] public float MaxHP {  get; set; }
        [field: SerializeField] public float AttackRange {  get; set; }
        [field: SerializeField] public float AttackDamage {  get; set; }
        [field: SerializeField] public float AttackCooltime {  get; set; }

    }
}
