using DG.Tweening;
using UnityEngine;

namespace Core.UI
{
    public sealed class UIMainMenu : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private CanvasGroup logoCanvasGroup;
        [SerializeField] private CanvasGroup buttonsCanvasGroup;

        [Header("Settings")]
        [SerializeField] [Range(0.1f, 1f)] private float fadeDuration = 0.4f;

        public void FadeIn()
        {
            logoCanvasGroup.DOKill();
            logoCanvasGroup.DOFade(1f, fadeDuration).OnComplete(() => 
            {
                buttonsCanvasGroup.DOKill();
                buttonsCanvasGroup.DOFade(1f, fadeDuration).OnComplete(() => 
                { 
                    buttonsCanvasGroup.interactable = true; 
                    buttonsCanvasGroup.blocksRaycasts = true;
                });
            });
        }
    }
}
