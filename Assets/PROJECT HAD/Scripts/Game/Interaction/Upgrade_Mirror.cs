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
            // 거울 강화 UI창 띄우기
            Debug.Log("거울 UI");
        }

        public void InteractEnable()
        {
            var interactionUI = UIManager.Show<InteractionNoticeUI>(UIList.InteractionNotice);
        }
        public void InteractDisenable()
        {
            var interactionUI = UIManager.Hide<InteractionNoticeUI>(UIList.InteractionNotice);
        }

        // UI, 유저 데이터, 게임 데이터 <- 얘네 건드리는 건 따로 빼고 이 스크립트는 Charon으로 이름 바꾸기?
        // OnButton에서 호출
        public void BuyUpgrade()
        {

        }
    }
}
