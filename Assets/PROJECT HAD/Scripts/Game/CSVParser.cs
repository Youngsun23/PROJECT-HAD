using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace HAD
{
    public class CSVParser : MonoBehaviour
    {
        private Dictionary<string, DialogueData> DialogueDataDictionary = new Dictionary<string, DialogueData>();

        [SerializeField] private string csvFilePath = "DialogueData"; // CSV 파일 경로

        void Start()
        {
            LoadCSV();
        }

        void LoadCSV()
        {
            // Resources 폴더에서 CSV 파일을 TextAsset으로 로드
            TextAsset csvFile = Resources.Load<TextAsset>(csvFilePath); // 확장자는 생략

            if (csvFile != null)
            {
                // TextAsset의 텍스트 내용을 줄 단위로 나눔
                string[] lines = csvFile.text.Split('\n');
                // Debug.Log(lines[1]);

                foreach (string line in lines)
                {
                    string[] values = ParseCSVLine(line); // 쉼표와 따옴표를 처리하는 함수
                    if (values.Length == 4)
                    {
                        string key = values[0];
                        string group = values[1];
                        string korean = values[2];
                        string english = values[3];

                        // Dictionary에 데이터를 저장
                        DialogueDataDictionary[key] = new DialogueData(key, group, korean, english);
                    }
                }
            }
            else
            {
                Debug.LogError($"CSV 파일을 찾을 수 없습니다: {csvFilePath}");
            }
        }


        // Key -> Korean 호출
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

                if (c == '"') // 따옴표를 만나면 (텍스트 내용에 쉼표가 포함된 경우 csv 저장 중 자동으로 큰따옴표 감싸기 수행함)
                {
                    if (insideQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        // 이중 따옴표 ""는 필드 내에서 하나의 따옴표로 간주
                        currentField += '"';
                        i++;
                    }
                    else
                    {
                        insideQuotes = !insideQuotes;
                    }
                }
                else if (c == ',' && !insideQuotes) // 따옴표 밖에서 쉼표를 만나면 필드 구분자로 처리
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
