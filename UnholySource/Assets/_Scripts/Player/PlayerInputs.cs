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

        private Action _pauseAction;
        private Action _inventoryAction;
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

            _gameplayInputActions.Gameplay.Shoot.started += ShootInput;

            _gameplayInputActions.Gameplay.Reload.started += ReloadInput;

            _gameplayInputActions.Gameplay.Interact.started += InteractInput;

            _gameplayInputActions.Gameplay.Heal.started += HealInput;

            _gameplayInputActions.Gameplay.Inventory.started += InventoryInput;

            _gameplayInputActions.Gameplay.Pause.started += PauseInput;

            BlockControls();
        }

        private void OnDisable() 
        {
            _gameplayInputActions.Disable();

            _gameplayInputActions.Gameplay.Sprint.started -= StartSprint;
            _gameplayInputActions.Gameplay.Sprint.canceled -= EndSprint;

            _gameplayInputActions.Gameplay.Aim.started -= AimInput;

            _gameplayInputActions.Gameplay.Shoot.started -= ShootInput;

            _gameplayInputActions.Gameplay.Reload.started -= ReloadInput;

            _gameplayInputActions.Gameplay.Interact.started -= InteractInput;

            _gameplayInputActions.Gameplay.Heal.started -= HealInput;

            _gameplayInputActions.Gameplay.Inventory.started -= InventoryInput;

            _gameplayInputActions.Gameplay.Pause.started -= PauseInput;
        }

        private void Update() 
        {
            _movementAxis = _gameplayInputActions.Gameplay.Movement.ReadValue<float>();
        }

        #region Input Setups
        public void ShootInput(InputAction.CallbackContext context)
        {
            behaviour.Actions.Attack();
        }

        public void AimInput(InputAction.CallbackContext context)
        {
            behaviour.Actions.AimingSetup();
        }

        public void ReloadInput(InputAction.CallbackContext context)
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

        public void InteractInput(InputAction.CallbackContext context)
        {
            _interactionAction?.Invoke();
        }

        public void HealInput(InputAction.CallbackContext context)
        {
            if(behaviour.Actions.IsReloading) return;

            behaviour.Actions.Healing();
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

        public void InventoryInput(InputAction.CallbackContext context)
        {
            _inventoryAction?.Invoke();
        }

        public void PauseInput(InputAction.CallbackContext context)
        {
            _pauseAction?.Invoke();
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

        public void SubscribeInventory(Action action)
        {
            if(action == null) return;

            _inventoryAction += action;
        }

        public void UnsubscribeInventory()
        {
            _inventoryAction = null;
        }

        public void SubscribePause(Action action)
        {
            if(action == null) return;

            _pauseAction += action;
        }

        public void UnsubscribePause()
        {
            _pauseAction = null;
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
        }

        public void BlockActionsWhenAiming()
        {
            _gameplayInputActions.Gameplay.Heal.Disable();
        }

        public void AllowInputsWhenUnPaused()
        {
            AllowActions();
            AllowMovement();

            behaviour.HideCursor(true);
        }

        public void BlockInputsWhenPaused()
        {
            BlockActions();
            BlockMovement();

            behaviour.HideCursor(false);
        }

        public void AllowInputsWhenInventory()
        {
            _gameplayInputActions.Gameplay.Pause.Enable();
        }

        public void BlockInputsWhenInventory()
        {
            _gameplayInputActions.Gameplay.Pause.Disable();
        }

        public void AllowInputsWhenPauseMenu()
        {
            _gameplayInputActions.Gameplay.Inventory.Enable();
        }

        public void BlockInputsWhenPauseMenu()
        {
            _gameplayInputActions.Gameplay.Inventory.Disable();
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
