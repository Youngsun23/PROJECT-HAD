using Unity.AI.Navigation;
using UnityEngine;

namespace HAD
{
    public class NavMeshSurfaceController : MonoBehaviour
    {
        public static NavMeshSurfaceController Instance { get; private set; }

        [SerializeField] private NavMeshSurface navMeshSurface;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            navMeshSurface = GetComponent<NavMeshSurface>();
        }

        private void OnDestroy()
        {
            if(Instance == this)
            {
                Instance = null;
            }
        }

        public void BakeNavMesh()
        {
            navMeshSurface.BuildNavMesh();
        }
    }
}
