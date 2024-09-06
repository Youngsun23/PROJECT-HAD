using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HAD
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> rewardItems = new List<GameObject>();
        
        private List<Gate> gates = new List<Gate>();
        public void AddGate(Gate newGate)
        {
            gates.Add(newGate);
        }
        public List<Gate> GetGateList()
        {
            return gates;
        }

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
                // LoadLevel("GrayRoomMix_Backyard");
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        #region OnSpawnedMonsterCountChanged : if(count <= 0) 외에도 로직 추가되면 살리기
        //private void Start()
        //{
        //    MonsterBase.OnSpawnedMonsterCountChanged += OnSpawnedMonsterCountChanged;
        //}

        //private void OnDisable()
        //{
        //    MonsterBase.OnSpawnedMonsterCountChanged -= OnSpawnedMonsterCountChanged;
        //}

        //private void OnSpawnedMonsterCountChanged(int count, Vector3 monPos)
        //{
        //    if (count <= 0) // 52 잠깐!! 스폰 시스템 아직 없으니까 이게 전투 종료라고 가정~
        //    {
        //        // To do :  방의 종류에 맞는 보상 아이템 생성
        //        // 마지막으로 죽은 몬스터의 위치에서 - 매번 Vector3 받아오기?
        //        Debug.Log("보상 생성");
        //        var reward = rewardItems.FindIndex(x => x.name == "HAD.Relic.Darkness");
        //        rewardItems[reward].tag = "RoomReward";
        //        // Instantiate(rewardItems[reward], monPos, Quaternion.Euler(0, 0, 0));
        //        StartCoroutine(DelayedInstantiate(rewardItems[reward], monPos, Quaternion.Euler(0, 0, 0), 1.0f));
        //        // Error: Some objects were not cleaned up when closing the scene. 
        //        // OnDestroy에서 Instantiate시키지는 않았는데 습,,,
        //        // coroutine으로 바꾸고 오류 사라진 대신 다른 오류 생김
        //        // -> 게임 실행 종료 시
        //        // MissingReferenceException: The object of type 'GameManager' has been destroyed but you are still trying to access it.
        //        // InGame에 있는 애가 왜 파괴됐다는거지?
        //    }
        //}
        #endregion

        public void NotifyLastMonsterDead(MonsterBase monsterBase)
        {
            Debug.Log("보상 생성");
            var reward = rewardItems.FindIndex(x => x.name == "HAD.Relic.Darkness");
            rewardItems[reward].tag = "RoomReward";
            StartCoroutine(DelayedInstantiate(rewardItems[reward], monsterBase.transform.position, Quaternion.Euler(0, 0, 0), 1.0f));
        }

        IEnumerator DelayedInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, float delay)
        {
            // 지정된 시간 동안 대기
            yield return new WaitForSeconds(delay);

            // 딜레이가 끝난 후 오브젝트 생성
            Instantiate(prefab, position, rotation);
        }

        public void LoadLevel(string levelName)
        {
            StartCoroutine(LoadLevelAsync(levelName));
        }

        IEnumerator LoadLevelAsync(string levelName)
        {
            FadeInOutUI.Instance.FadeIn();
            yield return new WaitForSeconds(1);

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

            gates.Clear();

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

            FadeInOutUI.Instance.FadeOut();
            yield return new WaitForSeconds(1);
        }
        // ToDo: 원래는 맵 로드하는 동안 캐릭터가 낙하하지 않도록 안 움직이게 처리해줘야 함
    }
}