using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HAD
{
    public class MonsterWave : MonoBehaviour
    {
        [SerializeField] private List<MonsterSpawner> spawnPoints = new List<MonsterSpawner>();

        // ToDo
        public int waveNum;

        private void Awake()
        {
            // 자동 등록 -> Wave1,2중 누가 먼저 될지 모름 / 이전 작동은 우연인가?
            // Awake들 순서 보장 문제 반복 - 어떻게 해결?
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

