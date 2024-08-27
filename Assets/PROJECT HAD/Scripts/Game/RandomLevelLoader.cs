using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class RandomLevelLoader : MonoBehaviour
    {
        [SerializeField] private List<GameObject> levels = new List<GameObject>();
        [SerializeField] private Vector3 pivot;

        private void Awake()
        {
            LoadRandomLevel(pivot);
        }

        public void LoadRandomLevel(Vector3 pivot)
        {
            int randomIndex = Random.Range(0, levels.Count);
            var randomLevelInstance = Instantiate(levels[randomIndex]);
            randomLevelInstance.transform.position = pivot;
        }
    }
}
