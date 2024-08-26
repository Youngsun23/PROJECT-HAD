using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HAD
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private string currentLevelName;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            if (string.IsNullOrEmpty(currentLevelName))
            {
                LoadLevel("GrayRoomMix 1");
            }
        }

        private void Start()
        {
            MonsterBase.OnSpawnedMonsterCountChanged += OnSpawnedMonsterCountChanged;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void OnSpawnedMonsterCountChanged(int count)
        {
            if(count <= 0) // 52 잠깐!! 스폰 시스템 아직 없으니까 이게 전투 종료라고 가정~
            {
                // To Do: 보상 옵젝(상호작용) 생성

            }
        }

        public void LoadLevel(string levelName)
        {
            StartCoroutine(LoadLevelAsync(levelName));
        }

        IEnumerator LoadLevelAsync(string levelName)
        {
            if (!string.IsNullOrEmpty(currentLevelName))
            {
                var unloadAsync = SceneManager.UnloadSceneAsync(currentLevelName);
                while (!unloadAsync.isDone)
                {
                    yield return null;
                }
                currentLevelName = string.Empty;
            }

            currentLevelName = levelName;
            var loadAsync = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            while (!loadAsync.isDone)
            {
                yield return null;
            }
        }
    }
}