using Core.Managers;
using Core.UI;
using UnityEngine;

namespace Core.Events
{
    public sealed class BootingEvents : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private BootingManager bootingManager;

        [Space(10)]

        [SerializeField] private UIBooting uIBooting;

        private void OnEnable() 
        {
            uIBooting.OnLoadMenu += bootingManager.LoadMainMenu;
            uIBooting.OnLanguageSelected += bootingManager.ChangeLanguage;

            uIBooting.OnSettingsSetted += bootingManager.SettingsSetted;

            bootingManager.OnLanguageChanged += uIBooting.WarningWindowFadeIn;
        }

        private void OnDisable() 
        {
            uIBooting.OnLoadMenu -= bootingManager.LoadMainMenu;
            uIBooting.OnLanguageSelected -= bootingManager.ChangeLanguage;

            uIBooting.OnSettingsSetted -= bootingManager.SettingsSetted;

            bootingManager.OnLanguageChanged -= uIBooting.WarningWindowFadeIn;
        }
    }
}
