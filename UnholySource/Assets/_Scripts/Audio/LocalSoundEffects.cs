using DG.Tweening;
using UnityEngine;

namespace Core.Audio
{
    [System.Serializable]
    public struct AudioSettings
    {
        public string Key { get => audioKey; }
        public float Volume { get => audioVolume; }
        public AudioClip Clip { get => audioClip; }

        [SerializeField] private string audioKey;
        [SerializeField] [Range(0.1f, 1f)] private float audioVolume;
        [SerializeField] private AudioClip audioClip;
    }

    public sealed class LocalSoundEffects : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private AudioSettings[] audioSettings;

        [Space(10)]

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource loopingAudioSource;

        [Header("Settings")]
        [SerializeField] [Range(0.1f , 1f)] private float loopingFadeDuration = 0.2f;

        private float _loopingVolume;
        
        public void PlayAudioLoop(string audioKey)
        {
            AudioSettings selectedAudio = new AudioSettings();

            foreach(AudioSettings audio in audioSettings)
            {
                if(audio.Key == audioKey)
                {
                    selectedAudio = audio;
                }
            }

            if(selectedAudio.Key == null || selectedAudio.Key == string.Empty) return;

            loopingAudioSource.clip = selectedAudio.Clip;
            loopingAudioSource.loop = true;
            
            _loopingVolume = selectedAudio.Volume;

            AudioFadeIn();
        }

        public void PlayerAudioNoLoop(string audioKey)
        {
            AudioSettings selectedAudio = new AudioSettings();

            foreach(AudioSettings audio in audioSettings)
            {
                if(audio.Key == audioKey)
                {
                    selectedAudio = audio;
                }
            }

            if(selectedAudio.Key == null || selectedAudio.Key == string.Empty) return;
            
            loopingAudioSource.clip = selectedAudio.Clip;
            loopingAudioSource.loop = false;

            _loopingVolume = selectedAudio.Volume;

            AudioFadeIn();
        }

        private void AudioFadeIn()
        {
            loopingAudioSource.volume = 0f;

            loopingAudioSource.Play();

            loopingAudioSource.DOKill();
            loopingAudioSource.DOFade(_loopingVolume, loopingFadeDuration).SetUpdate(true);
        }

        public void StopAudioLoop()
        {
            AudioFadeOut();
        }

        private void AudioFadeOut()
        {
            loopingAudioSource.DOKill();
            loopingAudioSource.DOFade(0f, loopingFadeDuration).SetUpdate(true).OnComplete(() => { loopingAudioSource.clip = null; });
        }

        public void StopAudioLoop(float customDuration)
        {
            AudioFadeOut(customDuration);
        }

        private void AudioFadeOut(float duration)
        {
            loopingAudioSource.DOKill();
            loopingAudioSource.DOFade(0f, duration).SetUpdate(true).OnComplete(() => { loopingAudioSource.clip = null; });
        }

        public void PlayAudioOneShoot(string audioKey)
        {
            AudioSettings selectedAudio = new AudioSettings();

            foreach(AudioSettings audio in audioSettings)
            {
                if(audio.Key == audioKey)
                {
                    selectedAudio = audio;
                }
            }

            if(selectedAudio.Key == null || selectedAudio.Key == string.Empty) return;

            audioSource.PlayOneShot(selectedAudio.Clip, selectedAudio.Volume);
        }

        public void PlayAudioOneShoot(AudioClip audioClip, float volume)
        {
            if(audioClip == null) return;

            audioSource.PlayOneShot(audioClip, volume);
        }
    }
}
