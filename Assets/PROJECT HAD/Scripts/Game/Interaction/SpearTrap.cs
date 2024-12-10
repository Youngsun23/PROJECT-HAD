using System.Collections;
using UnityEngine;

namespace HAD
{
    public class SpearTrap : MonoBehaviour
    {
        [SerializeField] private GameObject spear;
        private float durationTime = 1.5f;
        private float elapsedTime = 0f;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(ActivateTrap());
            }
        }

        IEnumerator ActivateTrap()
        {
            Vector3 originVec = spear.transform.position;
            while(elapsedTime <= durationTime)
            {
                elapsedTime += Time.deltaTime;
                float newY = Mathf.Lerp(0f, 2f, elapsedTime / durationTime);
                spear.transform.position = new Vector3(originVec.x, originVec.y + newY, originVec.z);
                yield return null;
            }            
            elapsedTime = 0f;
            while(elapsedTime <= durationTime)
            {
                elapsedTime += Time.deltaTime;
                float newY = Mathf.Lerp(0f, 2f, elapsedTime / durationTime);
                spear.transform.position = new Vector3(originVec.x, originVec.y - newY, originVec.z);
                yield return null;
            }
            spear.transform.position = originVec;
        }
    }
}
