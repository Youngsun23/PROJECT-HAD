using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace HAD
{
    public class Effect : MonoBehaviour
    {
        private IObjectPool<Effect> effectPool;
        public float delayTime = 1.5f;

        public void OnEnable()
        {
            StartCoroutine(DelayedInActive());
        }

        IEnumerator DelayedInActive()
        {
            yield return new WaitForSeconds(delayTime);
            effectPool.Release(this);
        }

        public void SetEffectPool(IObjectPool<Effect> pool)
        {
            effectPool = pool;
        }
    }
}
