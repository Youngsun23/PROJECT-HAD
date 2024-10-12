using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    // DTO 클래스들
    [System.Serializable]
    public class  UserDataDTO // Load는 게임 시작 - 데이터 슬롯 선택 시에만
    {
        // public으로 바로 접근 가능하게 둬도 되나??
        // 근데 get으로 하자니 각각 get 어떡해야 하는지...
        // 초기값 넣어두기
        public int id; // 데이터 슬롯 (1~)
        
        public Vector3 lastPlayerPosition = Vector3.zero;
        public string lastSceneName = string.Empty;

        // MaxHP - 영구 / 임시() - 직접 수치로 저장하는 게 아니라 '강화 A를 X회' 형태?
        public int centaurHeart = 0; // (임시 Max&Curr +25HP)
        // 임시 강화는 결국 최종값만 +- 하며 사용할 건데 강화 종류마다 정보 저장하고 계산할 필요 있나??
        public int vitality = 0; // (영구 recoveryHP +1~+3) // 거울의 경우 거울 쪽 초기화에도 정보 사용
        
        // CurrentHP
        // Arrow 개수 강화

        // <- 스탯으로 분류 / 강화 계열로 분류 ->

        // 거울 - 영구(name,level)

        // 카론 - 즉발/임시(RoomCount) - Save(방 전환) 시 LeftRoomCount가 0이 된 것들의 효과 제거

        // 획득한 은혜(name,level) - 사망 시 리셋

        // 그 외 아이템 사용? 

        // 자원 보유량(coin,key,darkness) - coin은 사망 시 리셋
        public int coin = 0;
        public int key = 0;
        public int darkness = 0;

        // 플레이 루프 횟수 - 초기값 1, 사망 시 +1
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

        public void UpdateUserDataResources(string resource, int value)
        {
            if (resource == "Coin")
                UserData.coin += value;
            if (resource == "Key")
                UserData.key += value;
            if (resource == "Darkness")
                UserData.darkness += value;

            // userdata에 정보 업데이트 -> component 쪽에 업데이트로 연결해줘야 하나?
            // 유저데이터 getset,,
        }

        public void ResetTempUserData()
        {
            UserData.centaurHeart = 0;
            UserData.coin = 0;
        }
        // ToDO
        // UserData의 값들 업데이트 함수
        // Save -> 전체 값 txt에 저장
        // Load(게임시작) -> UserData에 txt에 저장해둔 값 다시 집어넣음
        // UserData 값대로 캐릭터 세팅 해주는 처리 필요

        // userdata 기반으로 실제 attribute 버프값 총합 계산하는 함수
        public int CalUserDataMaxHP()
        {
            int maxHP = 0;
            maxHP += UserData.centaurHeart * 25;

            return maxHP;
        }
        // CurrentHP가 골치...이건 강화/buff 개념이 아니니까 별개로 관리해주고 싶은데
    }

    
}
