using Core.Player;
using Core.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Triggers
{
    public sealed class TriggerInteraction : MonoBehaviour, IInteractable
    {
        #region Constants
        private const string PLAYER_TAG = "Player";
        #endregion
        
        [Header("Trigger Event")]

        [Space(10)]

        [SerializeField] private UnityEvent OnInteractionEvent;
        [SerializeField] private UnityEvent OnTriggerEnterUI;
        [SerializeField] private UnityEvent OnTriggerExitUI;

        private PlayerBehaviour _playerBehaviour;

        private void OnDisable()
        {
            OnTriggerExitUI?.Invoke();

            if(_playerBehaviour == null) return;

            _playerBehaviour.Inputs.UnsubscribeInteraction();

            _playerBehaviour = null;
        }

        public void Interact()
        {
            OnInteractionEvent?.Invoke();
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(other.CompareTag(PLAYER_TAG))
            {
                OnTriggerEnterUI?.Invoke();

                _playerBehaviour = other.GetComponent<PlayerBehaviour>();

                _playerBehaviour.Inputs.SubscribeInteraction(Interact);
            }
        }

        private void OnTriggerExit(Collider other) 
        {
            if(other.CompareTag(PLAYER_TAG))
            {
                OnTriggerExitUI?.Invoke();

                _playerBehaviour.Inputs.UnsubscribeInteraction();

                _playerBehaviour = null;
            }
        }
    }
}
