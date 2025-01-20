using System;
using DG.Tweening;
using UnityEngine;

namespace Core.UI
{
    public sealed class UIFadeController : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Settings")]
        [SerializeField] [Range(0.1f, 0.6f)] private float fadeDuration = 0.2f;

        private void OnEnable() 
        {
            if(canvasGroup == null) return;

            if(canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha = 1f;
            }
        }

        public void FadeOut()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeDuration).SetUpdate(true);
        }

        public void CustomFadeOut(float customDuration, float delay, Action OnFadeOutEnd)
        {
            if(customDuration <= 0f) return;

            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, customDuration).SetUpdate(true).SetDelay(delay).OnComplete(()=> { OnFadeOutEnd?.Invoke(); });
        }

        public void FadeIn()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, fadeDuration).SetUpdate(true);
        }

        public void CustomFadeIn(float customDuration, float delay, Action OnFadeOutEnd)
        {
            if(customDuration <= 0f) return;

            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, customDuration).SetUpdate(true).OnComplete(()=> { OnFadeOutEnd?.Invoke(); });
        }
    }
}
