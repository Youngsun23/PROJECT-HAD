using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class ProjectileController : MonoBehaviour, IActor
    {
        public float arrowSpeed = 8f;
        private Vector3 moveDir;

        private void Awake()
        {
            moveDir = -transform.forward;
        }

        // 생성 지점 ~ 캐릭터 전방으로 일정 거리 이동
        private void Update()
        {
            transform.Translate(moveDir * arrowSpeed * Time.deltaTime);
        }

        // ToDo: OnCollisionEnter에서 처리할 것
        // PlayerProjectile과 MonProjectile 레이어 충돌 설정으로 Mon/PC/그 외 처리 다르게
        // 단, this.gameObject의 layer가 PlayerProjectile일 경우
        // -> 충돌체가 몬스터일 경우 해당 몬스터가 마법화살 소유하도록
        // -> 충돌체가 몬스터가 아니거나 최대거리까지 날아간 경우 바닥에 떨어지도록
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

        public GameObject GetActor()
        {
            return gameObject;
        }

        public void ReverseMoveDir()
        {
            moveDir = -moveDir;
        }

        public void SetLayer(int layer)
        {
            gameObject.layer = layer;
        }
    }
}
