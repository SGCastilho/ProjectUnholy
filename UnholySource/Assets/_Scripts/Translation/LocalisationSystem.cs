using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

namespace Core.Translation
{
    public sealed class LocalisationSystem
    {
        public enum Language
        {
            English,
            Brazilian
        }

        public static Language language = Language.English;

        private static Dictionary<string, string> localisedEN;
        private static Dictionary<string, string> localisedBR;

        public static bool isInit;

        public static CSVLoader csvLoader;

        public static void Init()
        {
            csvLoader = new CSVLoader();
            csvLoader.LoadCSV();

            UpdateDictionaries();

            isInit = true;
        }

        private static void UpdateDictionaries()
        {
            localisedEN = csvLoader.GetDictionaryValues("en");
            localisedBR = csvLoader.GetDictionaryValues("br");
        }
        
        #region Editor Function
        #if UNITY_EDITOR
        public static Dictionary<string, string> GetDictionaryForEditor()
        {
            if(!isInit){ Init(); }

            return localisedEN;
        }
        #endif
        #endregion

        /// <summary>
        /// Pega a tradução da key que colocarmos
        /// </summary>
        /// <param name="key">Key da localização</param>
        /// <returns></returns>
        public static string GetLocalisedValue(string key)
        {
            if(!isInit) { Init(); }

            string value = key;

            switch(language)
            {
                case Language.English:
                    localisedEN.TryGetValue(key, out value);
                    break;
                case Language.Brazilian:
                    localisedBR.TryGetValue(key, out value);
                    break;
            }

            return value;
        }

        #region Editor Function
        #if UNITY_EDITOR
        public static void Add(string key, string value)
        {
            if(value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            if(csvLoader == null)
            {
                csvLoader = new CSVLoader();
            }

            csvLoader.LoadCSV();
            csvLoader.Add(key, value);
            csvLoader.LoadCSV();

            UpdateDictionaries();
        }

        public static void Replace(string key, string value)
        {
            if(value.Contains("\""))
            {
                value.Replace('"', '\"');
            }

            if(csvLoader == null)
            {
                csvLoader = new CSVLoader();
            }

            csvLoader.LoadCSV();
            csvLoader.Edit(key, value);
            csvLoader.LoadCSV();

            UpdateDictionaries();
        }

        public static void Remove(string key)
        {
            if(csvLoader == null)
            {
                csvLoader = new CSVLoader();
            }

            csvLoader.LoadCSV();
            csvLoader.Remove(key);
            csvLoader.LoadCSV();

            UpdateDictionaries();
        }
        #endif
        #endregion
    }
}
