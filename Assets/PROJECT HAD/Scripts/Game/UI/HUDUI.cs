using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HAD
{
    public class HUDUI : UIBase
    {
        // 체력바 Fill
        // 마법TXT cur/max
        // 돈TXT cur
        // 어둠TXT cur
        [SerializeField] private Image HPBar;
        [SerializeField] private TextMeshProUGUI HPTXT;
        [SerializeField] private TextMeshProUGUI MagicTXT;
        [SerializeField] private TextMeshProUGUI CoinTXT;
        [SerializeField] private TextMeshProUGUI DarknessTXT;
        // 관련 유저데이터 갱신 -> UI도 갱신 or 그냥 Initalize될 때, 전체 쫙 업데이트?

        public void UpdateHUDUI()
        {
            
        }

        public void UpdateHUDUIHP() // cur/max
        {

            // 체력바 Fill
            // 체력TXT cur/max
            float maxHP = PlayerCharacter.Instance.CharacterAttributeComponent.GetAttribute(AttributeTypes.HealthPoint).MaxValue;
            float curHP = PlayerCharacter.Instance.CharacterAttributeComponent.GetAttribute(AttributeTypes.HealthPoint).CurrentValue;
            float targetFill = curHP / maxHP;
            targetFill = Mathf.Clamp01(targetFill);
            StartCoroutine(FillHPBar(targetFill));
            HPTXT.text = curHP.ToString() + " / " + maxHP.ToString();
        }

        private IEnumerator FillHPBar(float targetFill)
        {
            float duration = 2f; // 애니메이션 지속 시간
            float elapsed = 0f;    // 경과 시간
            float startFill = HPBar.fillAmount;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration; // 0에서 1로 증가
                HPBar.fillAmount = Mathf.Lerp(startFill, targetFill, t);
                yield return null;
            }

            // 마지막 값 보정
            HPBar.fillAmount = targetFill;
        }

        public void UpdateHUDUIMagic()
        {
            float maxArrow = PlayerCharacter.Instance.CharacterAttributeComponent.GetAttribute(AttributeTypes.MagicArrowCount).MaxValue;
            float curArrow = PlayerCharacter.Instance.CharacterAttributeComponent.GetAttribute(AttributeTypes.MagicArrowCount).CurrentValue;
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
