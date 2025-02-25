using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class CameraSystem : MonoBehaviour
    {
        public static CameraSystem Instance { get; private set; }

        public Transform CameraPivot
        {
            get => playerCamera.Follow;
            set
            {
                playerCamera.Follow = value;
                playerCamera.LookAt = value;
            }
        }

        public CinemachineVirtualCamera playerCamera;
        public CinemachineConfiner currentConfiner;
        public CameraShaker cameraShaker;
        private Camera mainCamera;

        [field: SerializeField, MinMaxSlider(-10f, 10f, true)] public Vector2 CameraOffsetRangeX { get; private set; }
        [field: SerializeField, MinMaxSlider(-10f, 10f, true)] public Vector2 CameraOffsetRangeY { get; private set; }
        [field: SerializeField] public float CameraOffsetMaxDistance { get; private set; }
        private List<MeshRenderer> transparentMeshs;

        private void Awake()
        {
            Instance = this;
            mainCamera = Camera.main;

            transparentMeshs = new List<MeshRenderer>();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void SetConfinerVolume(Collider collider)
        {
            currentConfiner.m_BoundingVolume = collider;
        }

        public void ShakeCamera()
        {
            cameraShaker.ShakeCamera();
        }

        #region 벽 반투명화 (사용X)
        //public LayerMask transparentMask;
        //private List<Renderer> subscribeRenderers = new List<Renderer>();
        //private void LateUpdate()
        //{
        //    for (int i = 0; i < subscribeRenderers.Count; i++)
        //    {
        //        Renderer renderer = subscribeRenderers[i];
        //        renderer.material.SetColor("BaseColor", new Color(1, 1, 1, 1f));
        //    }
        //    subscribeRenderers.Clear();

        //    Vector3 direction = (CharacterController.Instance.transform.position - mainCamera.transform.position).normalized;
        //    Ray ray = new Ray(mainCamera.transform.position, direction);
        //    RaycastHit[] hits = Physics.RaycastAll(ray, 1000f, transparentMask);

        //    for (int i = 0; i < hits.Length; i++)
        //    {
        //        if (hits[i].collider.CompareTag("Player"))
        //        {
        //            break;
        //        }
        //        Renderer renderer = hits[i].transform.GetComponent<Renderer>();
        //        renderer.material.SetColor("BaseColor", new Color(1, 1, 1, 0.5f));
        //        subscribeRenderers.Add(renderer);
        //    }
        //}
        #endregion

        #region 카메라 커서 팔로잉 (사용X)
        //private void Update()
        //{
        //    Transform charaterTransform = CharacterController.Instance.transform;
        //    Vector3 mousePositionAsViewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //    Vector3 centerViewport = new Vector3(0.5f, 0.5f, 0);
        //    Vector3 distMouseViewport = mousePositionAsViewport - centerViewport;

        //    float threshold = 0.1f;
        //    if (distMouseViewport.magnitude < threshold)
        //    {
        //        Vector3 originPosition = Vector3.up;
        //        CameraPivot.transform.localPosition = Vector3.Lerp(CameraPivot.transform.localPosition, originPosition, Time.deltaTime * 20f);
        //    }
        //    else
        //    {
        //        float clampX = Mathf.Clamp(distMouseViewport.x, CameraOffsetRangeX.x, CameraOffsetRangeX.y);
        //        float clampY = Mathf.Clamp(distMouseViewport.y, CameraOffsetRangeY.x, CameraOffsetRangeY.y);

        //        Vector3 direction = new Vector3(clampX, clampY, 0);

        //        Vector3 cameraDirection = Camera.main.transform.TransformDirection(direction);

        //        Vector3 localDirection = charaterTransform.transform.InverseTransformDirection(cameraDirection);

        //        Vector3 targetPosition = Vector3.up + localDirection * CameraOffsetMaxDistance;
        //        CameraPivot.transform.localPosition = Vector3.Lerp(CameraPivot.transform.localPosition, targetPosition, Time.deltaTime * 20f);
        //    }
        //}
        #endregion
    }
}