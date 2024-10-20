using Core.Input;
using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerInputs : MonoBehaviour
    {
        #region Encapsulation
        internal float MovementAxis { get => _movementAxis; }
        #endregion

        [Header("Classes")]
        [SerializeField] private PlayerBehaviour behaviour;

        private GameplayInput _gameplayInputActions;

        private float _movementAxis;

        private void Awake() 
        {
            _gameplayInputActions = new GameplayInput();
        }

        private void OnEnable() 
        {
            _gameplayInputActions.Enable();
        }

        private void OnDisable() 
        {
            _gameplayInputActions.Disable();
        }

        private void Update() 
        {
            _movementAxis = _gameplayInputActions.Gameplay.Movement.ReadValue<float>();
        }

        #region Input Functions
        public void ShootInput()
        {
            Debug.Log("Shooting");
        }

        public void AimInput()
        {
            Debug.Log("Aiming");
        }

        public void ReloadInput()
        {
            Debug.Log("Reloading");
        }

        public void SprintInput()
        {
            Debug.Log("Sprinting");
        }

        public void InteractInput()
        {
            Debug.Log("Interacting");
        }

        public void HealInput()
        {
            Debug.Log("Healing");
        }
        #endregion
    }
}
