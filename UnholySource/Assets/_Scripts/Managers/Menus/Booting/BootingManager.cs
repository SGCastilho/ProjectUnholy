using System.IO;
using Core.Translation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Managers
{
    public sealed class BootingManager : MonoBehaviour
    {
        #region Events
        public delegate void LanguageChanged();
        public event LanguageChanged OnLanguageChanged;
        #endregion

        [Header("Classes")]
        [SerializeField] private UITextLocaliser warningText;

        [Header("Settings")]
        [SerializeField] private string mainMenuScene = "scene name here";

        private SettingsLoader _settingsLoader;

        private void OnEnable() 
        {
            _settingsLoader = FindObjectOfType<SettingsLoader>();

            if(SettingsSetted())
            {
                _settingsLoader.LoadSettings();
                
                ChangeLanguage(_settingsLoader.GetClientSavedLanguage());
            }
        }

        public void ChangeLanguage(int languageIndex)
        {
            switch(languageIndex)
            {
                case 0:
                    LocalisationSystem.language = LocalisationSystem.Language.English;
                    break;
                case 1:
                    LocalisationSystem.language = LocalisationSystem.Language.Brazilian;
                    break;
            }

            warningText.ReloadTranslation();

            OnLanguageChanged?.Invoke();

            if(!SettingsSetted())
            {
                _settingsLoader.CreateSettingsFile(languageIndex);
            }
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(mainMenuScene);
        }

        public bool SettingsSetted()
        {
            return File.Exists(Application.persistentDataPath + "/config.txt");
        }
    }
}
