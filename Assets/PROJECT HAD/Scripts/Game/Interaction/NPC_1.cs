using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class NPC_1 : MonoBehaviour, IInteractable
    {
        public bool IsAutomaticInteraction => false;

        public void Interact(CharacterBase actor)
        {
            // ToDo: NPC의 상황에 맞는 대화로그 UI창 띄우기
            // DialogueUISetup.Instance.Display("Hi!\n I'm NPC 1!"); // 임시

            // CSVParser 사용 ex
            //DialogueUISetup.Instance.DisplayText("msg_m_60");
            var DialogueUI = UIManager.Singleton.GetUI<DialogueUI>(UIList.Dialogue);
            DialogueUI.DisplayText("msg_m_60");
        }

        public void InteractEnable()
        {
            var interactionUI = UIManager.Show<InteractionNoticeUI>(UIList.InteractionNotice);
        }
        public void InteractDisenable()
        {
            var interactionUI = UIManager.Hide<InteractionNoticeUI>(UIList.InteractionNotice);
        }
    }
}
