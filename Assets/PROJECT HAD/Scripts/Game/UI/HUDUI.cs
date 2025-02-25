using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HAD
{
    public class HUDUI : UIBase
    {
        [SerializeField] private Image HPBar;
        [SerializeField] private TextMeshProUGUI HPTXT;
        [SerializeField] private TextMeshProUGUI MagicTXT;
        [SerializeField] private TextMeshProUGUI CoinTXT;
        [SerializeField] private TextMeshProUGUI DarknessTXT;

        public static HUDUI Instance => UIManager.Singleton.GetUI<HUDUI>(UIList.HUD);

        public void UpdateHUDUI()
        {
            
        }

        public void UpdateHUDUIHP(float maxHP, float curHP) // cur/max
        {
            float targetFill = curHP / maxHP;
            targetFill = Mathf.Clamp01(targetFill);
            StartCoroutine(FillHPBar(targetFill));
            HPTXT.text = curHP.ToString() + " / " + maxHP.ToString();
        }

        private IEnumerator FillHPBar(float targetFill)
        {
            float duration = 2f; 
            float elapsed = 0f;    
            float startFill = HPBar.fillAmount;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration; 
                HPBar.fillAmount = Mathf.Lerp(startFill, targetFill, t);
                yield return null;
            }

            HPBar.fillAmount = targetFill;
        }

        public void UpdateHUDUIMagic(float maxArrow, float curArrow)
        {
            MagicTXT.text = curArrow.ToString() + " / " + maxArrow.ToString();
        }

        public void UpdateHUDUICoin(float value)
        {
            CoinTXT.text = value.ToString();
        }

        public void UpdateHUDUIDarkness(float value)
        {
            DarknessTXT.text = value.ToString();
        }
    }
}
