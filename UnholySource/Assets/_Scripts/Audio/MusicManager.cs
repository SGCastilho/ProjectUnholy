using DG.Tweening;
using UnityEngine;

namespace Core.Audio
{
    public sealed class MusicManager : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private AudioSource audioSource;

        [Header("Settings")]
        [SerializeField] private bool blockMusic;
        [SerializeField] private bool inTransition;
        [SerializeField] private bool isChasing;
        [SerializeField] [Range(0.1f, 4f)] private float fadeDuration = 1f;

        [Space(10)]

        [SerializeField] private AudioClip chasingClip;
        [SerializeField] private float _chasingVolume;

        private float _maxVolume;

        private void OnEnable() 
        {
            if(audioSource == null) return;

            _maxVolume = audioSource.volume;
        }

        public void PlayAudio(AudioClip musicAudio)
        {
            if(blockMusic || inTransition || isChasing || audioSource.clip == musicAudio) return;

            audioSource.clip = musicAudio;
            audioSource.volume = _maxVolume;

            audioSource.Play();
        }

        public void PlayAudioFadeIn(AudioClip musicAudio)
        {
            if(blockMusic || inTransition || isChasing || audioSource.clip == musicAudio) return;

            audioSource.clip = musicAudio;
            audioSource.volume = 0f;

            audioSource.Play();

            audioSource.DOKill();

            inTransition = true;

            audioSource.DOFade(_maxVolume, fadeDuration).OnComplete(() => { inTransition = false; });
        }

        public void TransitateTo(AudioClip musicAudio)
        {
            if(blockMusic || inTransition || isChasing) return;

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
            if(inTransition || isChasing) return;

            audioSource.Stop();

            audioSource.clip = null;
        }

        public void StopAudioFadeOut()
        {
            if(inTransition || isChasing) return;

            audioSource.DOKill();

            inTransition = true;

            audioSource.DOFade(0f, fadeDuration).OnComplete(() => 
            {
                audioSource.Stop();
                
                inTransition = false;

                audioSource.clip = null;
            }).SetUpdate(true);
        }

        public void PlayerChasing()
        {
            if(isChasing) return;

            isChasing = true;

            audioSource.Stop();
            audioSource.clip = null;

            audioSource.clip = chasingClip;
            audioSource.volume = _chasingVolume;

            audioSource.Play();
        }

        public void StopChasing()
        {
            audioSource.DOKill();

            inTransition = true;

            audioSource.DOFade(0f, fadeDuration).OnComplete(() => 
            {
                audioSource.Stop();
                audioSource.clip = null;

                isChasing = false;
                
                inTransition = false;
            }).SetUpdate(true);
        }

        public void BlockMusic()
        {
            blockMusic = true;
            StopAudioFadeOut();
        }

        public void UnBlockMusic()
        {
            blockMusic = false;
        }
    }
}
