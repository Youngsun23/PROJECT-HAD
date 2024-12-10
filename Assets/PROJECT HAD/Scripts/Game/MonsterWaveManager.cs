using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class MonsterWaveManager : MonoBehaviour
    {
        public static MonsterWaveManager Instance { get; private set; }
        [SerializeField] private SerializableDictionary<string, GameObject> monsterDictionary = new SerializableDictionary<string, GameObject>();
        [SerializeField] private List<MonsterWave> waveList = new List<MonsterWave>();
        private int currentWaveIndex = 0;
        [SerializeField] private List<GameObject> rewardItems = new List<GameObject>();
        private MonsterBase restoredMonsterBase;

        public int GetCurentWaveIndex()
        {
            return currentWaveIndex;
        }

        public void NotifyLastMonsterDead(MonsterBase monsterBase)
        {
            restoredMonsterBase = monsterBase;

            StartNextWave();
        }

        IEnumerator DelayedInstantiate(GameObject prefab, Vector3 position, Quaternion rotation, float delay)
        {
            // 지정된 시간 동안 대기
            yield return new WaitForSeconds(delay);

            // 딜레이가 끝난 후 오브젝트 생성
            Instantiate(prefab, position, rotation);
        }

        //// 습...
        //public void StartFirstWave()
        //{
        //    waveList[0].StartWave();
        //    currentWaveIndex++;
        //}

        public void StartNextWave()
        {
            if (currentWaveIndex < waveList.Count)
            {
                // Debug.Log($"현재 Index: {currentWaveIndex} -> 다음 웨이브");
                waveList[currentWaveIndex].StartWave();
                currentWaveIndex++;
            }
            else
            {
                // Debug.Log("모든 웨이브 종료 -> 보상 생성");
                // ToDo: 방의 보상을 리스트 중 하나로 랜덤 결정, 문에도 시각화
                var reward = rewardItems.FindIndex(x => x.name == "HAD.Relic.Darkness");
                rewardItems[reward].tag = "RoomReward";
                StartCoroutine(DelayedInstantiate(rewardItems[reward], restoredMonsterBase.transform.position + Vector3.up, Quaternion.Euler(0, 0, 0), 1.0f));
                waveList.Clear();
                currentWaveIndex = 0;
            }
        }

        // ToDo: ItemManager 스크립트 만들어서 함수 옮기고 호출로 변경
        // 보상 랜덤 선택 - 아이템/은혜 50%로 일단 정하고, 그 중에서 하나로 정하고, 개수도 각각 범위 중 하나로 결정
        private GameObject SelectReward()
        {
            int randomIndex = Random.Range(0, rewardItems.Count);
            GameObject reward = rewardItems[randomIndex];
            return reward;
        }

        public void AddWave(MonsterWave wave)
        {
            waveList.Add(wave);
        }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void MonsterSpawn(string monsterName, Vector3 pos)
        {
            // Debug.Log("찐 몬스터 스폰");
            Instantiate(monsterDictionary[monsterName], pos, Quaternion.Euler(0, 270, 0));
        }
    }
}
