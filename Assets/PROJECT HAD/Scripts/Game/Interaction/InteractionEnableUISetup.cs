using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HAD
{
    public class InteractionEnableUISetup : MonoBehaviour
    {
        # region Try
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

        [SerializeField] private GameObject InteractionEnableUIPrefab;
        private static InteractionEnableUISetup Instance;

        // 
        public enum IconType
        {

        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Instance.InteractionEnableUIPrefab.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void Display()
        { 
            Instance.InteractionEnableUIPrefab.SetActive(true);
        }

        public static void Hide()
        {
            Instance.InteractionEnableUIPrefab.SetActive(false);
        }
    }
}
