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
            // 강화 UI창 띄우기
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

        // UI, 유저 데이터, 게임 데이터 <- 얘네 건드리는 건 따로 빼고 이 스크립트는 Charon으로 이름 바꾸기?
        // OnButton에서 호출
        public void BuyUpgrade()
        {
            // 카론 업그레이드(temp) 유저데이터 갱신
            // UI는 유저데이터 갱신 받아서 팔린 건 시각 변화
            // 다시 못 사게 처리
        }
    }
}
