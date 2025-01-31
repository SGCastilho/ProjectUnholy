using System;
using DG.Tweening;
using UnityEngine;

namespace Core.UI
{
    public sealed class UIGameOver : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Settings")]
        [SerializeField] private float fadeInDelayDuration = 1f;
        [SerializeField] [Range(0.1f, 6f)] private float fadeDuration = 4f;

        private void OnEnable() 
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }

        private void Start() => FadeIn();

        public void FadeIn()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, fadeDuration).OnComplete(() => { canvasGroup.blocksRaycasts = true; }).SetDelay(fadeInDelayDuration);
        }

        public void FadeOut()
        {
            canvasGroup.blocksRaycasts = false;

            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeDuration);
        }

        public void FadeOut(Action ExecuteOnComplete)
        {
            canvasGroup.blocksRaycasts = false;

            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeDuration).OnComplete(() => { ExecuteOnComplete?.Invoke(); });
        }
    }
}
