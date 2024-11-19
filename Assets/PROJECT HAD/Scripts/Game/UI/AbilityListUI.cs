using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HAD
{
    public class AbilityListUI : UIBase
    {
        // ToDo: 어빌마다 본인 이미지 갖고 있는 거 불러와서 Image UI에 연결
        // [SerializeField] GameObject AbilityImage;
        [SerializeField] private TextMeshProUGUI AbilitySlot1;
        [SerializeField] private TextMeshProUGUI AbilitySlot2;
        [SerializeField] private TextMeshProUGUI AbilitySlot3;
        [SerializeField] private TextMeshProUGUI AbilitySlot4;
        [SerializeField] private TextMeshProUGUI AbilitySlot5;
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
                Show();
                isActivated = true;
            }
            else
            {
                Hide();
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
