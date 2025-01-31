using TMPro;
using DG.Tweening;
using UnityEngine;

namespace Core.UI
{
    public sealed class UIBooting : MonoBehaviour
    {
        #region Events
        public delegate bool SettingsSetted();
        public event SettingsSetted OnSettingsSetted;

        public delegate void LanguageSelected(int languageIndex);
        public event LanguageSelected OnLanguageSelected;

        public delegate void LoadMenu();
        public event LoadMenu OnLoadMenu;
        #endregion

        [Header("Classes")]
        [SerializeField] private CanvasGroup languageCanvasGroup;
        [SerializeField] private CanvasGroup warningCanvasGroup;
        [SerializeField] private CanvasGroup logoCanvasGroup;

        [Space(6)]

        [SerializeField] private RectTransform languageWindow;

        [Space(6)]

        [SerializeField] private TMP_Dropdown languageDropdown;

        [Header("Settings")]
        [SerializeField] private float windowMovement = 60f;
        [SerializeField] [Range(0.2f, 1f)] private float fadeDuration = 0.6f;

        [Space(6)]

        [SerializeField] [Range(6f, 20f)] private float warningWindowDuration = 14f;
        
        private bool _showLogo;
        private bool _warningWindow;
        private float _currentWarningWindowDuration;

        private void OnEnable() 
        {
            languageWindow.localPosition = new Vector2(0f, -windowMovement);

            languageCanvasGroup.alpha = 0f;
        }

        private void Start() 
        {
            if(OnSettingsSetted())
            {
                WarningWindowFadeIn();
            }
            else
            {
                LanguageFadeIn();
            }
        }

        private void Update() 
        {
            if(_warningWindow)
            {
                _currentWarningWindowDuration += Time.deltaTime;
                if(_currentWarningWindowDuration >= warningWindowDuration)
                {
                    _warningWindow = false;
                    _currentWarningWindowDuration = 0f;

                    WarningWindowFadeOut();
                }
            }

            if(_showLogo)
            {
                _currentWarningWindowDuration += Time.deltaTime;
                if(_currentWarningWindowDuration >= warningWindowDuration)
                {
                    _showLogo = false;
                    _currentWarningWindowDuration = 0f;

                    LogoWindowFadeOut();
                }
            }
        }

        public void ConfirmButton()
        {
            OnLanguageSelected?.Invoke(languageDropdown.value);

            LanguageFadeOut();
        }

        public void LogoWindowFadeIn()
        {
            logoCanvasGroup.DOKill();
            logoCanvasGroup.DOFade(1f, fadeDuration).SetDelay(1f);

            _showLogo = true;
        }

        public void LogoWindowFadeOut()
        {
            logoCanvasGroup.DOKill();
            logoCanvasGroup.DOFade(0f, fadeDuration).OnComplete(() => { OnLoadMenu?.Invoke(); });
        }


        public void WarningWindowFadeIn()
        {
            warningCanvasGroup.DOKill();
            warningCanvasGroup.DOFade(1f, fadeDuration).SetDelay(1f);

            _warningWindow = true;
        }

        public void WarningWindowFadeOut()
        {
            warningCanvasGroup.DOKill();
            warningCanvasGroup.DOFade(0f, fadeDuration).OnComplete(() => { LogoWindowFadeIn(); });
        }

        private void LanguageFadeIn()
        {
            languageWindow.DOKill();
            languageWindow.DOLocalMoveY(0f, fadeDuration).SetDelay(0.4f);

            languageCanvasGroup.DOKill();
            languageCanvasGroup.DOFade(1f, fadeDuration).SetDelay(0.4f);
        }

        private void LanguageFadeOut()
        {
            languageCanvasGroup.interactable = false;
            languageCanvasGroup.blocksRaycasts = false;

            languageWindow.DOKill();
            languageWindow.DOLocalMoveY(-windowMovement, fadeDuration);

            languageCanvasGroup.DOKill();
            languageCanvasGroup.DOFade(0f, fadeDuration);
        }
    }
}
