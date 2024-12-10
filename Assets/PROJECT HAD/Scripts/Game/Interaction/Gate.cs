using UnityEngine;

namespace HAD
{
    public class Gate : MonoBehaviour, IInteractable
    {
        public bool IsAutomaticInteraction => false;

        public string Message => "Gate";

        // 보상 옵젝 획득 후 문 상호작용 활성화
        private bool isActivated = false;
        // ToDo: 랜덤으로 연결할 맵 리스트 준비
        // [SerializeField] private string nextSceneName;
        // ToDo: Inspector 작성 -> 맵 연결 로직 짜서 코드로 제어
        [SerializeField] private string[] nextSceneNames;
        // 이펙스, 사운드
        public GameObject activationEffect;
        public Material[] mat;

        private void Awake()
        {
            GameManager.Instance.AddGate(this);
        }

        public void ActivateGate()
        {
            // ToDo: 연출
            // ExitPoint Visual 색상 변화 + Effect 생성 + Sound
            Instantiate(activationEffect);
            gameObject.GetComponentInChildren<MeshRenderer>().material = mat[1];

            isActivated = true;
        }

        private string SelectNextScene()
        {
            int randomIndex = Random.Range(0, nextSceneNames.Length);
            return nextSceneNames[randomIndex];
        }

        public void Interact(CharacterBase actor)
        {
            if (isActivated)
            {
                // Debug.Log("문이 열렸습니다.");
                GameManager.Instance.LoadLevel(SelectNextScene());
            }
        }

        public void InteractEnable()
        {
            if (isActivated)
            {
                // 과오
                //var message = (this as IInteractable).Message;
                //interactionUI.SetMessage(message);
                var interactionUI = UIManager.Show<InteractionNoticeUI>(UIList.InteractionNotice);
                interactionUI.SetMessage(Message);
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
