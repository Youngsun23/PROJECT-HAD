using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class DelayedActiveChange : MonoBehaviour
    {
        public float delayTime = 3f;
        public void Start()
        {
            StartCoroutine(DelayedInActive());
        }

        IEnumerator DelayedInActive()
        {
            yield return new WaitForSeconds(delayTime);
            gameObject.SetActive(false);
        }
    }
}
