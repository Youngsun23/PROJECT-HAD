using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    [System.Serializable]
    public class  EffectPool
    {
        public string key;
        public int capacity = 10;
        public int maxSize = 100;
        public GameObject prefab;
        public Queue<GameObject> pool = new Queue<GameObject>(); 
    }

    public class EffectManager : SingletonBase<EffectManager>
    {
        public List<EffectPool> container = new List<EffectPool>();

        protected override void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            // ToDO: 이펙트 풀 초기화
            foreach(var item in container)
            {
                for(int i = 0; i < item.capacity; i++)
                {
                    var newEffect = Instantiate(item.prefab);
                    newEffect.gameObject.SetActive(false);

                    item.pool.Enqueue(newEffect);
                }
            }
        }

        public GameObject GetEffect(string key)
        {
            var targetPool = container.Find(x => x.key.Equals(key));
            var newEffect = targetPool.pool.Dequeue();

            // Pool이 부족한 상황
            if (newEffect.activeSelf)
            {
                targetPool.pool.Enqueue(newEffect);
                IncreaseSizeUp(key);
                // ToDo: 새로운 이펙트를 다시 가져온다.
                while(newEffect.activeSelf)
                {
                    newEffect = targetPool.pool.Dequeue();
                    if(newEffect.activeSelf)
                    {
                        targetPool.pool.Enqueue(newEffect);
                    }
                }
            }

            newEffect.gameObject.SetActive(true);
            targetPool.pool.Enqueue(newEffect);

            return newEffect;
        }

        // 사양 튈 수 있음
        public void IncreaseSizeUp(string key)
        {
            var targetPool = container.Find(x => x.key.Equals(key));
            int afterCapacity = Mathf.Clamp(targetPool.capacity * 2, targetPool.capacity, targetPool.maxSize);
            for(int i = 0; i < targetPool.capacity; i++)
            {
                var newElement = Instantiate(targetPool.prefab);
                newElement.gameObject.SetActive(false);
                targetPool.pool.Enqueue(newElement);
            }
        }
    }
}
