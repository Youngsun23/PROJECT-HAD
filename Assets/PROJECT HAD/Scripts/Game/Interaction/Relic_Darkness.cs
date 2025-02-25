using UnityEngine;

namespace HAD
{
    public class Relic_Darkness : MonoBehaviour, IInteractable
    {
        private int quantity;

        public bool IsAutomaticInteraction => false;

        string IInteractable.Message => "Darkness";

        private void Awake()
        {
            quantity = 10;
        }

        public void Interact(CharacterBase actor)
        {
            UserDataManager.Singleton.UpdateUserDataResources("Darkness", quantity);
            var HUDUI = UIManager.Singleton.GetUI<HUDUI>(UIList.HUD);
            HUDUI.UpdateHUDUIDarkness(quantity);

            if (gameObject.CompareTag("RoomReward"))
            {
                {
                    GameManager.Instance.GetGateList().ForEach(gate => gate.ActivateGate());
                }
            }

            Destroy(gameObject);
        }


        public void InteractEnable()
        {
            var message = (this as IInteractable).Message;
            var interactionUI = UIManager.Show<InteractionNoticeUI>(UIList.InteractionNotice);
            interactionUI.SetMessage(message);
        }
        public void InteractDisenable()
        {
            var interactionUI = UIManager.Hide<InteractionNoticeUI>(UIList.InteractionNotice);
        }
    }
}
