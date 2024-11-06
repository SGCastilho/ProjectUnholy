using System;
using Core.Inputs;
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

        private Action _interactionAction;

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

            _gameplayInputActions.Gameplay.Aim.started += AimInput;
        }

        private void OnDisable() 
        {
            _gameplayInputActions.Disable();

            _gameplayInputActions.Gameplay.Sprint.started -= StartSprint;
            _gameplayInputActions.Gameplay.Sprint.canceled -= EndSprint;

            _gameplayInputActions.Gameplay.Aim.started -= AimInput;
        }

        private void Update() 
        {
            _movementAxis = _gameplayInputActions.Gameplay.Movement.ReadValue<float>();
        }

        #region Input Setups
        public void ShootInput()
        {
            behaviour.Actions.Attack();
        }

        public void AimInput(InputAction.CallbackContext context)
        {
            behaviour.Actions.AimingSetup();
        }

        public void ReloadInput()
        {
            if(!behaviour.Resources.CanReload() || !behaviour.Equipment.RangedEnabled) return;

            if(behaviour.Actions.IsAiming)
            {
                behaviour.Actions.CancelAiming();
            }

            behaviour.Resources.ReloadingBullets();

            behaviour.Actions.IsReloading = true;

            behaviour.Animation.CallReloadingTrigger();

            BlockActions();
            AllowMovement();
        }

        public void InteractInput()
        {
            _interactionAction?.Invoke();
        }

        public void HealInput()
        {
            if(behaviour.Status.IsDead || behaviour.Actions.IsReloading) return;

            behaviour.Actions.Healing();
            
            BlockActions();
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

        #region Input Functions
        public void SubscribeInteraction(Action action)
        {
            if(action == null) return;

            _interactionAction += action;
        }

        public void UnsubscribeInteraction()
        {
            _interactionAction = null;
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

        public void AllowActionsBeforeAiming()
        {
            _gameplayInputActions.Gameplay.Heal.Enable();
            _gameplayInputActions.Gameplay.Interact.Enable();
            _gameplayInputActions.Gameplay.Movement.Enable();
        }

        public void BlockActionsWhenAiming()
        {
            _gameplayInputActions.Gameplay.Heal.Disable();
            _gameplayInputActions.Gameplay.Interact.Disable();
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
