using UnityEngine;

namespace HAD
{
    public class CameraConfinerVolume : MonoBehaviour
    {
        private Collider volumeCollider;

        private void Awake()
        {
            volumeCollider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            //if(other.transform.root.TryGetComponent(out CharacterController playerCharacterController))
            if(other.transform.root.CompareTag("Player"))
            {
                CameraSystem.Instance.SetConfinerVolume(volumeCollider);
            }

        }
    }
}
