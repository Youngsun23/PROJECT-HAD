using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField] private string monsterName;

        public void SpawnMonster()
        {
            MonsterWaveManager.Instance.MonsterSpawn(monsterName, this.transform.position);
        }
    }
}
