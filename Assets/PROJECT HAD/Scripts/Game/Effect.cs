using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace HAD
{
    public class Effect : MonoBehaviour
    {
        private IObjectPool<Effect> effectPool;
        public float delayTime = 1.5f;

        public void OnEnable() // Start를 쓰니까 호출이 안되지 짜식아;;
        {
            StartCoroutine(DelayedInActive());
        }

        IEnumerator DelayedInActive()
        {
            yield return new WaitForSeconds(delayTime);
            // Debug.Log("Release 호출");
            effectPool.Release(this);
        }

        public void SetEffectPool(IObjectPool<Effect> pool)
        {
            // Debug.Log($"SetEffectPool 호출: {pool.GetHashCode()}");
            effectPool = pool;
        }
    }
}
