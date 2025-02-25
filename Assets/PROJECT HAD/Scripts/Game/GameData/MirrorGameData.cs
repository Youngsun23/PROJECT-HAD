using UnityEngine;

namespace HAD
{
    [CreateAssetMenu(fileName = "Mirror GameData", menuName = "HAD/Mirror Data")]
    public class MirrorGameData : GameDataBase
    {
        [field: SerializeField] public string Name { get; protected set; }
        [field: SerializeField] public int Num { get; protected set; }
        [field: SerializeField] public Sprite Icon { get; protected set; }
        [field: SerializeField] public string Comment { get; protected set; }
        [field: SerializeField] public SerializableDictionary<int, int> PriceAtLevel { get; protected set; } // Darkness
        [field: SerializeField] public SerializableDictionary<int, int> IncreamentAtLevel { get; protected set; } // Darkness
        [field: SerializeField] public string TargetStat { get; protected set; }
        [field: SerializeField] public int MaxLevel { get; protected set; }
    }
}
