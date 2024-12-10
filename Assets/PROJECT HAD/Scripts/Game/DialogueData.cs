namespace HAD
{
    public class DialogueData
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
