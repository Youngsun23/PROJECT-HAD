using UnityEngine;

namespace HAD
{
    public class CharacterController : MonoBehaviour
    {
        public static CharacterController Instance { get; private set; }

        public PlayerCharacter character;
        public InteractionSensor interactionSensor;
        private bool interactionEnabled = false;
        private IInteractable interactableObject;
        public Transform cameraPivot;
        private CharacterCommandManager characterCommandManager;

        public float moveSpeed = 5f;
        public float turnSpeed = 80f;

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
            var playerCharacterData = GameDataModel.Singleton.GetPlayerCharacterGameData("Default");
            character.InitializeCharacter(playerCharacterData);
            characterCommandManager = GetComponent<CharacterCommandManager>();

            CameraSystem.Instance.CameraPivot = cameraPivot;

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
            var abilityUI = UIManager.Singleton.GetUI<AbilityListUI>(UIList.AbilityList);
            abilityUI.SwitchAbilityListUI();
        }

        private void OnDetectedInteractable(IInteractable interactable)
        {
            if(interactable.IsAutomaticInteraction)
            {
                interactable.Interact(character);
            }
            else
            {
                interactionEnabled = true;
                interactableObject = interactable;
                interactable.InteractEnable();
            }
        }

        private void OnLostInteractable(IInteractable interactable)
        {
            if (!interactable.IsAutomaticInteraction)
            {
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

        private void ExecuteAttack()
        {
            characterCommandManager.AddCommand(character.CurrentAttackComboIndex);
        }

        private void ExecuteMagicAim()
        {
            character.MagicAim();
        }

        private void ExecuteMagicShot()
        {
            character.MagicShot();
        }

        private void ExecuteSpecialAttack()
        {
            character.SpecialAttack();

            var effect = EffectPoolManager.Singleton.GetEffect("SpecialAttack");
            effect.gameObject.transform.position = character.transform.position;
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
    }
}
