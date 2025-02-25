using TMPro;
using UnityEngine;

namespace HAD
{
    // 임시
    public class DialogueUI : UIBase
    {
        [SerializeField] private TextMeshProUGUI Dialoguetext;
        [SerializeField] private CSVParser csvParser;

        private void Awake()
        {
            csvParser = FindObjectOfType<CSVParser>();
        }

        public void DisplayText(string key)
        {
            string koreanText = csvParser.GetKoreanText(key);
            if (!string.IsNullOrEmpty(koreanText))
            {
                Dialoguetext.text = koreanText;
            }
        }
    }
}
