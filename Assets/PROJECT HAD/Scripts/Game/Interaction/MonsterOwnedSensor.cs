using UnityEngine;

namespace HAD
{
    public class MonsterOwnedSensor : MonoBehaviour
    {
        public float InteractionRadius
        {
            get => interactionRange;
            set
            {
                interactionRange = value;
                sensorCollider.radius = interactionRange;
            }
        }

        private float interactionRange = 8f; // 몬스터들 공통/개별 어떻게 쓸건지
        private SphereCollider sensorCollider;

        public System.Action<PlayerCharacter> OnDetectedTarget;
        public System.Action<PlayerCharacter> OnLostTarget;

        private void Awake()
        {
            sensorCollider = GetComponent<SphereCollider>();
            sensorCollider.isTrigger = true;
            sensorCollider.radius = interactionRange;
        }

        // Sensor Layer끼리는 물리충돌 x
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.TryGetComponent(out PlayerCharacter playerCharacter))
            {
                OnDetectedTarget?.Invoke(playerCharacter);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.TryGetComponent(out PlayerCharacter playerCharacter))
            {
                OnLostTarget?.Invoke(playerCharacter);
            }
        }
    }
}

