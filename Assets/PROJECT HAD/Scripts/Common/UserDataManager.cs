using System.Collections.Generic;

namespace HAD
{
    [System.Serializable]
    public class  UserDataDTO 
    {
        public int id; 
        public string lastSceneName = string.Empty;
        public int centaurHeart = 0; 
        public int vitality = 0; 
        public float lastCurrentHP;
        public Dictionary<int, int> mirrorDic = new Dictionary<int, int>()
        {
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 },
        };
        public int coin = 0;
        public int darkness = 0;
    }

    public partial class UserDataManager : SingletonBase<UserDataManager>
    {
        public UserDataDTO UserData = new UserDataDTO();

        public bool UpdateUserDataMirror(int setNum)
        {
            int updatedLevel = ++UserData.mirrorDic[setNum];
            PlayerCharacter.Instance.InitializeCharacter(GameDataModel.Singleton.GetPlayerCharacterGameData("Default"));
            if(updatedLevel == GameDataModel.Singleton.GetMirrorGameData(setNum).MaxLevel)
            {
                return true;
            }
            return false;
        }

        public void UpdateUserDataResources(string resource, int value)
        {
            if (resource == "Coin")
            {
                UserData.coin += value;
                HUDUI.Instance.UpdateHUDUICoin(value);
            }
            if (resource == "Darkness")
            {
                UserData.darkness += value;
                HUDUI.Instance.UpdateHUDUIDarkness(value);
            }
        }

        public void UpdateUserDataLast(string sceneName, float currentHP)
        {
            UserData.lastSceneName = sceneName;
            UserData.lastCurrentHP = currentHP;
        }

        public void ResetTempUserData()
        {
            UserData.centaurHeart = 0;
            UserData.coin = 0;
        }
    }
}
