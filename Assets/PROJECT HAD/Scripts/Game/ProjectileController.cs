using UnityEngine;

namespace HAD
{
    public class ProjectileController : MonoBehaviour, IActor
    {
        public float arrowSpeed = 8f;
        public string targetTag; // = "Player" / "Monster"
        private Vector3 moveDir;

        private void Awake()
        {
            moveDir = -transform.forward;
            // ToDo - > + 생성할 때 애초에 캐릭터 정면 쪽이 forward가 되도록 회전해서 소환
        }

        // 생성 지점 ~ 캐릭터 전방으로 일정 거리 이동
        private void Update()
        {
            transform.Translate(moveDir * arrowSpeed * Time.deltaTime);
        }

        // ToDo: OnTriggerEnter에서 처리할 것
        // PlayerProjectile과 MonProjectile 태그 충돌 설정으로 Mon/PC/그 외 처리 다르게
        // 단, this.gameObject의 tag가 PlayerProjectile일 경우
        // -> 충돌체가 몬스터일 경우 해당 몬스터가 마법화살 소유하도록
        // -> 충돌체가 몬스터가 아니거나 최대거리까지 날아간 경우 바닥에 떨어지도록
        private void OnTriggerEnter(Collider other)
        {
            // ToDo
            //if(other.gameObject.CompareTag("Monster"))
            //{

            //}

            var damageInterface = other.transform.root.GetComponent<IDamage>();
            if (damageInterface != null && other.gameObject.CompareTag("Monster"))
            {
                damageInterface.TakeDamage(this, 10f);
                Destroy(gameObject);
                // Debug.Log($"마법에 피격!");
            }
            else if (other.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }

            // ToDo
            // 적한테 충돌? => 얘는 Destroy, 적한테 Tag 부여, 적이 죽으면 magicArrow를 근처 어느 좌표에 생성
            // 그 외? => Destroy 하지 않고 바로 떨어뜨림

            // 특정 경우에만 Destroy
        }

        public GameObject GetActor()
        {
            return gameObject;
        }

        public void ReverseMoveDir()
        {
            moveDir = -moveDir;
        }

        //public void SetLayer(int layer)
        //{
        //    gameObject.layer = layer;
        //}
    }
}
