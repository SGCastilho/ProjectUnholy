using Core.UI;
using Core.Audio;
using Core.Player;
using Core.Triggers;
using Core.Managers;
using Core.GameCamera;
using UnityEngine;

namespace Core.Events
{
    public sealed class GameplayEvents : MonoBehaviour
    {
        [Header("Managers Classes")]
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private ScenarioLoaderManager scenarioLoaderManager;

        [Header("UI Classes")]
        [SerializeField] private UIFadeController uIFadeController;
        [SerializeField] private UIGameplayController uIGameplayController;

        private PlayerBehaviour playerBehaviour;
        private LocalSoundEffects playerSoundEffects;

        private CameraShake cameraShake;
        private ChangeCameraRendering changeCameraRendering;

        private UnlockDoorTrigger[] unlockDoorsTriggers;
        private PuzzleInteractionTrigger[] puzzleInteractionTriggers;

        private void Awake() => CacheVariables();

        private void CacheVariables()
        {
            playerBehaviour = FindObjectOfType<PlayerBehaviour>();
            playerSoundEffects = playerBehaviour.GetComponentInChildren<LocalSoundEffects>();

            cameraShake = FindObjectOfType<CameraShake>();
            changeCameraRendering = FindObjectOfType<ChangeCameraRendering>();

            var unlockTriggers = FindObjectsOfType<UnlockDoorTrigger>();

            if(unlockTriggers != null || unlockTriggers.Length > 0)
            {
                unlockDoorsTriggers = new UnlockDoorTrigger[unlockTriggers.Length];
                unlockDoorsTriggers = unlockTriggers;
            }

            var puzzleTriggers = FindObjectsOfType<PuzzleInteractionTrigger>();

            if(puzzleTriggers != null || puzzleTriggers.Length > 0)
            {
                puzzleInteractionTriggers = new PuzzleInteractionTrigger[puzzleTriggers.Length];
                puzzleInteractionTriggers = puzzleTriggers;
            }
        }

        private void OnEnable()
        {
            PlayerEnableEvents();

            PauseEnableEvents();

            InventoryEnableEvents();

            if(unlockDoorsTriggers != null && unlockDoorsTriggers.Length > 0)
            {
                foreach(UnlockDoorTrigger unlockDoor in unlockDoorsTriggers)
                {
                    unlockDoor.OnCheckIfPlayerHasTheItem += inventoryManager.CheckIfHasItem;
                    unlockDoor.OnRemoveItemFromInventory += inventoryManager.RemoveKeyItem;
                    unlockDoor.OnUnlockedDoor += unlockDoor.GetComponent<UIOverworldTriggerIcon>().ChangeInteractionSprite;
                }
            }

            if(puzzleInteractionTriggers != null && puzzleInteractionTriggers.Length > 0)
            {
                foreach(PuzzleInteractionTrigger puzzleUnlock in puzzleInteractionTriggers)
                {
                    puzzleUnlock.OnReceiveItemFromInventory += inventoryManager.AddKeyItem;
                    puzzleUnlock.OnCheckIfPlayerHasTheItem += inventoryManager.CheckIfHasItem;
                    puzzleUnlock.OnRemoveItemFromInventory += inventoryManager.RemoveKeyItem;
                }
            }

            ScenarioLoaderEnableEvents();

            UIEnableEvents();
        }

        private void UIEnableEvents()
        {
            uIGameplayController.UI_HurtAlertOverlay.OnEnterCriticalHealth += playerSoundEffects.PlayAudioLoop;
            uIGameplayController.UI_HurtAlertOverlay.OnLeftCriticalHealth += playerSoundEffects.StopAudioLoop;

            uIGameplayController.UI_Inventory.OnCallingInventory += pauseManager.Pause;
            uIGameplayController.UI_Inventory.OnCallingInventory += playerBehaviour.Inputs.BlockInputsWhenInventory;

            uIGameplayController.UI_Inventory.OnUnCallingInventory += pauseManager.UnPause;
            uIGameplayController.UI_Inventory.OnAllowingMenus += playerBehaviour.Inputs.AllowInputsWhenInventory;

            uIGameplayController.UI_Inventory.OnCheckMeleeWeaponState += playerBehaviour.Equipment.HasMeleeWeapon;
            uIGameplayController.UI_Inventory.OnCheckRangedWeaponState += playerBehaviour.Equipment.HasRangedWeapon;

            uIGameplayController.UI_Inventory.OnCheckHealingBottlesState += playerBehaviour.Resources.CurrentHealingBottles;
            uIGameplayController.UI_Inventory.OnCheckWeaponBulletsState += playerBehaviour.Resources.CurrentBullets;
            uIGameplayController.UI_Inventory.OnCheckWeaponAmmoState += playerBehaviour.Resources.CurrentAmmo;

            uIGameplayController.UI_ItemNotification.OnShowInterface += pauseManager.Pause;
            uIGameplayController.UI_ItemNotification.OnHideInteface += pauseManager.UnPause;

            uIGameplayController.UI_ItemNotification.OnAllowControls += playerBehaviour.Inputs.AllowControls;
            uIGameplayController.UI_ItemNotification.OnBlockControls += playerBehaviour.Inputs.BlockControls;

            uIGameplayController.UI_PauseGame.OnShowInterface += pauseManager.Pause;
            uIGameplayController.UI_PauseGame.OnShowInterface += playerBehaviour.Inputs.BlockInputsWhenPauseMenu;

            uIGameplayController.UI_PauseGame.OnHideInteface += pauseManager.UnPause;
            uIGameplayController.UI_PauseGame.OnHideInteface += playerBehaviour.Inputs.AllowInputsWhenPauseMenu;

            uIGameplayController.UI_Inventory.OnShowInventoryEnd += changeCameraRendering.OnlyRenderingUI;
            uIGameplayController.UI_Inventory.OnHideInventoryStarts += changeCameraRendering.BackToDefaultRendering;
        }

        private void ScenarioLoaderEnableEvents()
        {
            scenarioLoaderManager.OnStartTravel += pauseManager.Pause;
            scenarioLoaderManager.OnStartTravel += playerBehaviour.Inputs.BlockControls;
            scenarioLoaderManager.OnStartTravel += uIFadeController.FadeIn;

            scenarioLoaderManager.OnEndTravel += pauseManager.UnPause;
            scenarioLoaderManager.OnEndTravel += playerBehaviour.Inputs.AllowControls;
            scenarioLoaderManager.OnEndTravel += uIFadeController.FadeOut;
        }

        private void InventoryEnableEvents()
        {
            inventoryManager.OnModifyInventory += uIGameplayController.UI_Inventory.ModifyItems;
        }

        private void PauseEnableEvents()
        {
            pauseManager.OnPaused += playerBehaviour.Inputs.BlockInputsWhenPaused;
            pauseManager.OnUnPaused += playerBehaviour.Inputs.AllowInputsWhenUnPaused;
        }

        private void PlayerEnableEvents()
        {
            playerBehaviour.Inputs.SubscribeInventory(uIGameplayController.UI_Inventory.CallInventory);
            playerBehaviour.Inputs.SubscribePause(uIGameplayController.UI_PauseGame.CallPause);

            playerBehaviour.Status.OnModifingHealth += uIGameplayController.UI_HurtAlertOverlay.CheckAlertOverlay;
            playerBehaviour.Status.OnTakingDamage += cameraShake.HittedShake;
            playerBehaviour.Status.OnDeath += scenarioLoaderManager.ReloadCurrentScene;

            playerBehaviour.Resources.OnRefreshWeaponUI += uIGameplayController.UI_RangedWeapon.RefreshWeaponInfo;
            playerBehaviour.Resources.OnRefreshBottlesUI += uIGameplayController.UI_HealingBottles.RefreshHealingInfo;
        }

        private void OnDisable()
        {
            PlayerDisableEvents();

            PauseDisableEvents();

            InventoryDisableEvents();

            if(unlockDoorsTriggers != null && unlockDoorsTriggers.Length > 0)
            {
                foreach(UnlockDoorTrigger unlockDoor in unlockDoorsTriggers)
                {
                    unlockDoor.OnCheckIfPlayerHasTheItem -= inventoryManager.CheckIfHasItem;
                    unlockDoor.OnRemoveItemFromInventory -= inventoryManager.RemoveKeyItem;
                    unlockDoor.OnUnlockedDoor += null;
                }
            }

            if(puzzleInteractionTriggers != null && puzzleInteractionTriggers.Length > 0)
            {
                foreach(PuzzleInteractionTrigger puzzleUnlock in puzzleInteractionTriggers)
                {
                    puzzleUnlock.OnReceiveItemFromInventory -= inventoryManager.AddKeyItem;
                    puzzleUnlock.OnCheckIfPlayerHasTheItem -= inventoryManager.CheckIfHasItem;
                    puzzleUnlock.OnRemoveItemFromInventory -= inventoryManager.RemoveKeyItem;
                }
            }

            ScenarioLoaderDisableEvents();

            UIDisableEvents();
        }

        private void UIDisableEvents()
        {
            uIGameplayController.UI_HurtAlertOverlay.OnEnterCriticalHealth -= playerSoundEffects.PlayAudioLoop;
            uIGameplayController.UI_HurtAlertOverlay.OnLeftCriticalHealth -= playerSoundEffects.StopAudioLoop;

            uIGameplayController.UI_Inventory.OnCallingInventory -= pauseManager.Pause;
            uIGameplayController.UI_Inventory.OnCallingInventory -= playerBehaviour.Inputs.BlockInputsWhenInventory;

            uIGameplayController.UI_Inventory.OnUnCallingInventory -= pauseManager.UnPause;
            uIGameplayController.UI_Inventory.OnAllowingMenus -= playerBehaviour.Inputs.AllowInputsWhenInventory;

            uIGameplayController.UI_Inventory.OnCheckMeleeWeaponState -= playerBehaviour.Equipment.HasMeleeWeapon;
            uIGameplayController.UI_Inventory.OnCheckRangedWeaponState -= playerBehaviour.Equipment.HasRangedWeapon;

            uIGameplayController.UI_Inventory.OnCheckHealingBottlesState -= playerBehaviour.Resources.CurrentHealingBottles;
            uIGameplayController.UI_Inventory.OnCheckWeaponBulletsState -= playerBehaviour.Resources.CurrentBullets;
            uIGameplayController.UI_Inventory.OnCheckWeaponAmmoState -= playerBehaviour.Resources.CurrentAmmo;

            uIGameplayController.UI_ItemNotification.OnShowInterface -= pauseManager.Pause;
            uIGameplayController.UI_ItemNotification.OnHideInteface -= pauseManager.UnPause;

            uIGameplayController.UI_ItemNotification.OnAllowControls -= playerBehaviour.Inputs.AllowControls;
            uIGameplayController.UI_ItemNotification.OnBlockControls -= playerBehaviour.Inputs.BlockControls;

            uIGameplayController.UI_PauseGame.OnShowInterface -= pauseManager.Pause;
            uIGameplayController.UI_PauseGame.OnShowInterface -= playerBehaviour.Inputs.BlockInputsWhenPauseMenu;

            uIGameplayController.UI_PauseGame.OnHideInteface -= pauseManager.UnPause;
            uIGameplayController.UI_PauseGame.OnHideInteface -= playerBehaviour.Inputs.AllowInputsWhenPauseMenu;

            uIGameplayController.UI_Inventory.OnShowInventoryEnd -= changeCameraRendering.OnlyRenderingUI;
            uIGameplayController.UI_Inventory.OnHideInventoryStarts -= changeCameraRendering.BackToDefaultRendering;
        }

        private void ScenarioLoaderDisableEvents()
        {
            scenarioLoaderManager.OnStartTravel -= pauseManager.Pause;
            scenarioLoaderManager.OnStartTravel -= playerBehaviour.Inputs.BlockControls;
            scenarioLoaderManager.OnStartTravel -= uIFadeController.FadeIn;

            scenarioLoaderManager.OnEndTravel -= pauseManager.UnPause;
            scenarioLoaderManager.OnEndTravel -= playerBehaviour.Inputs.AllowControls;
            scenarioLoaderManager.OnEndTravel -= uIFadeController.FadeOut;
        }

        private void InventoryDisableEvents()
        {
            inventoryManager.OnModifyInventory -= uIGameplayController.UI_Inventory.ModifyItems;
        }

        private void PauseDisableEvents()
        {
            pauseManager.OnPaused -= playerBehaviour.Inputs.BlockInputsWhenPaused;
            pauseManager.OnUnPaused -= playerBehaviour.Inputs.AllowInputsWhenUnPaused;
        }

        private void PlayerDisableEvents()
        {
            playerBehaviour.Inputs.UnsubscribeInventory();
            playerBehaviour.Inputs.UnsubscribePause();

            playerBehaviour.Status.OnModifingHealth -= uIGameplayController.UI_HurtAlertOverlay.CheckAlertOverlay;
            playerBehaviour.Status.OnTakingDamage -= cameraShake.HittedShake;
            playerBehaviour.Status.OnDeath -= scenarioLoaderManager.ReloadCurrentScene;

            playerBehaviour.Resources.OnRefreshWeaponUI -= uIGameplayController.UI_RangedWeapon.RefreshWeaponInfo;
            playerBehaviour.Resources.OnRefreshBottlesUI -= uIGameplayController.UI_HealingBottles.RefreshHealingInfo;
        }
    }
}
