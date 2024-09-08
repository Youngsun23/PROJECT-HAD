using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace HAD
{
    public class AreaAnnouncerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI AreaUIText;
        [SerializeField] private Image AreaUIBackground;

        public static AreaAnnouncerUI Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void ShowAreaAnnouncerUI(string text)
        {
            StartCoroutine(SwitchAreaAnnouncerUI(text));
        }

        IEnumerator SwitchAreaAnnouncerUI(string text)
        {
            yield return new WaitForSeconds(1f);

            // todo: 연출 처리
            AreaUIText.text = text;
            AreaUIBackground.gameObject.SetActive(true);
            AreaUIText.gameObject.SetActive(true);

            yield return new WaitForSeconds(3f);

            AreaUIBackground.gameObject.SetActive(false);
            AreaUIText.gameObject.SetActive(false);
        }
    }
}

// 이후, 지역 입장 외에도 같은 UI 사용...
// 탈출은 없다(사망) / 퓨리 격파(1챕 보스 클리어) / 새 무기 잠금 해제됨(심장을 노리는 활 코로나크트)
