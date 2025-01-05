using UnityEngine;

namespace Core.Audio
{
    public sealed class SetAudioClipToPlaySpecificSeconds : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private AudioSource audioSource;

        [Header("Settings")]
        [SerializeField] private float secondsToJump;

        public void JumpToSeconds()
        {
            if(audioSource.time >= secondsToJump) return;

            audioSource.Stop();
            audioSource.time = secondsToJump;
            audioSource.Play();
        }
    }
}
