using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class FXSelfDestroy : MonoBehaviour
    {
        public float durationTime = 0.5f;

        private void Awake()
        {
            StartCoroutine(SelfDestroy());
        }

        IEnumerator SelfDestroy()
        {
            yield return new WaitForSeconds(durationTime);
            Destroy(gameObject);
        }
    }
}
