using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HAD
{
    public class Upgrade_Mirror : MonoBehaviour, IInteractable
    {
        public bool IsAutomaticInteraction => false;

        public void Interact(CharacterBase actor)
        {
            // 강화 UI창 띄우기
            Debug.Log("거울 UI");
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
