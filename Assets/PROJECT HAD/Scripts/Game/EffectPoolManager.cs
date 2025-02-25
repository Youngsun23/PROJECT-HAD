using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace HAD
{
    [System.Serializable]
    public class EffectInfo
    {
        public string key;
        public int capacity;
        public int maxSize;
        public GameObject prefab;
    }

    public class EffectPoolManager : SingletonBase<EffectPoolManager>
    {
        private Dictionary<string, IObjectPool<Effect>> effectPoolDic = new Dictionary<string, IObjectPool<Effect>>();
        [SerializeField] private List<EffectInfo> effectInfoList;

        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < effectInfoList.Count; i++)
            {
                IObjectPool<Effect> effectPool = null;
                var currentEffectInfo = effectInfoList[i];
                effectPool = new ObjectPool<Effect>(() => CreateEffect(currentEffectInfo.prefab, effectPool), OnGetEffect, OnReleaseEffect, OnDestroyEffect, true, currentEffectInfo.capacity, currentEffectInfo.maxSize);

                effectPoolDic.Add(currentEffectInfo.key, effectPool);

                for (int j = 0; j < currentEffectInfo.capacity; j++)
                {
                    Effect effect = CreateEffect(currentEffectInfo.prefab, effectPool);
                    effectPool.Release(effect);
                }
            }
        }

        private Effect CreateEffect(GameObject prefab, IObjectPool<Effect> pool)
        {
            Effect effect = Instantiate(prefab).GetComponent<Effect>();
            effect.SetEffectPool(pool);
            return effect;
        }

        private void OnGetEffect(Effect effect)
        {
            effect.gameObject.SetActive(true);
        }

        private void OnReleaseEffect(Effect effect)
        {
            effect.gameObject.SetActive(false);
        }

        private void OnDestroyEffect(Effect effect)
        {
            Destroy(effect.gameObject);
        }

        public Effect GetEffect(string key)
        {
            if (effectPoolDic.TryGetValue(key, out var pool))
            {
                return pool.Get();
            }
            else
            {
                Debug.Log($"Effect key {key}는 존재하지 않음");
                return null;
            }
        }
    }
}
