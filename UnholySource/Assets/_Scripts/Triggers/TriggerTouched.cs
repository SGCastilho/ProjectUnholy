using UnityEngine;
using UnityEngine.Events;

namespace Core.Triggers
{
    public sealed class TriggerTouched : MonoBehaviour
    {
        private const string PLAYER_TAG = "Player";

        [Header("Settings")]
        [SerializeField] private bool enabledWhenActive = true;

        [Header("Unity Event")]
        [Space(10)]

        [SerializeField] private UnityEvent OnPlayerTounch;

        private void Start() 
        {
            gameObject.SetActive(enabledWhenActive);
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.CompareTag(PLAYER_TAG))
            {
                OnPlayerTounch?.Invoke();
            }
        }
    }
}
