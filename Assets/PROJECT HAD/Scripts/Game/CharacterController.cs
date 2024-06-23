using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class CharacterController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        private void Update()
        {
            Vector2 input = InputManager.Instance.MovementInput;

            // To do : Character를 input에 따라 움직이게 구현
            // Camera 방향에 맞게 WASD로 이동하도록 구현

            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;
            cameraForward.y = 0;

            Vector3 direction = cameraForward.normalized * input.y + cameraRight.normalized * input.x;
            transform.Translate(direction * Time.deltaTime * moveSpeed, Space.World);
        }
    }
}
