using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class CharacterController : MonoBehaviour
    {
        public static CharacterController Instance { get; private set; }

        public PlayerCharacter character;
        public InteractionSensor interactionSensor;
        //private bool interactionInformed = false;
        private bool interactionEnabled = false;
        private IInteractable interactableObject;

        public Transform cameraPivot;

        public float moveSpeed = 5f;
        public float turnSpeed = 80f;

        // private CharacterCommandInvoker charCommandInvoker; // 동일 클래스(클래스명 변경)
        // Class Try 2 _ Command Pattern
        // private CharacterCommandManager charCommandInvoker;
        private CharacterCommandManager characterCommandManager;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Start()
        {
            var playerCharacterData = GameDataModel.Singleton.GetPlayerCharacterData(1);
            character.InitializeCharacter(playerCharacterData);

            CameraSystem.Instance.CameraPivot = cameraPivot;

            // charCommandInvoker = new CharacterCommandInvoker();
            // Class Try 2 _ Command Pattern
            //charCommandInvoker = GetComponent<CharacterCommandManager>();
            characterCommandManager = GetComponent<CharacterCommandManager>();

            InputManager.Instance.OnAttackPerformed += ExecuteAttack;                   
            InputManager.Instance.OnMagicAimPerformed += ExecuteMagicAim;
            InputManager.Instance.OnMagicShotPerformed += ExecuteMagicShot;
            InputManager.Instance.OnSpecialAttackPerformed += ExecuteSpecialAttack;
            InputManager.Instance.OnDashPerformed += ExecuteDash;

            interactionSensor.OnDetectedInteractable += OnDetectedInteractable;
            interactionSensor.OnLostInteractable += OnLostInteractable;
            InputManager.Instance.OnInteractPerformed += OnInteraction;

            InputManager.Instance.OnCharacterInfoMenuPerformed += ExecuteCharacterInfoMenu;
        }

        private void ExecuteCharacterInfoMenu()
        {
            AbilityListUI.Instance.SwitchAbilityListUI();
        }

        private void OnDetectedInteractable(IInteractable interactable)
        {
            if(interactable.IsAutomaticInteraction)
            {
                // 자동 상호작용 (템 획득 등)
                interactable.Interact(character);
            }
            else
            {
                //interactionInformed = true;
                //interactionSensor.SetInteractionInformed(true);

                // 상호작용 가능 UI 켜기
                interactionEnabled = true;
                interactableObject = interactable;
                interactable.InteractEnable();
            }
        }

        private void OnLostInteractable(IInteractable interactable)
        {
            if (!interactable.IsAutomaticInteraction)
            {
                //interactionInformed = false;
                //interactionSensor.SetInteractionInformed(false);

                // 상호작용 가능 UI 끄기
                interactionEnabled = false;
                interactableObject = null;
                interactable.InteractDisenable();
            }
        }

        private void OnInteraction()
        {
            if (interactionEnabled)
            {
                interactionEnabled = false;
                interactableObject?.InteractDisenable();
                interactableObject?.Interact(character);
            }
        }

        // public void MoveCameraPivot(Vector3 dir, float magnitude) { }

        private void ExecuteAttack()
        {
            #region trial 1

            // @ Client : 손님 주문
            //ICommand attackComboCommand = new AttackCombo1Command(character);
            //charCommandInvoker.AddCommand(attackComboCommand);
            #endregion

            #region trial 2
            // Class Try 2 _ Command Pattern
            // 일단 하드코딩 -> 점점 바꾸기
            //if (character.CurrentAttackComboIndex == 0)
            //{
            //    ICommand attackCombo1Command = new AttackCombo1Command(character);
            //    charCommandInvoker.AddCommand(attackCombo1Command);
            //}
            //else if (character.CurrentAttackComboIndex == 1)
            //{
            //    ICommand attackCombo2Command = new AttackCombo2Command(character);
            //    charCommandInvoker.AddCommand(attackCombo2Command);
            //}
            //else if (character.CurrentAttackComboIndex == 2)
            //{
            //    ICommand attackCombo3Command = new AttackCombo3Command(character);
            //    charCommandInvoker.AddCommand(attackCombo3Command);
            //}
            #endregion

            #region trial 3
            //// 대체 코드? 이러면 결국 다를 게 없음
            //if (character.CurrentAttackComboIndex == 0)
            //{
            //    characterCommandManager.ExecuteCommand(commands: new List<ICommand> { characterCommandManager.singleCommand });
            //}
            //else if (character.CurrentAttackComboIndex == 1)
            //{
            //    characterCommandManager.singleCommand = AttackComboCommands.Create<AttackCombo2Command>(character);
            //    characterCommandManager.ExecuteCommand(commands: new List<ICommand> { characterCommandManager.singleCommand });
            //}
            //else if (character.CurrentAttackComboIndex == 2)
            //{
            //    characterCommandManager.singleCommand = AttackComboCommands.Create<AttackCombo3Command>(character);
            //    characterCommandManager.ExecuteCommand(commands: new List<ICommand> { characterCommandManager.singleCommand });
            //}
            #endregion

            characterCommandManager.AddCommand(character.CurrentAttackComboIndex);

            // character.Attack();
            // character.PerformAttackCombo();
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
