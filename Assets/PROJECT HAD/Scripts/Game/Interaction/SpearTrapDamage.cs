using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class SpearTrapDamage : MonoBehaviour, IActor
    {
        private void OnTriggerEnter(Collider other)
        {
            var damageInterface = other.transform.root.GetComponent<IDamage>();
            if (damageInterface != null && other.gameObject.CompareTag("Player"))
            {
                damageInterface.TakeDamage(this, 5f);
                Debug.Log($"트랩에 피격!");
            }
        }

        public GameObject GetActor()
        {
            return gameObject;
        }
    }
}
