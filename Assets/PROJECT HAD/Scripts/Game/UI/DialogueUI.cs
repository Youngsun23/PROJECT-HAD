using TMPro;
using UnityEngine;

namespace HAD
{
    // 임시
    public class DialogueUI : UIBase
    {
        [SerializeField] private TextMeshProUGUI Dialoguetext;
        [SerializeField] private CSVParser csvParser;
        // ToDo: monologue, infoboard, special도 추가
        // [SerializeField] private TextMeshProUGUI monologueUI;

        // 음...
        private void Awake()
        {
            csvParser = FindObjectOfType<CSVParser>();
        }

        public void DisplayText(string key)
        {
            //Show();
            string koreanText = csvParser.GetKoreanText(key);
            if (!string.IsNullOrEmpty(koreanText))
            {
                Dialoguetext.text = koreanText;
            }
        }
    }
}
