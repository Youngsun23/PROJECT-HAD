using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class MagicArrowController : MonoBehaviour, IActor
    {
        public float arrowSpeed = 8f;

        // 생성 지점 ~ 캐릭터 전방으로 일정 거리 이동
        private void Update()
        {
            transform.Translate(-transform.forward * arrowSpeed * Time.deltaTime);
        }

        // collision 발생 시 (대미지 입히고) 소멸
        // 충돌 대상이 IDamage 갖고 있던 경우 -> 그 쪽이 화살 들고 있다가 죽으면 떨어뜨리게 해야 함
        private void OnCollisionEnter(Collision other)
        {
            var damageInterface = other.transform.root.GetComponent<IDamage>();
            if (damageInterface != null)
            {
                damageInterface.TakeDamage(this, 10f);
                Debug.Log($"마법에 피격!");
            }
            Destroy(gameObject);
        }
        // ToDo: 물리 콜리전 -> 콜라이더를 더 크게 하거나, overlapsphere로 바꾸거나 -> 판정 범위 넓혀야 함

        public GameObject GetActor()
        {
            return gameObject;
        }
    }
}
