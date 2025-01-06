using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Core.UI
{
    public sealed class UIPressAnyButton : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space(10)]

        [SerializeField] private TextMeshProUGUI pressAnyButtonTMP;

        [Header("Settings")]
        [SerializeField] private float fadeInDelay = 1.4f;

        [Space(10)]

        [SerializeField] [Range(0.1f , 2f)] private float fadeDuration = 0.4f;
        [SerializeField] [Range(0.4f, 2f)] private float fadeInDuration = 1.4f;

        [Space(10)]

        [SerializeField] [Range(0.1f , 1f)] private float pressAnyButtonFlashDuration = 0.8f;

        [Header("Unity Events")]

        [Space(10)]

        [SerializeField] private UnityEvent OnFadeOutStarted;
        [SerializeField] private UnityEvent OnFadeOutEnd;

        private bool _enableToFadeOut;
        private bool _pressedAnyButton;

        private void Start() => FadeIn();

        private void FadeIn()
        {
            pressAnyButtonTMP.gameObject.SetActive(false); 

            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, fadeInDuration).SetDelay(fadeInDelay).OnComplete(
                () => 
                {
                    pressAnyButtonTMP.gameObject.SetActive(true); 
                    pressAnyButtonTMP.DOFade(0.2f, pressAnyButtonFlashDuration).SetLoops(-1, LoopType.Yoyo);

                    _enableToFadeOut = true; 
                });
        }

        public void FadeOut()
        {
            if(_pressedAnyButton || !_enableToFadeOut) return;

            _pressedAnyButton = true;
            
            OnFadeOutStarted?.Invoke();   

            pressAnyButtonTMP.DOKill();

            pressAnyButtonTMP.alpha = 1f;

            pressAnyButtonTMP.DOFade(0f, 0.2f).OnComplete(() => 
            {
                canvasGroup.DOKill();
                canvasGroup.DOFade(0f, fadeDuration).OnComplete(() => 
                { 
                    canvasGroup.interactable = true; 
                    canvasGroup.blocksRaycasts = true;

                    OnFadeOutEnd?.Invoke(); 
                }); 
            });
        }
    }
}
