using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    // 인풋 입력 -> 애니메이션, 이동
    public class CharacterBase : MonoBehaviour, IDamage, IActor
    {
        // protected로 돌리는 것과, CharacterAttributeComponent 사용하는 것의 차이?
        protected CharacterAttributeComponent characterAttributeComponent; 
        public CharacterAttributeComponent CharacterAttributeComponent => characterAttributeComponent;

        [Title("Components")]
        public UnityEngine.CharacterController characterController;
        public Animator characterAnimator;

        [Title("Character Movement")]
        // public LayerMask groundLayer;
        private float rotationSmoothTime = 0.12f;
        private float rotationVelocity;
        private float verticalVelocity;
        private float targetRotation;
        private float targetMoveSpeed;
        private Vector2 targetMoveInput;
        private Vector2 targetMoveInputBlend;
        private Vector3 lastPosition;
        private bool isGrounded = false;

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {
            characterController = GetComponent<UnityEngine.CharacterController>();
        }

        protected virtual void Update()
        {
            targetMoveInputBlend = Vector2.Lerp(targetMoveInputBlend, targetMoveInput, Time.deltaTime * 10);
            characterAnimator.SetFloat("Horizontal", targetMoveInputBlend.x);
            characterAnimator.SetFloat("Vertical", targetMoveInputBlend.y);

            IsGround();
            FreeFall();
        }

        public void Move(Vector2 input, float yAxis)
        {
            // Debug.Log($"name: {this.gameObject.name} / tag: {this.gameObject.tag}");
            if(this.gameObject.CompareTag("Player"))
            {
                targetMoveSpeed = input != Vector2.zero ? characterAttributeComponent.GetAttribute(AttributeTypes.MoveSpeed).CurrentValue : 0.0f;
            }
            else if(this.gameObject.CompareTag("Monster"))
            {
                MonsterBase monsterBase = this as MonsterBase;
                targetMoveSpeed = input != Vector2.zero ? monsterBase.MoveSpeed : 0.0f;
            }

            Vector3 inputDirection = new Vector3(input.x, 0.0f, input.y).normalized;
            // 거미는 엉덩이로 걸어다니고 상자는 제대로 다니네...
            if (input != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + yAxis;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 deltaPosition = transform.InverseTransformDirection(transform.position - lastPosition).normalized;
            targetMoveInput = new Vector2(deltaPosition.x, deltaPosition.z);
            lastPosition = transform.position;

            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
            characterController.Move(
                targetDirection.normalized * (targetMoveSpeed * Time.deltaTime)     // 인풋이 있다면 움직이는곳 XZ 축으로 이동
                + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);      // 인풋이 있던 없던 중력 Y 축을 이동
        }

        private void IsGround()
        {
            RaycastHit groundHit;
            isGrounded = Physics.SphereCast(transform.position + (Vector3.up * characterController.radius), characterController.radius, Vector3.down, out groundHit, 0.1f, 1 << 3);
            // 적당히 0.1f(t/f 오가면서 지면에서 과하게 떨어지지 않음) 했는데...
        }
        // 일정거리 이하로 캐릭터의 y가 내려가지 않음. (일부 애니메이션의 발 위치가 Idle보다 낮아진다는 걸 생각하면 괜찮은데,
        // 문제는 이유를 모르겠음) (공중에서는 무한 낙하하고, collider 위치도 문제 없어보이는데 왜? height 조절해도 비슷...)

        private void FreeFall()
        {
            if (!isGrounded) // &&!isDashing 삭제함
                verticalVelocity = -9.8f;
            else
                verticalVelocity = 0;
        }

        // ===============================================================

        // TakeDamage, GetActor, AddBuffed 공통 사용 가능 OX?
        // CharacterGameData, MonsterGameData -> override
        // MonsterGameData의 스탯들도 Attribute화를 해야 하는지?
        // curMoveSpeed, curHP 같은 값들을 Character/Monster 갈라서 받아놓고 사용하기?
        // 둘의 상위 클래스 GameDataBase 만들어서 묶기?
        // 어차피 base 비워두고 override할 거면 여기에 둘 필요가 있나?

        public virtual void TakeDamage(IActor actor, float damage)
        {
            
        }

        public GameObject GetActor()
        {
            return gameObject;
        }

        // Attribute
        public virtual void AddBuffed(AttributeTypes type, float buffedValue)
        {

        }
        public void ExecuteDamage(IDamage damageInterface, float damage)
        {
            damageInterface?.TakeDamage(this, damage);
        }
    }
}