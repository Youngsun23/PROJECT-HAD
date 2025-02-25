using Sirenix.OdinInspector;
using UnityEngine;
using System.IO;

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
            if(FileUtility.ReadFileData("Assets/Resources/UserData.txt", out string loadedUserData))
            {
                UserData = JsonUtility.FromJson<UserDataDTO>(loadedUserData);
            }
        }

        [Button("Delete")]
        public void Delete()
        {
            string filePath = "Assets/Resources/UserData.txt";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("User Data 삭제");
            }
        }
    }
}
