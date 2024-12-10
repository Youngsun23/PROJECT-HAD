using UnityEngine;

namespace HAD
{
    public class Relic_Darkness : MonoBehaviour, IInteractable
    {
        // 아이템 아이콘, 수량 // 일단 UI Text는 "E Interact"로 통일
        private int quantity; // 이 옵젝 Instantiate하는 쪽에서 Set 해줄 것

        public bool IsAutomaticInteraction => false;

        string IInteractable.Message => "Darkness";

        private void Awake()
        {
            // 랜덤화
            quantity = 10;
        }

        public void Interact(CharacterBase actor)
        {
            // 플레이어의 Darkness 수량을 quantity만큼 증가, 획득 UI 등의 연출
            // DropItem 코드 피드백 전까지 임시
            Debug.Log("+ Darkness 획득 +");
            UserDataManager.Singleton.UpdateUserDataResources("Darkness", quantity);
            var HUDUI = UIManager.Singleton.GetUI<HUDUI>(UIList.HUD);
            HUDUI.UpdateHUDUIDarkness(quantity);
            // 유저데이터

            // 만약 맵 보상으로 생성된 경우라면, 문의 상호작용 버튼 on?
            if (this.gameObject.CompareTag("RoomReward"))
            {
                // GameObject.FindGameObjectsWithTag("Gate")
                {
                    // gamemanager에 list 두고 door의 awake에서 리스트에 자기 등록, 씬 언로드할 때 리스트 초기화
                    // list 돌면서 activate 함수 호출
                    Debug.Log("문을 활성화");
                    GameManager.Instance.GetGateList().ForEach(gate => gate.ActivateGate());
                }
            }

            Destroy(this.gameObject);
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
