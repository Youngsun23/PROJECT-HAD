using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HAD
{
    public class MirrorUI : UIBase
    {
        // Level -> 변경
        [SerializeField] private Button Set1Button;
        // ToDo: 1~5개, 배열로 만들어서 for문 돌기
        [SerializeField] private TextMeshProUGUI Set1Price;
        [SerializeField] private TextMeshProUGUI Set1Increment;
        [SerializeField] private TextMeshProUGUI Set1Name;

        // 누른 버튼이 set?의 버튼인지 알아내는 방법을 하다가...
        // OnButton에서 호출
        public void UpgradeButton1()
        {
            // 일단 비용이 있는지를 체크해야지
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
