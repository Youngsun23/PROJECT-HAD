using UnityEngine;

// @ 임시 스크립트 @

namespace HAD
{
    public class LevelEntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.Instance.AddEntryPoint(this);
        }
    }
}
