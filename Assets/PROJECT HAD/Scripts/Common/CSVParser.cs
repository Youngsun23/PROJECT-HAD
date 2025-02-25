using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class CSVParser : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<string, DialogueData> DialogueDataDictionary = new SerializableDictionary<string, DialogueData>();

        [SerializeField] private string csvFilePath = "DialogueData";

        void Start()
        {
            LoadCSV();
        }

        void LoadCSV()
        {
            TextAsset csvFile = Resources.Load<TextAsset>(csvFilePath);

            if (csvFile != null)
            {
                string[] lines = csvFile.text.Split('\n');

                foreach (string line in lines)
                {
                    string[] values = ParseCSVLine(line);
                    if (values.Length == 4)
                    {
                        string key = values[0];
                        string group = values[1];
                        string korean = values[2];
                        string english = values[3];

                        DialogueDataDictionary[key] = new DialogueData(key, group, korean, english);
                    }
                    else
                    {
                        string key = values[0];
                        string group = "-1";
                        string korean = "Null";
                        string english = "Null";

                        DialogueDataDictionary[key] = new DialogueData(key, group, korean, english);
                    }
                }
            }
            else
            {
                Debug.LogError($"CSV 파일을 찾을 수 없습니다: {csvFilePath}");
            }
        }

        public string GetKoreanText(string key)
        {
            if (DialogueDataDictionary.TryGetValue(key, out DialogueData data))
            {
                return data.Korean;
            }
            return null;
        }

        string[] ParseCSVLine(string line)
        {
            List<string> result = new List<string>();
            bool insideQuotes = false;
            string currentField = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"') 
                {
                    if (insideQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        currentField += '"';
                        i++;
                    }
                    else
                    {
                        insideQuotes = !insideQuotes;
                    }
                }
                else if (c == ',' && !insideQuotes)
                {
                    result.Add(currentField);
                    currentField = "";
                }
                else
                {
                    currentField += c;
                }
            }

            result.Add(currentField);
            return result.ToArray();
        }

    }
}
