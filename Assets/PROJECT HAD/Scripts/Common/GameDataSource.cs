using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class GameDataSource { }

    [System.Serializable]
    public class CharacterSampleData : GameDataSource
    {
        [System.Serializable]
        public class CharacterSampleDataEntity
        {
            public string Key;
            public int Level;
            public float HP;
            public float Damage;
        }

        public List<CharacterSampleDataEntity> DataGroup = new List<CharacterSampleDataEntity>();
    }
}