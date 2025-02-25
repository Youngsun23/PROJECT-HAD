using UnityEngine;

namespace HAD
{
    public class NPC_1 : MonoBehaviour, IInteractable
    {
        public bool IsAutomaticInteraction => false;

        string IInteractable.Message => "Talk";

        public void Interact(CharacterBase actor)
        {
            var DialogueUI = UIManager.Singleton.GetUI<DialogueUI>(UIList.Dialogue);
            DialogueUI.DisplayText("msg_m_60");
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
