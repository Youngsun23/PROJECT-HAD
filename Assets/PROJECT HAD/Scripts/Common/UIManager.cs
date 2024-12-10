using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class UIManager : SingletonBase<UIManager>
    {
        //public static T ShowNewInstance(UIList uiType) where T : UIBase
        //{
        //    T result = null;
        //    if (UIList.PANEL_START < uiType && uiType < UIList.PANEL_END)
        //    {
        //        T loadedPrefab = Resources.Load<T>(UI_Resource_PATH + "UI." + uiType.ToString());
        //        if (loadedPrefab != null)
        //        {
        //            result = Instantiate(loadedPrefab, Singleton.panelRoot);
        //            result.gameObject.SetActive(false);
        //        }
        //    }
        //    return result;
        //}

        public static T Show<T>(UIList uiType) where T : UIBase
        {
            var newUI = Singleton.GetUI<T>(uiType);
            if (newUI == null)
                return null;
            
            newUI.Show();
            return newUI;
        }

        public static T Hide<T>(UIList uiType) where T : UIBase
        {
            var newUI = Singleton.GetUI<T>(uiType);
            if (newUI == null)
                return null;

            newUI.Hide();
            return newUI;
        }

        public Dictionary<UIList, UIBase> ContainerPanel = new Dictionary<UIList, UIBase>();
        public Dictionary<UIList, UIBase> ContainerPopup = new Dictionary<UIList, UIBase>();

        private Transform panelRoot;
        private Transform popupRoot;

        private const string UI_Resource_PATH = "UI/";

        public void Initialize()
        {
            if(panelRoot == null)
            {
                panelRoot = new GameObject("Panel Root").transform;
                panelRoot.SetParent(transform);
            }        
            if(popupRoot == null)
            {
                popupRoot = new GameObject("Popup Root").transform;
                popupRoot.SetParent(transform);
            }

            for(int index = (int)UIList.PANEL_START + 1; index < (int)UIList.PANEL_END; index++)
            {
                UIList uiType = (UIList)index;
                ContainerPanel.Add(uiType, null);
            }            
            for(int index = (int)UIList.POPUP_START + 1; index < (int)UIList.POPUP_END; index++)
            {
                UIList uiType = (UIList)index;
                ContainerPopup.Add(uiType, null);
            }
        }

        public T GetUI<T>(UIList uiType) where T : UIBase
        {
            T result = null;
            if(UIList.PANEL_START < uiType && uiType < UIList.PANEL_END)
            {
                if (ContainerPanel[uiType] == null)
                {
                    T loadedPrefab = Resources.Load<T>(UI_Resource_PATH + "UI." + uiType.ToString());
                    if (loadedPrefab != null)
                    {
                        result = Instantiate(loadedPrefab, panelRoot);
                        //result.gameObject.SetActive(false);
                        ContainerPanel[uiType] = result;
                    }
                }
                else
                {
                    result = ContainerPanel[uiType] as T;
                }
            }

            if(UIList.POPUP_START < uiType && uiType < UIList.POPUP_END)
            {
                if (ContainerPopup[uiType] == null)
                {
                    T loadedPrefab = Resources.Load<T>(UI_Resource_PATH + "UI." + uiType.ToString());
                    if (loadedPrefab != null)
                    {
                        result = Instantiate(loadedPrefab, popupRoot);
                        //result.gameObject.SetActive(false);
                        ContainerPopup[uiType] = result;
                    }
                }
                else
                {
                    result = ContainerPopup[uiType] as T;
                }
            }

            return result;
        }
    }
}
