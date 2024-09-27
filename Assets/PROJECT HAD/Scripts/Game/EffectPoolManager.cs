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
        // 이펙트 풀들의 딕셔너리
        private Dictionary<string, IObjectPool<Effect>> effectPoolDic = new Dictionary<string, IObjectPool<Effect>>();
        // 풀링 할 이펙트 리스트
        [SerializeField] private List<EffectInfo> effectInfoList;
        // key-prefab 찾기 위한 딕셔너리...
        // private Dictionary<string, GameObject> effectPrefabDic = new Dictionary<string, GameObject>();

        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < effectInfoList.Count; i++)
            {
                IObjectPool<Effect> effectPool = null;
                var currentEffectInfo = effectInfoList[i];
                // 바로 effectInfoList[i] 사용하면 Get 시점에 IndexOutOfRange 발생
                // new, CreateEffect, Get 등의 호출 시점과 인자 값 확정 시점 혼란
                effectPool = new ObjectPool<Effect>(() => CreateEffect(currentEffectInfo.prefab, effectPool), OnGetEffect, OnReleaseEffect, OnDestroyEffect, true, currentEffectInfo.capacity, currentEffectInfo.maxSize);

                effectPoolDic.Add(currentEffectInfo.key, effectPool);
                // effectPrefabDic.Add(effectInfoList[i].key, effectInfoList[i].prefab);

                for (int j = 0; j < currentEffectInfo.capacity; j++)
                {
                    // 해당되는 prefab 옵젝 만들어서 자기 풀에 등록
                    Effect effect = CreateEffect(currentEffectInfo.prefab, effectPool);
                    //Effect effect = Instantiate(effectInfoList[i].prefab).GetComponent<Effect>();
                    //effect.SetEffectPool(effectPool);
                    effectPool.Release(effect);
                }
            }
        }

        // 여러 풀을 관리하려니 헷갈리네...
        private Effect CreateEffect(GameObject prefab, IObjectPool<Effect> pool)
        {
            //Effect effect = Instantiate(effectPrefabDic[effectKey]).GetComponent<Effect>();
            Effect effect = Instantiate(prefab).GetComponent<Effect>();
            //effect.SetEffectPool(effectPoolDic[effectKey]);
            //OnReleaseEffect(effect);
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
