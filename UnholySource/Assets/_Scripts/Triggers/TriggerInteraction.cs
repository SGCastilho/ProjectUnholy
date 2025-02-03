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

        #region Encapsulation
        public bool Interactable 
        { 
            set 
            { 
                _interactable = value;

                if(value == false)
                {
                    OnTriggerExitUI?.Invoke();

                    if(_playerBehaviour == null) return;

                    _playerBehaviour.Inputs.UnsubscribeInteraction();

                    _playerBehaviour = null;
                } 
            } 
        }

        internal PlayerBehaviour Player { get => _playerBehaviour; }
        #endregion
        
        [Header("Trigger Event")]

        [Space(10)]

        [SerializeField] private UnityEvent OnInteractionEvent;
        [SerializeField] private UnityEvent OnTriggerEnterUI;
        [SerializeField] private UnityEvent OnTriggerExitUI;

        private PlayerBehaviour _playerBehaviour;
        private Collider _triggerCollider;

        private bool _interactable;

        private void Awake() 
        {
            _interactable = true;
            _triggerCollider = GetComponent<Collider>();
        }

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

        public void DisableInteraction()
        {
            OnTriggerExitUI?.Invoke();
            
            _playerBehaviour.Inputs.UnsubscribeInteraction();

            _triggerCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(!_interactable) return;

            if(other.CompareTag(PLAYER_TAG))
            {
                OnTriggerEnterUI?.Invoke();

                _playerBehaviour = other.GetComponent<PlayerBehaviour>();

                _playerBehaviour.Inputs.SubscribeInteraction(Interact);
            }
        }

        private void OnTriggerExit(Collider other) 
        {
            if(!_interactable) return;

            if(other.CompareTag(PLAYER_TAG))
            {
                OnTriggerExitUI?.Invoke();

                _playerBehaviour.Inputs.UnsubscribeInteraction();

                _playerBehaviour = null;
            }
        }
    }
}
