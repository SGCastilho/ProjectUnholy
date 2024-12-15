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
        [SerializeField] private ScriptableObjectLoader scriptableObjectLoader;

        [Header("Settings")]
        [SerializeField] private string mainMenuScene = "scene name here";

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

            scriptableObjectLoader = new ScriptableObjectLoader();

            scriptableObjectLoader.Translate();

            OnLanguageChanged?.Invoke();
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}
