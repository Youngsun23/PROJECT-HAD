using Cinemachine;
using Sirenix.Utilities;
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
        //public Vector2 borderRange;

        private Camera mainCamera;

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
            #region 보더라인 체크 방식
            //bool isHorizontalBorder = false;
            //bool isVerticalBorder = false;  
            //Vector2 mousePosition = Input.mousePosition; // 왼쪽아래(0,0), 해상도 기준
            //// Debug.Log(mousePosition);
            //// 해상도 무관 뷰포트 기준으로 변환
            //Vector3 mouseViewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);
            //if(mouseViewportPosition.x < borderRange.x)
            //{
            //    // Left Border
            //    isHorizontalBorder = false;
            //}
            //if(mouseViewportPosition.x > 1 - borderRange.x)
            //{
            //    // Right Border
            //    isHorizontalBorder = true;
            //}
            //if(mouseViewportPosition.y < borderRange.y)
            //{
            //    // Bottom Border
            //    isVerticalBorder = false;
            //}
            //if(mouseViewportPosition.y > 1 - borderRange.y)
            //{
            //    // Top Border
            //    isVerticalBorder = true;
            //}

            //// Left Top, Left Bottom, ... 각각에 대해 처리
            //if(isHorizontalBorder)
            //{

            //}
            #endregion
            Vector2 mousePosition = Input.mousePosition;
            Vector3 mouseViewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);

            Vector3 differ = mouseViewportPosition - new Vector3(0.5f, 0.5f, 0);
            // ToDo: differ값(differ.magnitude)만큼, 중앙~커서 위치 방향(!주의!)으로, CharacterController.instance의 카메라 피벗 이동시켜주는 함수 만들기
            // 방향: 마우스가 화면의 +x축, 피벗 로컬 기준으로는 웬 대각선 방향으로 가야 함. 쿼터뷰니까...
            // CharacterController.Instance.MoveCameraPivot(direction, differMagnitude);
        }
    }
}
