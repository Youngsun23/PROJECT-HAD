using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HAD
{
    public class AreaAnnouncerUI : UIBase
    {
        [SerializeField] private TextMeshProUGUI AreaUIText;
        [SerializeField] private Image AreaUIBackground;

        public void ShowAreaAnnouncerUI()
        {
            Show();
            AreaUIBackground.gameObject.SetActive(false);
            StartCoroutine(SwitchAreaAnnouncerUI());
        }

        IEnumerator SwitchAreaAnnouncerUI()
        {
            yield return new WaitForSeconds(1f);

            AreaUIBackground.gameObject.SetActive(true);
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name != "Ingame")
                {
                    AreaUIText.text = scene.name;
                }
            }

            yield return new WaitForSeconds(3f);
            AreaUIText.text = "";

            Hide();
        }
    }
}
