using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class GameDataModel : SingletonBase<GameDataModel>
    {
        public List<CharacterGameData> characterData = new List<CharacterGameData>();

        public List<MirrorGameData> mirrorGameData;

        public CharacterGameData GetPlayerCharacterGameData(string option)
        {
            return characterData.Find(x => x.Option == option);  
        }

        public MirrorGameData GetMirrorGameData(int num)
        {
            return mirrorGameData.Find(data => data.Num == num);
        }
    }
}
