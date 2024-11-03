using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HAD
{
    public class TitleUI : MonoBehaviour
    {
        public void OnClickGameStartButton()
        { 
            StartCoroutine(ChangeSceneDelay());
        }

        IEnumerator ChangeSceneDelay()
        {
            var FadeUI = UIManager.Singleton.GetUI<FadeInOutUI>(UIList.FadeInOut);
            FadeUI.FadeOut();
            yield return new WaitForSeconds(2f);
            Main.Singleton.ChangeScene(SceneType.Ingame);
        }

        public void OnClickGameExitButton()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
