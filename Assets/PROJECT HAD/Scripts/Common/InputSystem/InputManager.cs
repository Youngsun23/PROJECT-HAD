using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

namespace HAD
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; } = null;

        private PlayerInput unityPlayerInput;

        public System.Action OnAttackPerformed;
        public System.Action OnMagicAimPerformed;
        public System.Action OnMagicShotPerformed;
        public System.Action OnSpecialAttackPerformed;
        public System.Action OnDashPerformed;

        // 커맨드 패턴 시도
        public CharacterBase character;
        private CharacterCommandInvoker charCommandInvoker;

        public Vector2 MovementInput { get; private set; }

        private void Awake()
        {
            Instance = this;
            unityPlayerInput = GetComponent<PlayerInput>();

            var actionMap = unityPlayerInput.actions.FindActionMap("Player"); 
            var magicAction = actionMap["Magic"];
            magicAction.canceled += ctx => OnMagicCanceled();
        }

        private void Start()
        {
            charCommandInvoker = new CharacterCommandInvoker();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void OnMove(InputValue input)
        {
            MovementInput = input.Get<Vector2>();
        }

        public void OnAttack()
        {
            #region 커맨드 패턴 시도 1
            //// 기존 공격x -> 공격1로, 공격1 -> 공격2로, 공격2 -> 공격3으로 커맨드 추가
            //int currentStateHash = character.GetCurrentAnimationStateHash();
            //ICommand attackCommand = null;

            //// 상태 해시 값을 직접 비교하여 적절한 커맨드를 설정합니다.
            //if (currentStateHash == Animator.StringToHash("Attack1"))
            //{
            //    attackCommand = new Attack2Command(character);
            //    Debug.Log("attack2Command 추가");
            //}
            //else if (currentStateHash == Animator.StringToHash("Attack2"))
            //{
            //    attackCommand = new Attack3Command(character);
            //    Debug.Log("attack3Command 추가");
            //}
            //else
            //{
            //    attackCommand = new Attack1Command(character);
            //    Debug.Log("attack1Command 추가");
            //}

            //if (attackCommand != null)
            //{
            //    character.EnqueueCommand(attackCommand);
            //    OnAttackPerformed?.Invoke();
            //}
            #endregion

            // @ Client : 손님 주문
            ICommand attackComboCommand = new AttackComboCommand(character);
            charCommandInvoker.AddCommand(attackComboCommand);

            OnAttackPerformed?.Invoke();
        }

        public void OnMagic()
        {
            OnMagicAimPerformed?.Invoke();
        }

        public void OnMagicCanceled()
        {
            // Debug.Log("매직 캔슬");
            OnMagicShotPerformed?.Invoke();
        }

        public void OnSpecialAttack()
        {
            OnSpecialAttackPerformed?.Invoke();  
        }

        public void OnDash()
        {
            OnDashPerformed?.Invoke();
        }
    }
}
