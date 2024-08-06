using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class InteractionSensor : MonoBehaviour
    {
        public float InteractionRadius
        { get => interactionRange;
          set
            { interactionRange = value;
              sensorCollider.radius = interactionRange;
            }
        }

        private float interactionRange = 2f;
        private SphereCollider sensorCollider;

        public System.Action<IInteractable> OnDetectedInteractable;
        public System.Action<IInteractable> OnLostInteractable;

        private void Awake()
        {
            sensorCollider = GetComponent<SphereCollider>();
            sensorCollider.isTrigger = true;
            sensorCollider.radius = interactionRange;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.root.TryGetComponent(out IInteractable interactable))
            {
                OnDetectedInteractable?.Invoke(interactable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.TryGetComponent(out IInteractable interactable))
            {
                OnLostInteractable?.Invoke(interactable);
            }
        }
    }
}
