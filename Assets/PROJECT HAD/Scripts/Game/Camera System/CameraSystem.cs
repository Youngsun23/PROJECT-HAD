#region Old Try
//using Cinemachine;
//using Sirenix.Utilities;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace HAD
//{
//    public class CameraSystem : MonoBehaviour
//    {
//        public static CameraSystem Instance { get; private set; }

//        public CinemachineVirtualCamera playerCamera;
//        public CinemachineConfiner currentConfiner;
//        //public Vector2 borderRange;

//        private Camera mainCamera;
//        [SerializeField] private Transform cameraPivot;
//        public float moveDistance = 5.0f; // 피벗이 이동할 거리 (*magnitude)
//        public float stopThreshold = 0.1f;  // 멈추는 임계값
//        public float maxPivotDistance = 3.0f; // 피벗의 최대 이동 가능 거리

//        private void Awake()
//        {
//            Instance = this;
//            mainCamera = Camera.main;
//        }

//        private void OnDestroy()
//        {
//            Instance = null;
//        }

//        public void SetConfinerVolume(Collider collider)
//        {
//            currentConfiner.m_BoundingVolume = collider;
//        }

//        private void Update()
//        {
//            #region 보더라인 체크 방식
//            //bool isHorizontalBorder = false;
//            //bool isVerticalBorder = false;  
//            //Vector2 mousePosition = Input.mousePosition; // 왼쪽아래(0,0), 해상도 기준
//            //// Debug.Log(mousePosition);
//            //// 해상도 무관 뷰포트 기준으로 변환
//            //Vector3 mouseViewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);
//            //if(mouseViewportPosition.x < borderRange.x)
//            //{
//            //    // Left Border
//            //    isHorizontalBorder = false;
//            //}
//            //if(mouseViewportPosition.x > 1 - borderRange.x)
//            //{
//            //    // Right Border
//            //    isHorizontalBorder = true;
//            //}
//            //if(mouseViewportPosition.y < borderRange.y)
//            //{
//            //    // Bottom Border
//            //    isVerticalBorder = false;
//            //}
//            //if(mouseViewportPosition.y > 1 - borderRange.y)
//            //{
//            //    // Top Border
//            //    isVerticalBorder = true;
//            //}

//            //// Left Top, Left Bottom, ... 각각에 대해 처리
//            //if(isHorizontalBorder)
//            //{

//            //}
//            #endregion

//            //Vector3 mouseViewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);
//            //Vector3 differ = mouseViewportPosition - new Vector3(0.5f, 0.5f, 0);

//            // ToDo: differ값(differ.magnitude)만큼, 중앙~커서 위치 방향(!주의!)으로, CharacterController.instance의 카메라 피벗 이동시켜주는 함수 만들기
//            // 방향: 마우스가 화면의 +x축, 피벗 로컬 기준으로는 웬 대각선 방향으로 가야 함. 쿼터뷰니까...

//            #region Try
//            //// 마우스 위치를 화면 좌표에서 월드 좌표로 변환
//            //Vector2 mousePosition = Input.mousePosition;
//            //Ray ray = mainCamera.ScreenPointToRay(mousePosition);

//            //// 게임 화면 범위 내에 있는지 확인
//            //if (mousePosition.x < 0 || mousePosition.x > Screen.width || mousePosition.y < 0 || mousePosition.y > Screen.height)
//            //{
//            //    // 마우스 커서가 게임 화면을 벗어난 경우
//            //    return;
//            //}

//            //// 쿼터뷰의 경우 레이캐스트는 일반적으로 y = 0 평면을 향해 쏩니다. 
//            //// 레이가 y=0인 지점에서의 위치를 구합니다.
//            //Plane plane = new Plane(Vector3.up, Vector3.zero); // y = 0 평면
//            //if (plane.Raycast(ray, out float distance))
//            //{
//            //    Vector3 worldMousePosition = ray.GetPoint(distance);

//            //    // 카메라 피벗과 마우스 위치 사이의 방향을 구합니다.
//            //    Vector3 direction = (worldMousePosition - cameraPivot.position).normalized;

//            //    // 마우스 커서와 화면 중앙의 차이를 구합니다. 이 값을 이동의 크기로 사용합니다.
//            //    Vector3 mouseViewportPosition = mainCamera.ScreenToViewportPoint(mousePosition);
//            //    Vector3 differ = mouseViewportPosition - new Vector3(0.5f, 0.5f, 0);

//            //    // 이동 거리를 제한합니다.
//            //    float differMagnitude = differ.magnitude;
//            //    Debug.Log(differ.magnitude);
//            //    if (differMagnitude < stopThreshold) // differMagnitude 최대 약 0.6
//            //    {
//            //        // 차이가 임계값보다 작으면 이동하지 않음
//            //        return;
//            //    }

//            //    //float totalMoveDistance = Mathf.Min(differMagnitude * moveDistance, moveDistance);
//            //    float totalMoveDistance = differMagnitude * moveDistance;

//            //    // 카메라 피벗 이동, 이동 거리를 제한하고 클릭이 아닌 이동에만 반응
//            //    Vector3 newPivotPosition = cameraPivot.position + (direction * totalMoveDistance * Time.deltaTime);

//            //    // 피벗의 최대 이동 가능 거리를 제한
//            //    if (Vector3.Distance(newPivotPosition, transform.position) <= maxPivotDistance)
//            //    {
//            //        cameraPivot.position = newPivotPosition;
//            //    }
//            //}
//            #endregion

// 월드 기준이니까 고정적으로 이 방향 아니야?
// private Vector3 rightDir = new Vector3(-1, 0, 1).normalized; // -x, +z
// private Vector3 forwardDir = new Vector3(-1, 0, 1).normalized; //
// 카메라 피벗이 0,0,0을 벗어나면 부모인 캐릭터를 기준으로 공전을 함
// 피벗 위치는 그대로 놔둔 채 카메라에 오프셋만 줄 방법 없나?
// 안 벗어나도 캐릭터 회전 영향 받음


//            // 마우스 위치를 화면 좌표에서 월드 좌표로 변환
//            Vector2 mousePosition = Input.mousePosition;
//            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

//            // 게임 화면 범위 내에 있는지 확인
//            if (mousePosition.x < 0 || mousePosition.x > Screen.width || mousePosition.y < 0 || mousePosition.y > Screen.height)
//            {
//                // 마우스 커서가 게임 화면을 벗어난 경우
//                return;
//            }

//            // 쿼터뷰의 경우 레이캐스트는 일반적으로 y = 0 평면을 향해 쏩니다. 
//            // 레이가 y=0인 지점에서의 위치를 구합니다.
//            Plane plane = new Plane(Vector3.up, Vector3.zero); // y = 0 평면
//            if (plane.Raycast(ray, out float distance))
//            {
//                Vector3 worldMousePosition = ray.GetPoint(distance);

//                // 카메라 피벗과 마우스 위치 사이의 방향을 구합니다.
//                Vector3 direction = (worldMousePosition - cameraPivot.position).normalized;

//                // 마우스 커서와 화면 중앙의 차이를 구합니다. 이 값을 이동의 크기로 사용합니다.
//                Vector3 mouseViewportPosition = mainCamera.ScreenToViewportPoint(mousePosition);
//                Vector3 differ = mouseViewportPosition - new Vector3(0.5f, 0.5f, 0);

//                // 이동 거리를 제한합니다.
//                float differMagnitude = differ.magnitude;
//                Debug.Log(differ.magnitude);
//                if (differMagnitude < stopThreshold) // differMagnitude 최대 약 0.6
//                {
//                    // 차이가 임계값보다 작으면 이동하지 않음
//                    return;
//                }

//                //float totalMoveDistance = Mathf.Min(differMagnitude * moveDistance, moveDistance);
//                float totalMoveDistance = differMagnitude * moveDistance;

//                //// 카메라 피벗 이동, 이동 거리를 제한하고 클릭이 아닌 이동에만 반응
//                //Vector3 newPivotPosition = cameraPivot.position + (direction * totalMoveDistance * Time.deltaTime);

//                //// 피벗의 최대 이동 가능 거리를 제한
//                //if (Vector3.Distance(newPivotPosition, transform.position) <= maxPivotDistance)
//                //{
//                //    cameraPivot.position = newPivotPosition;
//                //}

//                cameraPivot.position += direction * totalMoveDistance * Time.deltaTime;
//            }
//        }
//    }
//}
#endregion

using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        //[field: SerializeField] public AnimationCurve CameraOffsetCurveX { get; private set; }
        //[field: SerializeField] public AnimationCurve CameraOffsetCurveY { get; private set; }
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

        #region 벽 반투명화 Old
        // 카메라-플레이어 사이 옵젝(벽) 반투명화
        //private void LateUpdate()
        //{
        //    Vector3 direction = CharacterController.Instance.transform.position - mainCamera.transform.position;
        //    // 레이어 마스크 필요하려나?
        //    RaycastHit hit;
        //    if(Physics.Raycast(mainCamera.transform.position, direction.normalized, out hit))
        //    {
        //        // Debug.Log("Raycast에 옵젝 감지");

        //        if (!hit.collider.CompareTag("Player"))
        //        {
        //            // Debug.Log("Raycast에 Player 아닌 옵젝 감지");

        //            MeshRenderer meshRenderer = hit.transform.GetComponent<MeshRenderer>();
        //            // Debug.Log((meshRenderer == null)); // 왜 meshRenderer가 없다고 하는가,,, // mainCam에서 안 쐈으니까...
        //            if(meshRenderer != null && !transparentMeshs.Contains(meshRenderer))
        //            {
        //                // 이 if절을 못 넘어옴
        //                // Debug.Log("Raycast에 감지한 옵젝 알파값 변경");

        //                Color color = meshRenderer.material.color;
        //                color.a = 0.5f;
        //                meshRenderer.material.color = color;
        //                transparentMeshs.Add(meshRenderer);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var mesh in transparentMeshs)
        //            {
        //                Color color = mesh.material.color;
        //                color.a = 1f;
        //                mesh.material.color = color;
        //            }
        //            transparentMeshs.Clear();
        //        }
        //    }
        //}
        #endregion
        #region 임시 Off - 커서 따라 카메라 이동
        // @ 임시 Off @
        //private void Update()
        //{
        //    // Camera Pivot의 기준점 => 캐릭터 위치
        //    // 계산하는 기준점 => 화면의 중앙 기준으로 보았을 때, 현재 Mouse의 위치

        //    // 얼마만큼 움직일꺼냐?
        //    // => 누구를? Camera Pivot 을 움직인다
        //    // => 어떻게? 어디로? 캐릭터의 위치와 계산하는 기준점을 토대로 방향을 계산한다.
        //    // => 얼마나? CameraSystem에 Inspector 창에 노출을해서 값을 조정할 수 있게 잡아보자.

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

        //        // 마우스 움직임을 받아서 이동할 방향
        //        Vector3 direction = new Vector3(clampX, clampY, 0);

        //        // 카메라를 기준으로 마우스 움직임 방향을 월드방향으로 변환
        //        Vector3 cameraDirection = Camera.main.transform.TransformDirection(direction);

        //        // 카메라에서 구한 방향을 CameraPivot의 로컬 방향으로 변환
        //        Vector3 localDirection = charaterTransform.transform.InverseTransformDirection(cameraDirection);

        //        // 최종 움직일 위치 계산
        //        Vector3 targetPosition = Vector3.up + localDirection * CameraOffsetMaxDistance;
        //        CameraPivot.transform.localPosition = Vector3.Lerp(CameraPivot.transform.localPosition, targetPosition, Time.deltaTime * 20f);
        //    }
        //}
        #endregion
    }
}

// 카메라 피벗 계속 바뀌는 중 캐릭터 회전, 클릭 반응 - 매 프레임 Lerp 중이라
// -> Max Distance를 낮게 잡기
// 하지만? 하데스에서는 값 20 정도의 거리를 이동하는데도 이런 문제가 발생하지 않음 (의문...)
