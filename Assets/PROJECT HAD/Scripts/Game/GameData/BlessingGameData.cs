using UnityEngine;
using UnityEngine.UI;

namespace HAD
{
    [CreateAssetMenu(fileName = "Blessing GameData", menuName = "HAD/Blessing Data")]
    public class BlessGameData : GameDataBase
    {
        // 신마다 따로 묶는 편이...
        // [field: SerializeField] public string God { get; protected set; }
        [field: SerializeField] public string Name { get; protected set; }
        [field: SerializeField] public Image Icon { get; protected set; }
        [field: SerializeField] public string Comment { get; protected set; }
        [field: SerializeField] public string TargetStat { get; protected set; }
        [field: SerializeField] public string TargetQuantity { get; protected set; }
    }
}
