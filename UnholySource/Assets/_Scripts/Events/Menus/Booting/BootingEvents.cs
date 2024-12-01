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
            uIBooting.OnLanguageSelected += bootingManager.ChangeLanguage;

            bootingManager.OnLanguageChanged += uIBooting.WarningWindowFadeIn;
        }

        private void OnDisable() 
        {
            uIBooting.OnLanguageSelected -= bootingManager.ChangeLanguage;

            bootingManager.OnLanguageChanged -= uIBooting.WarningWindowFadeIn;
        }
    }
}
