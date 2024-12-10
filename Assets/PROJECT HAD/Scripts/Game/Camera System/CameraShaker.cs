using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HAD
{
    public class CameraShaker : MonoBehaviour
    {
        public CinemachineImpulseSource impulseSource;

        private void Awake()
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        [Button("Shake Camera")]
        public void ShakeCamera()
        {
            impulseSource.GenerateImpulse();
        }
    }
}
