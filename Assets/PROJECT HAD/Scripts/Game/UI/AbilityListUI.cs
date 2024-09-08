using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HAD
{
    public class AbilityListUI : MonoBehaviour
    {
        [SerializeField] GameObject AbilityImage;
        [SerializeField] TextMeshProUGUI AbilitySlot1;
        [SerializeField] TextMeshProUGUI AbilitySlot2;
        [SerializeField] TextMeshProUGUI AbilitySlot3;
        [SerializeField] TextMeshProUGUI AbilitySlot4;
        [SerializeField] TextMeshProUGUI AbilitySlot5;
        private bool isActivated = false;
        // private int registeredAbility = 0;

        public static AbilityListUI Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void SwitchAbilityListUI()
        {
            if(!isActivated)
            {
                AbilityImage.SetActive(true);
                isActivated = true;
            }
            else
            {
                AbilityImage.SetActive(false);
                isActivated = false;
            }
        }

        public void RegisterAbility(AbilityBase abil)
        {
            if(abil.Tag.HasFlag(AbilityTag.Attack))
            {
                AbilitySlot1.text = abil.Name;
            }
            if (abil.Tag.HasFlag(AbilityTag.SpecialAttack))
            {
                AbilitySlot2.text = abil.Name;
            }
            if (abil.Tag.HasFlag(AbilityTag.Magic))
            {
                AbilitySlot3.text = abil.Name;
            }
            if (abil.Tag.HasFlag(AbilityTag.Dash))
            {
                AbilitySlot4.text = abil.Name;
            }
            if (abil.Tag.HasFlag(AbilityTag.Gauge))
            {
                AbilitySlot5.text = abil.Name;
            }
        }
    }
}
