using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class CharacterController : MonoBehaviour
    {
        public CharacterBase character;

        public float moveSpeed = 5f;
        public float turnSpeed = 80f;

        // private CharacterCommandInvoker charCommandInvoker; // 동일 클래스(클래스명 변경)
        // Class Try 2 _ Command Pattern
        private CharacterCommandManager charCommandInvoker;

        private void Start()
        {
            var playerCharacterData = GameDataModel.Singleton.GetPlayerCharacterData(1);
            character.InitializeCharacter(playerCharacterData);

            // charCommandInvoker = new CharacterCommandInvoker();
            // Class Try 2 _ Command Pattern
            charCommandInvoker = GetComponent<CharacterCommandManager>();

            InputManager.Instance.OnAttackPerformed += ExecuteAttack;                   
            InputManager.Instance.OnMagicAimPerformed += ExecuteMagicAim;
            InputManager.Instance.OnMagicShotPerformed += ExecuteMagicShot;
            InputManager.Instance.OnSpecialAttackPerformed += ExecuteSpecialAttack;
            InputManager.Instance.OnDashPerformed += ExecuteDash;
        }

        private void ExecuteAttack()
        {
            // @ Client : 손님 주문
            //ICommand attackComboCommand = new AttackCombo1Command(character);
            //charCommandInvoker.AddCommand(attackComboCommand);

            // Class Try 2 _ Command Pattern
            // 일단 하드코딩 -> 점점 바꾸기
            if (character.CurrentAttackComboIndex == 0)
            {
                ICommand attackCombo1Command = new AttackCombo1Command(character);
                charCommandInvoker.AddCommand(attackCombo1Command);
            }
            else if(character.CurrentAttackComboIndex == 1)
            {
                ICommand attackCombo2Command = new AttackCombo2Command(character);
                charCommandInvoker.AddCommand(attackCombo2Command);
            }
            else if(character.CurrentAttackComboIndex == 2)
            {
                ICommand attackCombo3Command = new AttackCombo3Command(character);
                charCommandInvoker.AddCommand(attackCombo3Command);
            }

            // character.Attack();
            character.PerformAttackCombo();
        }

        private void ExecuteMagicAim()
        {
            character.MagicAim();
        }

        // ToDo: ExecuteMagicAim으로부터 1초 이내에 아직 Shot 되지 않았으면 강제로 ExecuteMagicShot() 호출
        // 이후 mouse button up에서 중복호출 되지 않도록 막기
        private void ExecuteMagicShot()
        {
            character.MagicShot();
        }

        private void ExecuteSpecialAttack()
        {
            character.SpecialAttack();
        }

        private void ExecuteDash()
        {
            character.Dash();
        }

        private void Update()
        {
            if(!character.IsDashing && !character.IsAttacking && !character.IsMagicAiming)
            {
                Vector2 input = InputManager.Instance.MovementInput;
                character.Move(input, Camera.main.transform.eulerAngles.y);
            }
        }

        #region Old ver.Update(이동/회전)
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

        //    // 키 조작 있을 때만
        //    if (input != Vector2.zero)
        //    {
        //        // 새 회전값 = 이동 방향{direction}을 바라보도록 하는 회전값
        //        Quaternion newRotation = Quaternion.LookRotation(direction);
        //        // this의 회전값을 현재 회전값->새 회전값으로 보간 변경
        //        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
        //    }
        // }
        #endregion
    }
}
