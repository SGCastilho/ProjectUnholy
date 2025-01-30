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

        public AudioSettings _oneShootDelayClip;

        private bool _isDelaying;

        private float _loopingVolume;

        private float _oneShootDelay;
        private float _currentOneShootDelay;

        private void Update() 
        {
            if(_isDelaying)
            {
                _currentOneShootDelay += Time.deltaTime;
                if(_currentOneShootDelay >= _oneShootDelay)
                {
                    _isDelaying = false;
                    _currentOneShootDelay = 0f;

                    audioSource.PlayOneShot(_oneShootDelayClip.Clip, _oneShootDelayClip.Volume);
                }
            }
        }
        
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

        public void PlayAudioOneShootWithDelay(string audioKey, float delay)
        {
            if(_isDelaying) return;

            _oneShootDelayClip = new AudioSettings();

            AudioSettings selectedAudio = new AudioSettings();

            foreach(AudioSettings audio in audioSettings)
            {
                if(audio.Key == audioKey)
                {
                    selectedAudio = audio;
                }
            }

            if(selectedAudio.Key == null || selectedAudio.Key == string.Empty) return;

            _oneShootDelay = delay;
            _oneShootDelayClip = selectedAudio;

            _isDelaying = true;
        }
    }
}
