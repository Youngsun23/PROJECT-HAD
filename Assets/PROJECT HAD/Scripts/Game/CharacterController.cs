using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class CharacterController : MonoBehaviour
    {
        public CharacterBase character;

        public float moveSpeed = 5f;
        // @YS--회전 속력
        public float turnSpeed = 80f;
        // --

        private void Update()
        {
            Vector2 input = InputManager.Instance.MovementInput;
            character.Move(input, Camera.main.transform.eulerAngles.y);
        }

        //private void Update()
        //{
        //    Vector2 input = InputManager.Instance.MovementInput;

        //    // To do : Character를 input에 따라 움직이게 구현
        //    // Camera 방향에 맞게 WASD로 이동하도록 구현

        //    Vector3 cameraForward = Camera.main.transform.forward;
        //    Vector3 cameraRight = Camera.main.transform.right;
        //    cameraForward.y = 0;

        //    Vector3 direction = cameraForward.normalized * input.y + cameraRight.normalized * input.x;
        //    transform.Translate(direction * Time.deltaTime * moveSpeed, Space.World);

        //    // @YS--캐릭터 이동 방향에 맞게 회전
        //    // 키 조작 있을 때만
        //    if (input != Vector2.zero)
        //    {
        //        // 새 회전값 = 이동 방향{direction}을 바라보도록 하는 회전값
        //        Quaternion newRotation = Quaternion.LookRotation(direction);
        //        // this의 회전값을 현재 회전값->새 회전값으로 보간 변경
        //        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
        //    }
        //    // --
        // }
    }
}
