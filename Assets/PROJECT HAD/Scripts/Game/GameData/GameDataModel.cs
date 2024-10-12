using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class GameDataModel : SingletonBase<GameDataModel>
    {
        public List<CharacterGameData> characterData = new List<CharacterGameData>();

        public CharacterGameData GetPlayerCharacterData(string option)
        {
            return characterData.Find(x => x.Option == option);  
        }
    }
}
