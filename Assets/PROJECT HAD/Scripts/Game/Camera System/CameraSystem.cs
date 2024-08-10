#region
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class CameraSystem : MonoBehaviour
    {
        public static CameraSystem Instance { get; private set; }

        public CinemachineVirtualCamera playerCamera;
        public CinemachineConfiner currentConfiner;

        private Camera mainCamera;
        //[SerializeField] private Transform cameraPivot;  // 피벗 위치
        //[SerializeField] private Transform character;    // 캐릭터 위치 기준

        public float maxMoveDistance = 1.5f; // 피벗이 이동할 최대 거리
        public float smoothSpeed = 4.0f;     // 피벗 이동의 부드러움
        private Vector3 initialPivotPosition;  // 피벗의 초기 위치
        
        // 월드 기준이니까 고정적으로 이 방향 아니야?
        // private Vector3 rightDir = new Vector3(-1, 0, 1).normalized; // -x, +z
        // private Vector3 forwardDir = new Vector3(-1, 0, 1).normalized; //
        // 카메라 피벗이 0,0,0을 벗어나면 부모인 캐릭터를 기준으로 공전을 함
        // 피벗 위치는 그대로 놔둔 채 카메라에 오프셋만 줄 방법 없나?
        // 안 벗어나도 캐릭터 회전 영향 받음

        private void Awake()
        {
            Instance = this;
            mainCamera = Camera.main;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void SetConfinerVolume(Collider collider)
        {
            currentConfiner.m_BoundingVolume = collider;
        }

        private void Update()
        {
            #region Try
            // Vector3 initialPivotPosition = character.position; // 

            // 마우스 위치를 화면 좌표에서 뷰포트 좌표로 변환
            // Vector3 mouseViewportPosition = mainCamera.ScreenToViewportPoint(Input.mousePosition);

            // 뷰포트 좌표에서 중앙과의 차이를 계산
            // Vector3 offsetFromCenter = mouseViewportPosition - new Vector3(0.5f, 0.5f, 0);

            // Vector3 moveDirection = character.right * offsetFromCenter.x + character.forward * offsetFromCenter.y;
            // 캐릭터 로컬 방향으로 해도 gizmos, 실제 회전값 반영한 월드 좌표 아님

            // 피벗의 목표 위치를 계산 (최대 이동 거리 제한)
            // Vector3 targetPivotPosition = initialPivotPosition + moveDirection * maxMoveDistance;

            // 피벗을 부드럽게 이동시킴
            //cameraPivot.localPosition = Vector3.Lerp(cameraPivot.localPosition, targetPivotPosition, Time.deltaTime * smoothSpeed);
            #endregion

            // 
        }

    }
}

// 문제 0. 캐릭터 회전 -> x,z축 기준 달라짐
// 문제 1. 전체 화면이 아닌 게임 화면 기준
// 문제 2. 클릭에 반응하지 않게
