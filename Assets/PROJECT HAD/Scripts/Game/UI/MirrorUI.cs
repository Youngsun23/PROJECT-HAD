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

        // 누른 버튼이 set?의 버튼인지 알아내는 방법을 하다가...
        // OnButton에서 호출
        public void UpgradeButton1()
        {
            bool noMoreButton = UserDataManager.Singleton.UpdateUserDataMirror(1);
            if(noMoreButton)
                Set1Button.interactable = false;
            UpdateMirrorUI();
        }

        public void ExitButton()
        {
            UIManager.Hide<MirrorUI>(UIList.Mirror);
        }

        private void Awake()
        {
            UpdateMirrorUI();
        }
        private void UpdateMirrorUI()
        {
            int currentLevel = UserDataManager.Singleton.UserData.mirrorDic[1];
            int currentPrice = GameDataModel.Singleton.GetMirrorGameData(1).PriceAtLevel[currentLevel];
            int currentStat = GameDataModel.Singleton.GetMirrorGameData(1).IncreamentAtLevel[currentLevel];
            Set1Price.text = currentPrice.ToString();
            Set1Increment.text = currentStat.ToString();
        }
    }
}
