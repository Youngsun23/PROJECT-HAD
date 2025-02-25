using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HAD
{
    public class MirrorUI : UIBase
    {
        [SerializeField] private Button Set1Button;
        [SerializeField] private TextMeshProUGUI Set1Price;
        [SerializeField] private TextMeshProUGUI Set1Increment;
        [SerializeField] private TextMeshProUGUI Set1Name;

        public void UpgradeButton1()
        {
            int currentLevel = UserDataManager.Singleton.UserData.mirrorDic[1] + 1;
            int currentPrice = GameDataModel.Singleton.GetMirrorGameData(1).PriceAtLevel[currentLevel];
            if (UserDataManager.Singleton.UserData.darkness < currentPrice)
                return;
            UserDataManager.Singleton.UpdateUserDataResources("Darkness", -currentPrice);

            bool noMoreButton = UserDataManager.Singleton.UpdateUserDataMirror(1);
            if(noMoreButton)
                Set1Button.interactable = false;
            UpdateMirrorUI();
        }

        public void ExitButton()
        {
            UIManager.Hide<MirrorUI>(UIList.Mirror);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                ExitButton();
            }
        }

        private void OnEnable()
        {
            UpdateMirrorUI();
        }
        private void UpdateMirrorUI()
        {
            int currentLevel = UserDataManager.Singleton.UserData.mirrorDic[1] + 1;
            int currentPrice = GameDataModel.Singleton.GetMirrorGameData(1).PriceAtLevel[currentLevel];
            int currentStat = GameDataModel.Singleton.GetMirrorGameData(1).IncreamentAtLevel[currentLevel];
            string currentName = GameDataModel.Singleton.GetMirrorGameData(1).Name;
            Set1Price.text = currentPrice.ToString();
            Set1Increment.text = currentStat.ToString();
            Set1Name.text = currentName;
            if (UserDataManager.Singleton.UserData.darkness < currentPrice)
                Set1Price.color = Color.red;
            else Set1Price.color = Color.black;
        }
    }
}
