using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class Upgrade_Charon : MonoBehaviour, IInteractable
    {
        public bool IsAutomaticInteraction => false;
        string IInteractable.Message => "Buy";
        [SerializeField] private List<CharonGameData> charonData;

        public void Interact(CharacterBase actor)
        {

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

        public void BuyUpgrade()
        {

        }
    }
}
