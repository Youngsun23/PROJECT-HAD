using UnityEngine;

namespace HAD
{
    public class Gate : MonoBehaviour, IInteractable
    {
        public bool IsAutomaticInteraction => false;
        public string Message => "Gate";

        private bool isActivated = false;
        [SerializeField] private string[] nextSceneNames;
        public GameObject activationEffect;
        public Material[] mat;

        private void Awake()
        {
            GameManager.Instance.AddGate(this);
        }

        public void ActivateGate()
        {
            Instantiate(activationEffect, transform.position, Quaternion.identity);
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
                GameManager.Instance.LoadLevel(SelectNextScene());
            }
        }

        public void InteractEnable()
        {
            if (isActivated)
            {
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
