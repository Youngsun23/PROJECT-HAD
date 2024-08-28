using System;
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
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            if (string.IsNullOrEmpty(currentLevelName))
            {
                LoadLevel("GrayRoomMix_ZagreusRoom");
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Start()
        {
            MonsterBase.OnSpawnedMonsterCountChanged += OnSpawnedMonsterCountChanged;
        }

        private void OnSpawnedMonsterCountChanged(int count)
        {
            if (count <= 0) // 52 잠깐!! 스폰 시스템 아직 없으니까 이게 전투 종료라고 가정~
            {
                // To do :  상호작용이 가능한 보상 오브젝트를 생성

            }
        }

        public void LoadLevel(string levelName)
        {
            StartCoroutine(LoadLevelAsync(levelName));
        }

        IEnumerator LoadLevelAsync(string levelName)
        {
            // To do : Unload Current Level Scene
            if (!string.IsNullOrEmpty(currentLevelName))
            {
                var unloadAsync = SceneManager.UnloadSceneAsync(currentLevelName);
                while (!unloadAsync.isDone)
                {
                    yield return null;
                }
                currentLevelName = string.Empty;
            }

            // To do : Load Level Scene (Additive)
            currentLevelName = levelName;
            var loadAsync = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            while (!loadAsync.isDone)
            {
                yield return null;
            }

            // NavMesh를 최신 맵으로 한번 다시 굽기
            NavMeshSurfaceController.Instance.BakeNavMesh();

            // ToDo: 임시 코드라서 FindObjectOfType 사용 -> 대체할 것
            // Player Character를 Entry Point로 이동
            var entryPoint = GameObject.FindObjectOfType<LevelEntryPoint>();
            CharacterController.Instance.transform.position = entryPoint.transform.position;
        }
        // ToDo: 원래는 맵 로드하는 동안 캐릭터가 낙하하지 않도록 안 움직이게 처리해줘야 함
    }
}