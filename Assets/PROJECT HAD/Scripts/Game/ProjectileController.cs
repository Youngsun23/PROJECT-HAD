using UnityEngine;

namespace HAD
{
    public class ProjectileController : MonoBehaviour, IActor
    {
        public float arrowSpeed = 8f;
        public string targetTag;
        private Vector3 moveDir;

        private void Awake()
        {
            moveDir = -transform.forward;
        }

        private void Update()
        {
            transform.Translate(moveDir * arrowSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            var damageInterface = other.transform.root.GetComponent<IDamage>();
            if (damageInterface != null && other.gameObject.CompareTag("Monster"))
            {
                damageInterface.TakeDamage(this, 10f);
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }

        public GameObject GetActor()
        {
            return gameObject;
        }

        public void ReverseMoveDir()
        {
            moveDir = -moveDir;
        }
    }
}
