using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class Upgrade_Charon : MonoBehaviour, IInteractable
    {
        public bool IsAutomaticInteraction => false;

        public void Interact(CharacterBase actor)
        {
            // 강화 UI창 띄우기
        }

        public void InteractDisenable()
        {
            InteractionEnableUISetup.Display();
        }

        public void InteractEnable()
        {
            InteractionEnableUISetup.Hide();
        }
    }
}
