using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HAD
{
    public class MonsterWave : MonoBehaviour
    {
        [SerializeField] private List<MonsterSpawner> spawnPoints = new List<MonsterSpawner>();

        private void Awake()
        {
            MonsterWaveManager.Instance.AddWave(this);

            // 습...
            if (MonsterWaveManager.Instance.GetCurentWaveIndex() <= 0)
            {
                MonsterWaveManager.Instance.StartNextWave();
            }
        }

        public void StartWave()
        {
            foreach (var spawnPoint in spawnPoints)
            {
                spawnPoint.SpawnMonster();
            }
        }
    }
}

