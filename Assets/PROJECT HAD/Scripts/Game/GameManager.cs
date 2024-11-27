using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HAD
{
    public class GameManager : MonoBehaviour
    {
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
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(currentLevelName))
            {
                // LoadLevel("GrayRoomMix_ZagreusRoom");
                LoadLevel("Entrance");
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

        public void LoadLevel(string levelName)
        {
            StartCoroutine(LoadLevelAsync(levelName));
        }

        IEnumerator LoadLevelAsync(string levelName)
        {
            // 씬 로드에는 문제 X
            // CharacterController.Instance가 null인 상태 - Awake 순서 탓? // 얘를 Start로
            // 캐릭터 비활성화 -> 애니메이션 고정 등 문제
            if(CharacterController.Instance != null)
            {
                CharacterController.Instance.enabled = false;
                CharacterController.Instance.character.Move(Vector2.zero, Camera.main.transform.eulerAngles.y);
                // Debug.Log(CharacterController.Instance.gameObject.name);
            }

            var FadeUI = UIManager.Singleton.GetUI<FadeInOutUI>(UIList.FadeInOut);
            if(!string.IsNullOrEmpty(currentLevelName))
            {
                FadeUI.FadeOut();
                yield return new WaitForSeconds(1);

                var unloadAsync = SceneManager.UnloadSceneAsync(currentLevelName);
                while (!unloadAsync.isDone)
                {
                    yield return null;
                }
                currentLevelName = string.Empty;
            }

            gates.Clear();

            currentLevelName = levelName;
            var loadAsync = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            while (!loadAsync.isDone)
            {
                yield return null;
            }

            // NavMesh를 최신 맵으로 한번 다시 굽기
            NavMeshSurfaceController.Instance.BakeNavMesh();

            // ToDo: FindObjectOfType 사용 -> entry 스크립트에 string 변수(키값) 두고, gate처럼 awake에서 자신 등록?
            // Player Character를 Entry Point로 이동
            var entryPoint = GameObject.FindObjectOfType<LevelEntryPoint>();
            CharacterController.Instance.character.Move(Vector2.zero, Camera.main.transform.eulerAngles.y);
            CharacterController.Instance.transform.position = entryPoint.transform.position;

            // 캐릭터 활성화
            CharacterController.Instance.enabled = true;
            FadeUI.FadeIn();
            yield return new WaitForSeconds(1);

            // 전투 씬인 경우만...
            // MonsterWaveManager.Instance.StartNextWave(null);


            // 현재 씬의 이름대로 출력
            var areaUI = UIManager.Singleton.GetUI<AreaAnnouncerUI>(UIList.AreaAnnouncer);
            areaUI.ShowAreaAnnouncerUI();
              
            //if (currentLevelName == "GrayRoomMix_ZagreusRoom")
            //{
            //    // AreaAnnouncerUI.Instance.ShowAreaAnnouncerUI("Zagreus's Room");
            //    var areaUI = UIManager.Singleton.GetUI<AreaAnnouncerUI>(UIList.AreaAnnouncer);
            //    areaUI.ShowAreaAnnouncerUI("Zagreus's Room");
            //}

            // 전투 없이 방 활성화되는 경우들
            if(currentLevelName == "Entrance")
            {
                foreach(Gate gate in gates)
                {
                    gate.ActivateGate();
                }
            }

        }
        // ToDo: 원래는 맵 로드하는 동안 캐릭터가 낙하하지 않도록 안 움직이게 처리해줘야 함
    }
}