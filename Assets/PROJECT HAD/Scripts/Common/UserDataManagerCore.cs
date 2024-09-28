using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public partial class UserDataManager : SingletonBase<UserDataManager>
    {
        [Button("Save")]

        public void Save()
        {
            string serializeUserData = JsonUtility.ToJson(UserData, true);

            FileUtility.WriteFileFromString("Assets/Resources/UserData.txt", serializeUserData);
        }

        [Button("Load")]
        public void Load()
        {
            if(FileUtility.ReadFileData("Assets/Resources/UserData.txt", out string loadedBookData))
            {
                UserData = JsonUtility.FromJson<UserDataDTO>(loadedBookData);
            }
        }

        public void Delete()
        {

        }
    }
}
