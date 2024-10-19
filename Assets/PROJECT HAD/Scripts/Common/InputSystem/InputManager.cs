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
        public System.Action OnInteractPerformed;
        public System.Action OnCharacterInfoMenuPerformed;

        // 커맨드 패턴 시도
        // public CharacterBase character;
        // private CharacterCommandInvoker charCommandInvoker;

        public Vector2 MovementInput { get; private set; }

        private void Awake()
        {
            Instance = this;
            unityPlayerInput = GetComponent<PlayerInput>();

            var actionMap = unityPlayerInput.actions.FindActionMap("Player"); 
            var magicAction = actionMap["Magic"];
            magicAction.canceled += ctx => OnMagicCanceled();
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

        public void OnInteract()
        {
            OnInteractPerformed?.Invoke();
        }

        public void OnCharacterInfoMenu()
        {
            OnCharacterInfoMenuPerformed?.Invoke();
        }

        public void OnEscape()
        {
            // var testUI2 = UIManager.Singleton.GetUI<TestUI2>(UIList.TestUI2);
            // testUI2.Show();

            // 간략화 버전
            // var testUI = UIManager.Show<TestUI>(UIList.TestUI);
        }
    }
}
