using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class Gate : MonoBehaviour, IInteractable
    {
        public bool IsAutomaticInteraction => false;

        // 보상 옵젝 획득 후 문 상호작용 활성화
        private bool isActivated = false;
        // ToDo: 랜덤으로 연결할 맵 리스트 준비
        [SerializeField] private string nextSceneName;

        private void Awake()
        {
            GameManager.Instance.AddGate(this);
        }

        public void ActivateGate()
        {
            // ToDo: 연출

            isActivated = true;
        }

        public void Interact(CharacterBase actor)
        {
            if (isActivated)
            {
                Debug.Log("문이 열렸습니다.");
                GameManager.Instance.LoadLevel(nextSceneName);
            }
        }

        public void InteractEnable()
        {
            if (isActivated)
            {
                var interactionUI = UIManager.Show<InteractionNoticeUI>(UIList.InteractionNotice);
            }
        }

        public void InteractDisenable()
        {
            if (isActivated)
            {
                var interactionUI = UIManager.Hide<InteractionNoticeUI>(UIList.InteractionNotice);  
            }
        }
    }
}
