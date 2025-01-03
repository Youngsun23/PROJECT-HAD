using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class MonsterWave : MonoBehaviour
    {
        [SerializeField] private List<MonsterSpawner> spawnPoints = new List<MonsterSpawner>();

        // 인스펙터에서 MonsterWave에 스크립트 만들고 직접 연결해놓고, 거기서 List 순서대로 한 번에 MonsterWaveManager에 AddWave 해주는 방법도 있음
        [SerializeField] private int waveNum;
        public int WaveNum => waveNum;

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

