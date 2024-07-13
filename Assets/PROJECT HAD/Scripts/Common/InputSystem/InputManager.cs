using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HAD
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; } = null;

        private PlayerInput unityPlayerInput;

        public System.Action OnAttackPerformed;
        public System.Action OnMagicPerformed;
        public System.Action OnSpecialAttackPerformed;
        public System.Action OnDashPerformed;

        public Vector2 MovementInput { get; private set; }

        private void Awake()
        {
            Instance = this;
            unityPlayerInput = GetComponent<PlayerInput>();
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
            OnMagicPerformed?.Invoke();
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
