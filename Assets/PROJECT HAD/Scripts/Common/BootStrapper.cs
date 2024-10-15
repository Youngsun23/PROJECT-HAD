using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace HAD
{
    public class BootStrapper : MonoBehaviour
    {
        private static List<string> AutoBootStrappedScenes = new List<string>()
        {
            "Main",
            "Ingame",
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void SystemBoot()
        {
#if UNITY_EDITOR            
            var activeScene = EditorSceneManager.GetActiveScene();
            for (int i = 0; i < AutoBootStrappedScenes.Count; i++)
            {
                if (activeScene.name.Equals(AutoBootStrappedScenes[i]))
                {
                    InternalBoot();
                }
            }
#else
            InternalBoot();
#endif
        }

        private static void InternalBoot()
        {
            UIManager.Singleton.Initialize();
        }
    }
}