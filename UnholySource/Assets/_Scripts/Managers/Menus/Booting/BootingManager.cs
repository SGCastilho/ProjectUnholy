using Core.Translation;
using UnityEngine;

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
    }
}
