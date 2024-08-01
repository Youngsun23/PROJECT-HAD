//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace HAD
//{
//    public class CharacterActionController : MonoBehaviour
//    {
//        public List<CharacterActionData> characterActionDatas = new List<CharacterActionData>();

//        public CharacterActionData GetActionData(CharacterActionData pivotData)
//        {
//            if(pivotData == null)
//            {
//                // -> 첫 번째 액션을 달라는 뜻
//                return characterActionDatas[0];
//            }

//            var targetActionData = characterActionDatas.Find(x => x == pivotData.NextAction); // 시퀀스의 다음 액션 받기
//            if(targetActionData == null)
//            {
//                // -> 시퀀스의 다음 액션 X
//                return null;
//            }
//            else
//            {
//                return targetActionData;
//            }
//        }
//    }
//}
