using UnityEngine;

namespace HAD
{
    [CreateAssetMenu(fileName ="MonsterGameData", menuName = "HAD/Monster Data")]
    public class MonsterStatData : ScriptableObject
    {
        [field: SerializeField] public float MaxHP {  get; set; }
        // currentHP?
        [field: SerializeField] public float MoveSpeed {  get; set; }
        // [field: SerializeField] public float AttackDashSpeed {  get; set; }
        [field: SerializeField] public float AttackPossibleRange {  get; set; }
        [field: SerializeField] public float AttackRange {  get; set; }
        [field: SerializeField] public float AttackDamage {  get; set; }
        [field: SerializeField] public float AttackCooltime {  get; set; }

        // 스킬1, 스킬2, ...
    }
}
