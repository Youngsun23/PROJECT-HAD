using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HAD
{
    // 임시
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI Dialoguetext;  

        public void Setup(string txt)
        {
            Dialoguetext.text = txt;
        }
    }
}
