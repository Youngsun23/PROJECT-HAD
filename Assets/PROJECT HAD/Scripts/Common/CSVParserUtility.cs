using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAD
{
    public class CSVParserUtility
    {
        public static string[] LoadData(string path)
        {
            // Resources 폴더에서 CSV 파일을 TextAsset으로 로드
            TextAsset csvFile = Resources.Load<TextAsset>(path); // 확장자는 생략
            if (csvFile != null)
            {
                string[] lines = csvFile.text.Split('\n');
                return lines;
            }

            return null;
        }
    }
}