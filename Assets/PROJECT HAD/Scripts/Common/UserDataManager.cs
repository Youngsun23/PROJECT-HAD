using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    // DTO 클래스들
    [System.Serializable]
    public class  UserDataDTO
    {
        public int id; // 데이터 슬롯 (1~)
        
        public Vector3 lastPlayerPosition = Vector3.zero;
        public string lastSceneName = string.Empty;
    }

    #region 포켓몬 도감 예시
    //[System.Serializable]
    //public class BookDataDTO
    //{
    //[System.Serializable]
    //    public class BookData
    //    {
    //        public int id;
    //        public float progress;
    //        public int count; // 몇 마리 잡았고
    //    }

    //    public List<BookData> DataList;
    //}
    #endregion

    public partial class UserDataManager : SingletonBase<UserDataManager>
    {
        #region 포켓몬 도감 예시
        //public BookDataDTO BookData = new BookDataDTO();

        //public void AddBookData(int id, float progress, int count)
        //{
        //    if(BookData.DataList.Exists(x => x.id == id))
        //    {
        //        var existDataIndex = BookData.DataList.FindIndex(x => x.id == id);
        //        BookData.DataList[existDataIndex].progress += progress;
        //        BookData.DataList[existDataIndex].progress += count;
        //    }
        //    else
        //    {

        //    }
        //}
        #endregion

        public UserDataDTO UserData = new UserDataDTO();
    }
}
