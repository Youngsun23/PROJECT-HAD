using UnityEngine;

namespace HAD
{
    public class Upgrade_Mirror : MonoBehaviour, IInteractable
    {
        public bool IsAutomaticInteraction => false;
        string IInteractable.Message => "Upgrade";

        public void Interact(CharacterBase actor)
        {
            var mirrorUI = UIManager.Show<MirrorUI>(UIList.Mirror);
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
