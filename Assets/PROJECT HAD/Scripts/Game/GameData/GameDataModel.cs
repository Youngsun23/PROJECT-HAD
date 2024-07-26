using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class GameDataModel : SingletonBase<GameDataModel>
    {
        public List<CharacterGameData> characterData = new List<CharacterGameData>();

        public CharacterGameData GetPlayerCharacterData(int level)
        {
            return characterData.Find(x => x.Level == level);  
        }
    }
}
