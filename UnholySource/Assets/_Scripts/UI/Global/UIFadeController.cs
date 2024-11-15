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

        private void Start() => FadeOut();

        public void FadeOut()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeDuration).SetUpdate(true);
        }

        public void FadeOut(int customDuration)
        {
            if(customDuration <= 0f) return;

            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, customDuration).SetUpdate(true);
        }

        public void FadeIn()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, fadeDuration).SetUpdate(true);
        }

        public void FadeIn(int customDuration)
        {
            if(customDuration <= 0f) return;

            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, customDuration).SetUpdate(true);
        }
    }
}
