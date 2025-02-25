using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HAD
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private string currentLevelName;
        private HUDUI HUDUI;

        private List<Gate> gates = new List<Gate>();
        private List<LevelEntryPoint> entries = new List<LevelEntryPoint>();

        public void AddGate(Gate newGate)
        {
            gates.Add(newGate);
        }
        public List<Gate> GetGateList()
        {
            return gates;
        }
        public void AddEntryPoint(LevelEntryPoint entry)
        {
            entries.Add(entry);
        }
        public List<LevelEntryPoint> GetLevelEntryPoints()
        {
            return entries;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(currentLevelName))
            {
                LoadLevel("Entrance");
            }

            HUDUI = UIManager.Singleton.GetUI<HUDUI>(UIList.HUD);
            HUDUI.Show();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        [Button]
        public void LoadLevel(string levelName)
        {
            StartCoroutine(LoadLevelAsync(levelName));
        }

        IEnumerator LoadLevelAsync(string levelName)
        {
            if(CharacterController.Instance != null)
            {
                CharacterController.Instance.enabled = false;
                CharacterController.Instance.character.Move(Vector2.zero, Camera.main.transform.eulerAngles.y);
            }

            var FadeUI = UIManager.Singleton.GetUI<FadeInOutUI>(UIList.FadeInOut);
            if(!string.IsNullOrEmpty(currentLevelName))
            {
                FadeUI.FadeOut();
                yield return new WaitForSeconds(1);

                var unloadAsync = SceneManager.UnloadSceneAsync(currentLevelName);
                while (!unloadAsync.isDone)
                {
                    yield return null;
                }
                currentLevelName = string.Empty;
            }

            gates.Clear();
            entries.Clear();

            currentLevelName = levelName;
            var loadAsync = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            while (!loadAsync.isDone)
            {
                yield return null;
            }

            NavMeshSurfaceController.Instance.BakeNavMesh();

            PlayerCharacter.Instance.CharacterAttributeComponent.SetAttributeCurrentValue(AttributeTypes.MagicArrowCount, PlayerCharacter.Instance.CharacterAttributeComponent.GetAttribute(AttributeTypes.MagicArrowCount).MaxValue);

            var entryPoint = entries[0];

            CharacterController.Instance.character.Move(Vector2.zero, Camera.main.transform.eulerAngles.y);
            CharacterController.Instance.transform.position = entryPoint.transform.position;

            CharacterController.Instance.enabled = true;
            FadeUI.FadeIn();
            yield return new WaitForSeconds(1);

            var areaUI = UIManager.Singleton.GetUI<AreaAnnouncerUI>(UIList.AreaAnnouncer);
            areaUI.ShowAreaAnnouncerUI();
              
            if(currentLevelName == "Entrance")
            {
                foreach(Gate gate in gates)
                {
                    gate.ActivateGate();
                }
            }

            UserDataManager.Singleton.UpdateUserDataLast(currentLevelName, PlayerCharacter.Instance.CharacterAttributeComponent.GetAttribute(AttributeTypes.HealthPoint).CurrentValue);
        }
    }
}