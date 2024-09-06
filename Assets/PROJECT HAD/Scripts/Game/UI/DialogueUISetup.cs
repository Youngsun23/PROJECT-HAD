using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class DialogueUISetup : MonoBehaviour
    {
        [SerializeField] private GameObject DialogueUIPrefab;
        private static DialogueUISetup Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // Instance.DialogueUIPrefab.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void Display(string txt)
        {
            Instance.DialogueUIPrefab.GetComponent<DialogueUI>().Setup(txt);
            Instance.DialogueUIPrefab.SetActive(true);
        }

        public static void Hide()
        {
            Instance.DialogueUIPrefab.SetActive(false);
        }
    }
}
