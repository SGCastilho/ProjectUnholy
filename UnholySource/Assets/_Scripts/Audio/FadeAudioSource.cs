using DG.Tweening;
using UnityEngine;

namespace Core.Audio
{
    public sealed class FadeAudioSource : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private AudioSource audioSource;

        [Header("Settings")]
        [SerializeField] [Range(1f, 2f)] private float fadeDuration = 1.2f;

        public void FadeIn()
        {
            audioSource.DOKill();
            audioSource.DOFade(1f, fadeDuration);
        }

        public void FadeOut()
        {
            audioSource.DOKill();
            audioSource.DOFade(0f, fadeDuration);
        }
    }
}
