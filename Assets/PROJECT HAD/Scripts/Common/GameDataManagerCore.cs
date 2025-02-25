using System;
using System.Collections.Generic;

namespace HAD
{
    public partial class GameDataManager : SingletonBase<GameDataManager>
    {
        public void Initialize()
        {
            // 예시
            string[] characterSampleDataText = CSVParserUtility.LoadData("CharacterSampleData");
            characterSampleData = DataParsing<CharacterSampleData, CharacterSampleData.CharacterSampleDataEntity>(characterSampleDataText);
        }

        public T DataParsing<T, K>(string[] line) where T : GameDataSource, new() where K : new()
        {
            T result = new T();
            List<K> dataGroup = new List<K>();
            var fields = typeof(K).GetFields();

            for (int i = 1; i < line.Length; i++)
            {
                if (string.IsNullOrEmpty(line[i]))
                    continue;

                string[] columnDatas = ConvertColumn(line[i]);

                K newEntity = new K();

                for (int j = 0; j < fields.Length; j++)
                {
                    string fieldName = fields[j].Name;
                    var field = newEntity.GetType().GetField(fieldName);

                    if (field != null)
                    {
                        object value = null;
                        Type fieldType = field.FieldType;

                        if (fieldType == typeof(int))
                        {
                            value = int.Parse(columnDatas[j]);
                        }
                        else if (fieldType == typeof(float))
                        {
                            value = float.Parse(columnDatas[j]);
                        }
                        else if (fieldType == typeof(bool))
                        {
                            value = bool.Parse(columnDatas[j]);
                        }
                        else if (fieldType == typeof(string))
                        {
                            value = columnDatas[j];
                        }

                        field.SetValue(newEntity, value);
                    }
                }
                dataGroup.Add(newEntity);
            }

            var resultField = result.GetType().GetField("DataGroup");
            resultField.SetValue(result, dataGroup);

            return result;
        }

        public string[] ConvertColumn(string line)
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