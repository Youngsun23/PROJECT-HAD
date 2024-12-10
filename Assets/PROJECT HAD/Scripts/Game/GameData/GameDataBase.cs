using UnityEngine;

namespace HAD
{
    public abstract class GameDataBase : ScriptableObject
    {
        [field: SerializeField] public string UniqueID { get; set; }
    }
}
