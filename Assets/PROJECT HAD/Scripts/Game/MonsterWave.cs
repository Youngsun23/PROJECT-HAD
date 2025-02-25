using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class MonsterWave : MonoBehaviour
    {
        [SerializeField] private List<MonsterSpawner> spawnPoints = new List<MonsterSpawner>();

        [SerializeField] private int waveNum;
        public int WaveNum => waveNum;

        private void Awake()
        {
            MonsterWaveManager.Instance.AddWave(this);

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

