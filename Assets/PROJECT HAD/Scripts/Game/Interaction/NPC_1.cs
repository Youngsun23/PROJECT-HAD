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
            DialogueUISetup.Display("Hi!\n I'm NPC 1!"); // 임시
            
        }
        public void InteractEnable()
        {
            InteractionEnableUISetup.Display();
        }
        public void InteractDisenable()
        {
            InteractionEnableUISetup.Hide();
        }
    }
}
