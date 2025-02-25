using Sirenix.OdinInspector;
using UnityEngine;

namespace HAD
{
    public class CharacterBase : MonoBehaviour, IDamage, IActor
    {
        protected CharacterAttributeComponent characterAttributeComponent; 
        public CharacterAttributeComponent CharacterAttributeComponent => characterAttributeComponent;

        [Title("Components")]
        public UnityEngine.CharacterController characterController;
        public Animator characterAnimator;

        [Title("Character Movement")]
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
            lastPosition = transform.position;
            characterController = GetComponent<UnityEngine.CharacterController>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void OnDestroy()
        {

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
                targetDirection.normalized * (targetMoveSpeed * Time.deltaTime)   
                + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);     
        }

        private void IsGround()
        {
            RaycastHit groundHit;
            isGrounded = Physics.SphereCast(transform.position + (Vector3.up * characterController.radius), characterController.radius, Vector3.down, out groundHit, 0.1f, 1 << 3);
        }

        private void FreeFall()
        {
            if (!isGrounded) 
                verticalVelocity = -9.8f;
            else
                verticalVelocity = 0;
        }

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