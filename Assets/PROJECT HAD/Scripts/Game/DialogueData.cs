using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class DialogueData : MonoBehaviour
    {
        public string Key;
        public string Group;
        public string Korean;
        public string English;

        public DialogueData(string key, string group, string korean, string english)
        {
            Key = key;
            Group = group;
            Korean = korean;
            English = english;
        }
    }
}
