using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Core.Translation
{
    public sealed class CSVLoader
    {
        private TextAsset _csvFile; //Arquivo CSV que vamos carregar
        
        private char _lineSeperator = '\n'; //A referencia que iremos usar para separar as linhas
        private char _surround = '"'; //O charactere que sera usado para sabermos o valor da chave

        private string[] _fieldSeperator = {"\",\""};

        /// <summary>
        /// Carrega nosso CSV na pasta Resources
        /// </summary>
        public void LoadCSV()
        {
            _csvFile = Resources.Load<TextAsset>("Localisation/localisation");
        }

        /// <summary>
        /// Quando essa função for chamada, ela irá retornar os valores da linguagem que escolhemos
        /// </summary>
        public Dictionary<string, string> GetDictionaryValues(string attributeId)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string[] lines = _csvFile.text.Split(_lineSeperator);

            int attributeIndex = -1;

            string[] headers = lines[0].Split(_fieldSeperator, System.StringSplitOptions.None);

            for(int i = 0; i<headers.Length; i++)
            {
                if(headers[i].Contains(attributeId))
                {
                    attributeIndex = i;
                    break;
                }
            }

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            for(int i = 1; i<lines.Length; i++)
            {
                string line = lines[i];

                string[] fields = CSVParser.Split(line);

                for(int f = 0; f<fields.Length; f++)
                {
                    fields[f] = fields[f].TrimStart(' ', _surround);
                    fields[f] = fields[f].Replace("\"","");
                }

                if(fields.Length > attributeIndex)
                {
                    var key = fields[0];

                    if(dictionary.ContainsKey(key)) { continue; }

                    var value = fields[attributeIndex];

                    dictionary.Add(key, value);
                }
            }

            return dictionary;
        }

        #region Editor Functions
        #if UNITY_EDITOR
        public void Add(string key, string value)
        {
            string appended = string.Format("\n\"{0}\",\"{1}\",\"\"", key, value);
            File.AppendAllText("Assets/Resources/Localisation/localisation.csv", appended);

            UnityEditor.AssetDatabase.Refresh();
        }

        public void Remove(string key)
        {
            string[] lines = _csvFile.text.Split(_lineSeperator);

            string[] keys = new string[lines.Length];

            for(int i = 0; i<lines.Length; i++)
            {
                string line = lines[i];

                keys[i] = line.Split(_fieldSeperator, System.StringSplitOptions.None)[0];
            }

            int index = -1;

            for(int i = 0; i<keys.Length; i++)
            {
                if(keys[i].Contains(key))
                {
                    index = i;
                    break;
                }
            }

            if(index > -1)
            {
                string[] newLines;
                newLines = lines.Where(w => w != lines[index]).ToArray();

                string replaced = string.Join(_lineSeperator.ToString(), newLines);
                File.WriteAllText("Assets/Resources/Localisation/localisation.csv", replaced);
            }
        }

        public void Edit(string key, string value)
        {
            Remove(key);
            Add(key, value);
        }
        #endif
        #endregion
    }
}
