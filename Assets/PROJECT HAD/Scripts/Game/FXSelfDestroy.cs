using UnityEngine;

namespace HAD
{
    public class FXSelfDestroy : MonoBehaviour
    {
        public float durationTime = 0.5f;

        private void Awake()
        {
            Destroy(gameObject, 0.5f);
        }

        //IEnumerator SelfDestroy()
        //{
        //    yield return new WaitForSeconds(durationTime);
        //    Destroy(gameObject);
        //}
    }
}
