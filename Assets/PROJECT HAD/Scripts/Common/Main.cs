using System.Collections;
using UnityEngine.SceneManagement;

namespace HAD
{
    // 1. 프로젝트 초기화
    // 2. 씬 관리/전환

    public enum SceneType
    {
        None,
        Title,
        Ingame,
    }

    public class Main : SingletonBase<Main>
    {
        public SceneType currentSceneType = SceneType.None;

        protected override void Awake()
        {
            base.Awake();

            Initialize();
        }

        public void Initialize()
        {
            //// 초기화 -> BootStrapper로 이동
            //// GameDataManager
            //// GameDataModel
            //// UIManager
            //UIManager.Singleton.Initialize();
            //// ...

            ChangeScene(SceneType.Title);
        }

        public void ChangeScene(SceneType sceneType)
        {
            if (currentSceneType == sceneType)
                return;

            switch(sceneType)
            {
                case SceneType.Title:
                    StartCoroutine(ChangSceneCoroutine(SceneType.Title));
                    break;
                case SceneType.Ingame:
                    StartCoroutine(ChangSceneCoroutine(SceneType.Ingame));
                    break;
            }
        }

        IEnumerator ChangSceneCoroutine(SceneType sceneType)
        {
            var loadSceneAsync = SceneManager.LoadSceneAsync(sceneType.ToString());
            float progress = 0f;
            while(!loadSceneAsync.isDone)
            {
                progress = loadSceneAsync.progress / 0.9f;
                // Debug.Log($"Loading Progres: {progress:0.0}");
                // ToDo: Log -> Loading UI로 변경

                yield return null;
            }
        }
    }
}

