using Core.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player
{
    public sealed class PlayerInputs : MonoBehaviour
    {
        #region Encapsulation
        internal float MovementAxis { get => _movementAxis; }
        internal float MovementAxisAbs { get => Mathf.Abs(_movementAxis); }
        #endregion

        [Header("Behaviour")]
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

            _gameplayInputActions.Gameplay.Sprint.started += StartSprint;
            _gameplayInputActions.Gameplay.Sprint.canceled += EndSprint;
        }

        private void OnDisable() 
        {
            _gameplayInputActions.Disable();

            _gameplayInputActions.Gameplay.Sprint.started -= StartSprint;
            _gameplayInputActions.Gameplay.Sprint.canceled -= EndSprint;
        }

        private void Update() 
        {
            _movementAxis = _gameplayInputActions.Gameplay.Movement.ReadValue<float>();
        }

        #region Input Functions
        public void ShootInput()
        {
            behaviour.Attack.StartAttack();
        }

        public void AimInput()
        {
            
        }

        public void ReloadInput()
        {
            
        }

        public void InteractInput()
        {
            
        }

        public void HealInput()
        {
            
        }

        private void StartSprint(InputAction.CallbackContext context)
        {
            behaviour.Movement.SprintEnabled = true;
            behaviour.Animation.SprintAnimation = true;
        }

        private void EndSprint(InputAction.CallbackContext context)
        {
            behaviour.Movement.SprintEnabled = false;
            behaviour.Animation.SprintAnimation = false;
        }
        #endregion

        public void AllowMovement()
        {
            _gameplayInputActions.Gameplay.Movement.Enable();
        }

        public void BlockMovement()
        {
            _gameplayInputActions.Gameplay.Movement.Disable();
        }

        public void AllowActions()
        {
            _gameplayInputActions.Gameplay.Aim.Enable();
            _gameplayInputActions.Gameplay.Heal.Enable();
            _gameplayInputActions.Gameplay.Interact.Enable();
            _gameplayInputActions.Gameplay.Reload.Enable();
            _gameplayInputActions.Gameplay.Shoot.Enable();
        }

        public void BlockActions()
        {
            _gameplayInputActions.Gameplay.Aim.Disable();
            _gameplayInputActions.Gameplay.Heal.Disable();
            _gameplayInputActions.Gameplay.Interact.Disable();
            _gameplayInputActions.Gameplay.Reload.Disable();
            _gameplayInputActions.Gameplay.Shoot.Disable();
        }

        public void AllowControls()
        {
            _gameplayInputActions.Gameplay.Enable();
        }

        public void BlockControls()
        {
            _gameplayInputActions.Gameplay.Disable();
        }
    }
}
