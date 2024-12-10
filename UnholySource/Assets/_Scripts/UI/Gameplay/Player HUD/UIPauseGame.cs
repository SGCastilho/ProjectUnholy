using DG.Tweening;
using UnityEngine;

namespace Core.UI
{
    public sealed class UIPauseGame : MonoBehaviour
    {
        #region Events
        public delegate void ShowInterface();
        public event ShowInterface OnShowInterface;

        public delegate void HideInteface();
        public event HideInteface OnHideInteface;
        #endregion

        [Header("Classes")]
        [SerializeField] private CanvasGroup backgroundCanvasGroup;
        [SerializeField] private CanvasGroup windowNameCanvasGroup;
        [SerializeField] private CanvasGroup buttonsCanvasGroup;
        
        [Header("Settings")]
        [SerializeField] private bool isPaused;
        [SerializeField] private bool canBeCalled = true;

        [Space(10)]

        [SerializeField] [Range(0.4f, 2f)] private float callingCouldown = 1.2f;

        [Space(20)]

        [SerializeField] [Range(0.1f, 1f)] private float fadeDuration = 0.2f;
        [SerializeField] [Range(0.1f, 1f)] private float delayDuration = 0.4f;

        [Space(6)]

        [SerializeField] private float buttonsWindowMovementY = 40f;

        private RectTransform buttonsRectTransform;

        private float _currentCouldown;
        private float _currentWindowPosistionY;
        private float _hiddedWindowPosistionY;

        private void Start() 
        {
            buttonsRectTransform = buttonsCanvasGroup.GetComponent<RectTransform>();

            _currentWindowPosistionY = buttonsRectTransform.anchoredPosition.y;

            _hiddedWindowPosistionY = buttonsRectTransform.anchoredPosition.y - buttonsWindowMovementY;

            buttonsRectTransform.anchoredPosition = _hiddedWindowPosistionY * Vector2.up;

            backgroundCanvasGroup.alpha = 0;
            windowNameCanvasGroup.alpha = 0;
            buttonsCanvasGroup.alpha = 0;

            canBeCalled = true;
        }
        
        private void Update() 
        {
            if(!canBeCalled)
            {
                _currentCouldown += Time.unscaledDeltaTime;
                if(_currentCouldown >= callingCouldown)
                {
                    canBeCalled = true;
                    _currentCouldown = 0f;
                }
            }
        }

        public void CallPause()
        {
            if(!canBeCalled) return;

            isPaused = !isPaused;
            canBeCalled = false;

            if(isPaused)
            {
                ShowWindow();
            }
            else
            {
                HideWindow();
            }
        }

        private void ShowWindow()
        {
            OnShowInterface?.Invoke();

            backgroundCanvasGroup.DOKill();
            backgroundCanvasGroup.DOFade(1f, fadeDuration).SetUpdate(true);

            windowNameCanvasGroup.DOKill();
            windowNameCanvasGroup.DOFade(1f, fadeDuration).SetDelay(delayDuration).SetUpdate(true);

            buttonsRectTransform.DOKill();
            buttonsRectTransform.DOAnchorPosY(_currentWindowPosistionY, fadeDuration).SetDelay(delayDuration).SetUpdate(true);

            buttonsCanvasGroup.DOKill();
            buttonsCanvasGroup.DOFade(1f, fadeDuration).SetDelay(delayDuration).SetUpdate(true);

            buttonsCanvasGroup.blocksRaycasts = true;
        }

        private void HideWindow()
        {
            OnHideInteface?.Invoke();

            buttonsCanvasGroup.blocksRaycasts = false;

            backgroundCanvasGroup.DOKill();
            backgroundCanvasGroup.DOFade(0f, fadeDuration);

            windowNameCanvasGroup.DOKill();
            windowNameCanvasGroup.DOFade(0f, fadeDuration);

            buttonsCanvasGroup.DOKill();
            buttonsCanvasGroup.DOFade(0f, fadeDuration).OnComplete(
                () => { buttonsRectTransform.anchoredPosition = _hiddedWindowPosistionY * Vector2.up; });
        }
    }
}
