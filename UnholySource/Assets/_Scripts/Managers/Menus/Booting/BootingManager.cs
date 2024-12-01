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
        }
    }
}
