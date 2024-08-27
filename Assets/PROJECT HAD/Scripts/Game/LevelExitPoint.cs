//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace HAD
//{
//    public class LevelExitPoint : MonoBehaviour
//    {
//        [SerializeField] private string nextSceneName;

//        private void OnTriggerEnter(Collider other)
//        {
//            if (other.transform.root.TryGetComponent<PlayerCharacter>(out var player))
//            {
//                GameManager.Instance.LoadLevel(nextSceneName);
//            }
//        }
//    }
//}
