using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HAD
{
    public class TitleUI : MonoBehaviour
    {
        public void OnClickGameStartButton()
        {
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
