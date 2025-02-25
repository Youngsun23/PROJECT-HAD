using UnityEngine;

namespace HAD
{
    public class CSVParserUtility
    {
        public static string[] LoadData(string path)
        {
            TextAsset csvFile = Resources.Load<TextAsset>(path); 
            if (csvFile != null)
            {
                string[] lines = csvFile.text.Split('\n');
                return lines;
            }

            return null;
        }
    }
}