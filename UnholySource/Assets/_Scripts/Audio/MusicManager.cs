using DG.Tweening;
using UnityEngine;

namespace Core.Audio
{
    public sealed class MusicManager : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private AudioSource audioSource;

        [Header("Settings")]
        [SerializeField] private bool inTransition;
        [SerializeField] private bool predatorChasing;
        [SerializeField] [Range(0.1f, 4f)] private float fadeDuration = 1f;

        [Space(10)]

        [SerializeField] private AudioClip predatorChasingClip;
        [SerializeField] private AudioClip predatorSearchingClip;

        private float _maxVolume;

        private void OnEnable() 
        {
            if(audioSource == null) return;

            _maxVolume = audioSource.volume;
        }

        public void PlayAudio(AudioClip musicAudio)
        {
            if(inTransition || predatorChasing || audioSource.clip == musicAudio) return;

            audioSource.clip = musicAudio;
            audioSource.volume = _maxVolume;

            audioSource.Play();
        }

        public void PlayAudioFadeIn(AudioClip musicAudio)
        {
            if(inTransition || predatorChasing || audioSource.clip == musicAudio) return;

            audioSource.clip = musicAudio;
            audioSource.volume = 0f;

            audioSource.Play();

            audioSource.DOKill();

            inTransition = true;

            audioSource.DOFade(_maxVolume, fadeDuration).OnComplete(() => { inTransition = false; });
        }

        public void TransitateTo(AudioClip musicAudio)
        {
            if(inTransition || predatorChasing) return;

            audioSource.DOKill();

            inTransition = true;

            audioSource.DOFade(0f, fadeDuration).OnComplete(() => 
            {
                audioSource.Stop();

                audioSource.clip = musicAudio;
                audioSource.volume = 0f;

                audioSource.Play();

                audioSource.DOKill();
                audioSource.DOFade(_maxVolume, fadeDuration).OnComplete(() => { inTransition = false; });
            }).SetUpdate(true);
        }

        public void StopAudio()
        {
            if(inTransition || predatorChasing) return;

            audioSource.Stop();

            audioSource.clip = null;
        }

        public void StopAudioFadeOut()
        {
            if(inTransition || predatorChasing) return;

            audioSource.DOKill();

            inTransition = true;

            audioSource.DOFade(0f, fadeDuration).OnComplete(() => 
            {
                audioSource.Stop();
                
                inTransition = false;

                audioSource.clip = null;
            }).SetUpdate(true);
        }

        public void PlayPredatorChasing()
        {
            if(predatorChasing) return;

            predatorChasing = true;

            audioSource.Stop();
            audioSource.clip = null;

            audioSource.clip = predatorChasingClip;
            audioSource.volume = _maxVolume;

            audioSource.Play();
        }

        public void PlayPredatorSearching()
        {
            audioSource.DOKill();

            inTransition = true;

            audioSource.DOFade(0f, fadeDuration).OnComplete(() => 
            {
                audioSource.Stop();

                audioSource.clip = predatorSearchingClip;
                audioSource.volume = 0f;

                audioSource.Play();

                audioSource.DOKill();
                audioSource.DOFade(_maxVolume, fadeDuration).OnComplete(() => { inTransition = false; });
            }).SetUpdate(true);
        }

        public void StopPredatorMusic()
        {
            audioSource.Stop();
            audioSource.clip = null;

            predatorChasing = false;
        }
    }
}
