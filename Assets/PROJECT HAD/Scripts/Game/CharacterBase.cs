using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    // 인풋 입력 -> 애니메이션, 이동
    public class CharacterBase : MonoBehaviour
    {
        public UnityEngine.CharacterController characterController;
        public Animator characterAnimator;
        public float moveSpeed;
        public float RotationSmoothTime = 0.12f;
        private float rotationVelocity;
        private float verticalVelocity;
        private float targetRotation;

        public void Move(Vector2 input, float yAxis)
        {
            characterAnimator.SetFloat("Horizontal", input.x);
            characterAnimator.SetFloat("Vertical", input.y);

            Vector3 inputDirection = new Vector3(input.x, 0.0f, input.y).normalized;
            if(input != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + yAxis;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            characterController.Move(targetDirection.normalized * (moveSpeed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
        }
    }
}
