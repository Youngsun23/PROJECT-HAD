using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
                Debug.Log($"현재 Index: {currentWaveIndex} -> 다음 웨이브");
                waveList[currentWaveIndex].StartWave();
                currentWaveIndex++;
            }
            else
            {
                Debug.Log("모든 웨이브 종료 -> 보상 생성");
                var reward = rewardItems.FindIndex(x => x.name == "HAD.Relic.Darkness");
                rewardItems[reward].tag = "RoomReward";
                StartCoroutine(DelayedInstantiate(rewardItems[reward], restoredMonsterBase.transform.position + Vector3.up, Quaternion.Euler(0, 0, 0), 1.0f));
                waveList.Clear();
                currentWaveIndex = 0;
            }
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
            Debug.Log("찐 몬스터 스폰");
            Instantiate(monsterDictionary[monsterName], pos, Quaternion.identity);
        }
    }
}
