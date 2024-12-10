using TMPro;
using UnityEngine;

namespace HAD
{
    public class InteractionNoticeUI : UIBase
    {
        #region Try
        //[SerializeField] private GameObject interactionUIImage;
        //[SerializeField] private TextMeshProUGUI interactionUIText;

        //public bool isDisplayed = false;

        //private void Start()
        //{
        //    interactionUIImage.SetActive(false);
        //}

        //public void Open(string interactionText)
        //{
        //    interactionUIText.text = interactionText;
        //    interactionUIImage.SetActive(true);
        //    isDisplayed = true;
        //}

        //public void Close()
        //{
        //    interactionUIImage.SetActive(false);
        //    isDisplayed = false;
        //}
        #endregion

        //[SerializeField] private GameObject InteractionEnableUIPrefab;
        //private static InteractionNoticeUI Instance;
        [SerializeField] private TextMeshProUGUI Text;

        public enum IconType
        {

        }

        public void SetMessage(string message)
        {
            Text.text = message;
        }

        //public void Display()
        //{
        //    Show();
        //}

        //public void Hide()
        //{
        //    Hide();
        //}
    }
}
