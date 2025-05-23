using UnityEngine;

namespace HAD
{
    [CreateAssetMenu(fileName ="MonsterGameData", menuName = "HAD/Monster Data")]
    public class MonsterStatData : GameDataBase
    {
        [field: SerializeField] public float MaxHP {  get; set; }
        [field: SerializeField] public float MoveSpeed {  get; set; }
        [field: SerializeField] public float AttackPossibleRange {  get; set; }
        [field: SerializeField] public float AttackRange {  get; set; }
        [field: SerializeField] public float AttackDamage {  get; set; }
        [field: SerializeField] public float AttackCooltime {  get; set; }
    }
}
