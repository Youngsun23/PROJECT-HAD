using UnityEngine;
// Image?
using UnityEngine.UI;

namespace HAD
{
    [CreateAssetMenu(fileName = "Charon GameData", menuName = "HAD/Charon Data")]
    public class CharonGameData : GameDataBase
    {
        [field: SerializeField] public string Name { get; protected set; }
        [field: SerializeField] public Image Icon { get; protected set; }
        [field: SerializeField] public string Comment { get; protected set; }
        [field: SerializeField] public string Price { get; protected set; } // Coin
        [field: SerializeField] public string Duration { get; protected set; }
        [field: SerializeField] public string TargetStat { get; protected set; }
        [field: SerializeField] public string TargetQuantity { get; protected set; }
    }
}
