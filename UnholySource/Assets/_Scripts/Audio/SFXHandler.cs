using UnityEngine;

namespace Core.Audio
{
    public sealed class SFXHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private AudioSettings[] sfxHandler;

        public AudioClip GetAudioClipByKey(string key)
        {
            if(sfxHandler == null || sfxHandler.Length <= 0) return null;

            for(int i = 0; i<sfxHandler.Length;i++)
            {
                if(sfxHandler[i].Key == key)
                {
                    return sfxHandler[i].Clip;
                }
            }

            return null;
        }

        public float GetVolumeByKey(string key)
        {
            if(sfxHandler == null || sfxHandler.Length <= 0) return 1f;

            for(int i = 0; i<sfxHandler.Length;i++)
            {
                if(sfxHandler[i].Key == key)
                {
                    return sfxHandler[i].Volume;
                }
            }

            return 1f;
        }
    }
}
