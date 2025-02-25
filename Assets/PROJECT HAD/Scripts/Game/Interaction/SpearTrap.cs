using System.Collections;
using UnityEngine;

namespace HAD
{
    public class SpearTrap : MonoBehaviour
    {
        [SerializeField] private GameObject spear;
        private float durationTime = 1f;
        private float elapsedTime;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(ActivateTrap());
            }
        }

        IEnumerator ActivateTrap()
        {
            elapsedTime = 0f;
            Vector3 StartVec = spear.transform.position;
            while(elapsedTime <= durationTime)
            {
                elapsedTime += Time.deltaTime;
                float newY = Mathf.Lerp(0f, 2f, elapsedTime / durationTime);
                spear.transform.position = new Vector3(StartVec.x, StartVec.y + newY, StartVec.z);
                yield return null;
            }            
            elapsedTime = 0f;
            Vector3 EndVec = spear.transform.position;
            while (elapsedTime <= durationTime)
            {
                elapsedTime += Time.deltaTime;
                float newY = Mathf.Lerp(0f, 2f, elapsedTime / durationTime);
                spear.transform.position = new Vector3(EndVec.x, EndVec.y - newY, EndVec.z);
                yield return null;
            }
            spear.transform.position = StartVec;
        }
    }
}
