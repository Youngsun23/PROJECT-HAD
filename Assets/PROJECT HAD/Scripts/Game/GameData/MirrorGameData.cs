using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HAD
{
    [CreateAssetMenu(fileName = "Mirror GameData", menuName = "HAD/Mirror Data")]
    public class MirrorGameData : GameDataBase
    {
        [field: SerializeField] public string Name { get; protected set; }
        [field: SerializeField] public Image Icon { get; protected set; }
        [field: SerializeField] public string Comment { get; protected set; }
        [field: SerializeField] public string Price { get; protected set; } // Darkness
        [field: SerializeField] public string TargetStat { get; protected set; }
        [field: SerializeField] public string TargetQuantity { get; protected set; }
    }
}
