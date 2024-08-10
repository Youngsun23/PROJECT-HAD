using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class Relic_Darkness : MonoBehaviour, IInteractable
    {
        // 아이템 아이콘, 수량 // 일단 UI Text는 "E Interact"로 통일
        private int quantity; // 이 옵젝 Instantiate하는 쪽에서 Set 해줄 것

        public bool IsAutomaticInteraction => false;
        public void Interact(CharacterBase actor)
        {
            // 플레이어의 Darkness 수량을 quantity만큼 증가, 획득 UI 등의 연출
            // DropItem 코드 피드백 전까지 임시
            Debug.Log("+ Darkness 획득 +");
            Destroy(this.gameObject);
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
