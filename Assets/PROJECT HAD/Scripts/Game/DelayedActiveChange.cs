using System.Collections;
using UnityEngine;

// EffectManager 때의 스크립트
// 전체 주석 ok

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
