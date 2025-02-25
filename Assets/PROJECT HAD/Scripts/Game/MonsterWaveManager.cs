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
            yield return new WaitForSeconds(delay);

            Instantiate(prefab, position, rotation);
        }

        public void StartNextWave()
        {
            if (currentWaveIndex < waveList.Count)
            {
                for (int i = 0; i < waveList.Count; i++)
                {
                    if(waveList[i].WaveNum == currentWaveIndex)
                    {
                        waveList[i].StartWave();
                        currentWaveIndex++;
                    }
                }
            }
            else
            {
                var reward = rewardItems.FindIndex(x => x.name == "HAD.Relic.Darkness");
                rewardItems[reward].tag = "RoomReward";
                StartCoroutine(DelayedInstantiate(rewardItems[reward], restoredMonsterBase.transform.position + Vector3.up, Quaternion.Euler(0, 0, 0), 1.0f));
                waveList.Clear();
                currentWaveIndex = 0;
            }
        }

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
            Instantiate(monsterDictionary[monsterName], pos, Quaternion.Euler(0, 270, 0));
        }
    }
}
