using UnityEngine;

namespace HAD
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField] private string monsterName;
        [SerializeField] private GameObject spawnFX;

        private void Awake()
        {
            
        }

        public void SpawnMonster()
        {
            Instantiate(spawnFX, transform.position + Vector3.up, Quaternion.identity);
            MonsterWaveManager.Instance.MonsterSpawn(monsterName, transform.position);
        }
    }
}
