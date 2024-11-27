using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class Dropitem_Arrow : MonoBehaviour, IInteractable
    {
        public bool IsAutomaticInteraction => true;

        string IInteractable.Message => throw new System.NotImplementedException();

        private Transform target;
        private Rigidbody dropItemRigidbody;
        private Collider dropItemCollider;

        private bool isTracking = false;

        void Awake()
        {
            dropItemRigidbody = GetComponent<Rigidbody>();
            dropItemCollider = GetComponent<Collider>();
        }

        void Update()
        {
            if (isTracking)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 10f);

                float distance = Vector3.Distance(transform.position, target.position);
                if(distance <= 0.1f)
                {
                    isTracking = false;

                    // ToDo: PC currentArrow 증가 // 맞나?
                    float currentArrowCount = target.gameObject.GetComponent<CharacterAttributeComponent>().GetAttribute(AttributeTypes.MagicArrowCount).CurrentValue;
                    target.gameObject.GetComponent<CharacterAttributeComponent>().SetAttributeBuffedValue(AttributeTypes.MagicArrowCount, currentArrowCount + 1);

                    Destroy(gameObject);
                }
            }
        }

        public void Interact(CharacterBase actor)
        {
            // 아이템 획득 연출
            dropItemRigidbody.isKinematic = true;
            dropItemCollider.isTrigger = true;

            target = actor.transform;
            isTracking = true;
        }

        public void InteractEnable() { }
        public void InteractDisenable() { }
    }
}
