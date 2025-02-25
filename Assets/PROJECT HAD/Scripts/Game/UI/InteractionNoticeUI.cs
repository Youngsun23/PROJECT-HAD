using TMPro;
using UnityEngine;

namespace HAD
{
    public class InteractionNoticeUI : UIBase
    {
        [SerializeField] private TextMeshProUGUI Text;

        public enum IconType
        {

        }

        public void SetMessage(string message)
        {
            Text.text = message;
        }
    }
}
