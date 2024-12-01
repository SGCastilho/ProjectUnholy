using TMPro;
using DG.Tweening;
using UnityEngine;

namespace Core.UI
{
    public sealed class UIBooting : MonoBehaviour
    {
        #region Events
        public delegate void LanguageSelected(int languageIndex);
        public event LanguageSelected OnLanguageSelected;
        #endregion

        [Header("Classes")]
        [SerializeField] private CanvasGroup languageCanvasGroup;
        [SerializeField] private CanvasGroup warningCanvasGroup;

        [Space(6)]

        [SerializeField] private RectTransform languageWindow;

        [Space(6)]

        [SerializeField] private TMP_Dropdown languageDropdown;

        [Header("Settings")]
        [SerializeField] private float windowMovement = 60f;
        [SerializeField] [Range(0.2f, 1f)] private float fadeDuration = 0.6f;

        [Space(6)]

        [SerializeField] [Range(14f, 20f)] private float warningWindowDuration = 14f;
        
        private bool _warningWindow;
        private float _currentWarningWindowDuration;

        private void OnEnable() 
        {
            languageWindow.localPosition = new Vector2(0f, -windowMovement);

            languageCanvasGroup.alpha = 0f;
        }

        private void Start() 
        {
            LanguageFadeIn();
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
        }

        public void ConfirmButton()
        {
            OnLanguageSelected?.Invoke(languageDropdown.value);

            LanguageFadeOut();
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
            warningCanvasGroup.DOFade(0f, fadeDuration);

            //Carregar menu principal quando animação terminar
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
