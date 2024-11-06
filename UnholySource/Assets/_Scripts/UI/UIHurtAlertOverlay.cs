using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public sealed class UIHurtAlertOverlay : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private Image overlayImage;

        [Header("Settings")]
        [SerializeField] private Sprite midHealthOverlaySprite;
        [SerializeField] private Sprite lowHealthOverlaySprite;

        [Space(10)]

        [SerializeField] private int midHealthTrigger = 50;
        [SerializeField] private int lowHealthTrigger = 20;
        [SerializeField] [Range(0.1f, 1f)] private float fadeDuration = 0.4f;

        public void CheckAlertOverlay(ref int currentHealth)
        {
            if(currentHealth > midHealthTrigger)
            {
                HideOverlay();
                return;
            }

            if(currentHealth <= midHealthTrigger && currentHealth > lowHealthTrigger)
            {
                ShowOverlay(ref midHealthOverlaySprite);
                return;
            }

            if(currentHealth <= lowHealthTrigger)
            {
                ShowOverlay(ref lowHealthOverlaySprite);
                return;
            }
        }

        private void ShowOverlay(ref Sprite overlay)
        {
            overlayImage.sprite = overlay;

            if(overlayImage.color.a >= 1f) return;

            overlayImage.DOKill();
            overlayImage.DOFade(1f, fadeDuration);
        }

        private void HideOverlay()
        {
            if(overlayImage.color.a <= 0f) return;

            overlayImage.DOKill();
            overlayImage.DOFade(0f, fadeDuration);
        }
    }
}
