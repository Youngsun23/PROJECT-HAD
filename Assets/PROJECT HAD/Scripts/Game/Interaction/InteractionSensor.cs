using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        //private IInteractable interactableObject;
        //private bool interactionInformed = false;
        //public bool InteractionInformed => interactionInformed;
        //public void SetInteractionInformed(bool tf) {  interactionInformed = tf; }
        //[SerializeField] private InteractionUI interactionUI;

        private void Awake()
        {
            sensorCollider = GetComponent<SphereCollider>();
            sensorCollider.isTrigger = true;
            sensorCollider.radius = interactionRange;
        }

        #region Try
        //private void Update()
        //{
        //if(interactionInformed)
        //{
        //    if (!interactionUI.isDisplayed)
        //        interactionUI.Open();
        //    if(Keyboard.current.eKey.wasPressedThisFrame)
        //        interactableObject.Interact(this.gameObject.GetComponent<CharacterBase>());
        //}
        //}
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.root.TryGetComponent(out IInteractable interactable))
            {
                // interactableObject = interactable;
                OnDetectedInteractable?.Invoke(interactable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.root.TryGetComponent(out IInteractable interactable))
            {
                // interactableObject = null;
                OnLostInteractable?.Invoke(interactable);
            }
        }
    }
}
