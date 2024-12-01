using System.Collections.Generic;
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

        public static Language language = Language.Brazilian;

        private static Dictionary<string, string> localisedEN;
        private static Dictionary<string, string> localisedBR;

        public static bool isInit;

        public static void Init()
        {
            CSVLoader csvLoader = new CSVLoader();
            csvLoader.LoadCSV();

            localisedEN = csvLoader.GetDictionaryValues("en");
            localisedBR = csvLoader.GetDictionaryValues("br");

            isInit = true;
        }

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
    }
}
