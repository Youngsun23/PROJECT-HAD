using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

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
