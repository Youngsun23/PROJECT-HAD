//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//namespace HAD
//{
//    public class DialogueUISetup : MonoBehaviour
//    {
//        [SerializeField] private GameObject DialogueUIPrefab;
//        public static DialogueUISetup Instance { get; private set; }
//        //
//        [SerializeField] private CSVParser csvParser;
//        [SerializeField] private TextMeshProUGUI monologueUI;
//        // ToDo: dialogue, infoboard, special도 추가

//        private void Awake()
//        {
//            if (Instance == null)
//            {
//                Instance = this;
//                // Instance.DialogueUIPrefab.SetActive(false);
//            }
//            else
//            {
//                Destroy(gameObject);
//            }
//        }

//        public void Display(string txt)
//        {
//            // Instance.DialogueUIPrefab.GetComponent<DialogueUI>().Setup(txt);
//            monologueUI.text = txt;
//            Instance.DialogueUIPrefab.SetActive(true);
//        }

//        public void Hide()
//        {
//            Instance.DialogueUIPrefab.SetActive(false);
//        }

//        //
//        public void DisplayText(string key)
//        {
//            string koreanText = csvParser.GetKoreanText(key);
//            if (!string.IsNullOrEmpty(koreanText))
//            {
//                monologueUI.text = koreanText;
//            }
//            Instance.DialogueUIPrefab.SetActive(true);
//        }
//    }
//}
