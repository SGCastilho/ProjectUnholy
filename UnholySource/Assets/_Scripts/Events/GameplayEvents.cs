using Core.UI;
using Core.Audio;
using Core.Player;
using Core.Enemies;
using Core.Triggers;
using Core.Utilities;
using Core.Managers;
using Core.GameCamera;
using Core.StateMachines;
using UnityEngine;

namespace Core.Events
{
    public sealed class GameplayEvents : MonoBehaviour
    {
        [Header("UI Classes")]
        [SerializeField] private UIFadeController uIFadeController;
        [SerializeField] private UIGameplayController uIGameplayController;
        [SerializeField] private UIAreaNotification uIAreaNotification;
        [SerializeField] private UISaveAndLoad uISaveAndLoad;

        [Header("Audio Classes")]
        [SerializeField] private MusicManager musicManager;
        [SerializeField] private LocalSoundEffects globalSoundEffects;

        [Header("Managers Classes")]
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private LoaderManager loaderManager;
        [SerializeField] private ChaserManager chaserManager;
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private ChapterEventsManager chapterEventsManager;
        [SerializeField] private ScenarioLoaderManager scenarioLoaderManager;

        [Header("Utilities Classes")]
        [SerializeField] private VolumeProfileHandler gameOverVolumeProfile;

        [Header("State Machines Classes")]
        [SerializeField] private StateMachine chasersStateMachine;

        [Header("Pursuer Classes")]
        [SerializeField] private PursuerSearchingState pursuerSearchingState;

        private PlayerBehaviour _playerBehaviour;
        private LocalSoundEffects _playerSoundEffects;

        private CameraShake _cameraShake;
        private ChangeCameraRendering _changeCameraRendering;

        private UnlockDoorTrigger[] _unlockDoorsTriggers;
        private PuzzleInteractionTrigger[] _puzzleInteractionTriggers;

        private void Awake() => CacheVariables();

        private void CacheVariables()
        {
            _playerBehaviour = FindObjectOfType<PlayerBehaviour>();
            _playerSoundEffects = _playerBehaviour.GetComponentInChildren<LocalSoundEffects>();

            _cameraShake = FindObjectOfType<CameraShake>();
            _changeCameraRendering = FindObjectOfType<ChangeCameraRendering>();

            var unlockTriggers = FindObjectsOfType<UnlockDoorTrigger>();

            if(unlockTriggers != null || unlockTriggers.Length > 0)
            {
                _unlockDoorsTriggers = new UnlockDoorTrigger[unlockTriggers.Length];
                _unlockDoorsTriggers = unlockTriggers;
            }

            var puzzleTriggers = FindObjectsOfType<PuzzleInteractionTrigger>();

            if(puzzleTriggers != null || puzzleTriggers.Length > 0)
            {
                _puzzleInteractionTriggers = new PuzzleInteractionTrigger[puzzleTriggers.Length];
                _puzzleInteractionTriggers = puzzleTriggers;
            }
        }

        private void OnEnable()
        {
            PlayerEnableEvents();

            PauseEnableEvents();

            InventoryEnableEvents();

            if (_unlockDoorsTriggers != null && _unlockDoorsTriggers.Length > 0)
            {
                foreach (UnlockDoorTrigger unlockDoor in _unlockDoorsTriggers)
                {
                    unlockDoor.OnCheckIfPlayerHasTheItem += inventoryManager.CheckIfHasItem;
                    unlockDoor.OnRemoveItemFromInventory += inventoryManager.RemoveKeyItem;
                    unlockDoor.OnUnlockedDoor += unlockDoor.GetComponent<UIOverworldTriggerIcon>().ChangeInteractionSprite;
                }
            }

            if (_puzzleInteractionTriggers != null && _puzzleInteractionTriggers.Length > 0)
            {
                foreach (PuzzleInteractionTrigger puzzleUnlock in _puzzleInteractionTriggers)
                {
                    puzzleUnlock.OnReceiveItemFromInventory += inventoryManager.AddKeyItem;
                    puzzleUnlock.OnCheckIfPlayerHasTheItem += inventoryManager.CheckIfHasItem;
                    puzzleUnlock.OnRemoveItemFromInventory += inventoryManager.RemoveKeyItem;
                }
            }

            ChaserManagerEnableEvents();

            ScenarioLoaderEnableEvents();

            UIEnableEvents();

            GameManagerEnableEvents();

            SaveManagerEnableEvents();

            LoaderManagerEnableEvents();

            if(pursuerSearchingState != null)
            {
                pursuerSearchingState.OnChasingStarted += musicManager.PlayerChasing;
            }
        }

        private void LoaderManagerEnableEvents()
        {
            loaderManager.OnStartLoadSaveGame += _playerBehaviour.Inputs.BlockActions;
            loaderManager.OnLoadSaveGame += scenarioLoaderManager.LoadScene;
        }

        private void SaveManagerEnableEvents()
        {
            saveManager.OnGetCurrentChapter += gameManager.GetCurrentChapter;

            saveManager.OnGetCurrentLevel += scenarioLoaderManager.GetCurrentScene;
            saveManager.OnGetCurrentRoom += scenarioLoaderManager.GetCurrentRoom;

            saveManager.OnGetPlayerCurrentPos += _playerBehaviour.GetCurrentPosistion;

            saveManager.OnGetPlayerCurrentHealth += _playerBehaviour.Status.GetCurrentHealth;

            saveManager.OnGetPlayerCurrentHealthBottles += _playerBehaviour.Resources.CurrentHealingBottles;
            saveManager.OnGetPlayerCurrentBullets += _playerBehaviour.Resources.CurrentBullets;
            saveManager.OnGetPlayerCurrentMunition += _playerBehaviour.Resources.CurrentAmmo;

            saveManager.OnGetPlayerMeleeStatus += _playerBehaviour.Equipment.HasMeleeWeapon;
            saveManager.OnGetPlayerRangedStatus += _playerBehaviour.Equipment.HasRangedWeapon;

            saveManager.OnGetInventoryItems += inventoryManager.GetInventoryItems;

            saveManager.OnGetChaserStatus += chaserManager.GetChaserStatus;

            saveManager.OnGetChapterEventsTriggereds += chapterEventsManager.GetAllEndedEvents;
        }

        private void ChaserManagerEnableEvents()
        {
            chaserManager.OnChaserEmitSounds += musicManager.BlockMusic;
            chaserManager.OnEmitSound += globalSoundEffects.PlayerAudioNoLoop;
            chaserManager.OnStopEmittingSound += globalSoundEffects.StopAudioLoop;
            chaserManager.OnChaserStopEmitingSounds += musicManager.UnBlockMusic;
            chaserManager.OnSpawnNoRules += globalSoundEffects.PlayAudioOneShootWithDelay;
            chaserManager.OnGetPlayerLastSpawn += scenarioLoaderManager.ReturnPlayerLastSpawn;

            chaserManager.OnChasingEnd += musicManager.StopChasing;

            if (chasersStateMachine != null)
            {
                chaserManager.OnChasingEnd += chasersStateMachine.ChasingStateOver;
            }
        }

        private void GameManagerEnableEvents()
        {
            gameManager.OnLoadRoom += scenarioLoaderManager.LoadRoom;
            gameManager.OnLoadPlayerHealth += _playerBehaviour.Status.LoadedPlayerHealth;
            gameManager.OnLoadPlayerResources += _playerBehaviour.Resources.LoadPlayerResources;
            gameManager.OnLoadPlayerEquipment += _playerBehaviour.Equipment.LoadPlayerEquipment;
            gameManager.OnLoadPlayerInventory += inventoryManager.LoadPlayerInventory;
            gameManager.OnLoadChaserStatus += chaserManager.LoadChaserStatus;
            gameManager.OnLoadChapterEndedEvents += chapterEventsManager.LoadEndedEvents;

            gameManager.OnGameLoadedEnd += uIAreaNotification.DisableNotificationOnStart;

            gameManager.EnablePlayerControlls += _playerBehaviour.Inputs.AllowControls;

            gameManager.OnGameStarted += uIFadeController.CustomFadeOut;
            gameManager.OnGameLoaded += uIFadeController.CustomFadeOut;

            gameManager.OnGameWinEnded += scenarioLoaderManager.LoadScene;

            gameManager.OnGameOverStart += gameOverVolumeProfile.Show;
            gameManager.OnGameOverEnd += scenarioLoaderManager.GameOverLoadScene;
        }
        
        private void UIEnableEvents()
        {
            uIGameplayController.OnPlaySFX += globalSoundEffects.PlayAudioOneShoot;

            uIGameplayController.UI_HurtAlertOverlay.OnEnterCriticalHealth += _playerSoundEffects.PlayAudioLoop;
            uIGameplayController.UI_HurtAlertOverlay.OnLeftCriticalHealth += _playerSoundEffects.StopAudioLoop;

            uIGameplayController.UI_Inventory.OnCallingInventory += pauseManager.Pause;
            uIGameplayController.UI_Inventory.OnCallingInventory += _playerBehaviour.Inputs.BlockInputsWhenInventory;

            uIGameplayController.UI_Inventory.OnUnCallingInventory += pauseManager.UnPause;
            uIGameplayController.UI_Inventory.OnAllowingMenus += _playerBehaviour.Inputs.AllowInputsWhenInventory;

            uIGameplayController.UI_Inventory.OnCheckMeleeWeaponState += _playerBehaviour.Equipment.HasMeleeWeapon;
            uIGameplayController.UI_Inventory.OnCheckRangedWeaponState += _playerBehaviour.Equipment.HasRangedWeapon;

            uIGameplayController.UI_Inventory.OnCheckHealingBottlesState += _playerBehaviour.Resources.CurrentHealingBottles;
            uIGameplayController.UI_Inventory.OnCheckWeaponBulletsState += _playerBehaviour.Resources.CurrentBullets;
            uIGameplayController.UI_Inventory.OnCheckWeaponAmmoState += _playerBehaviour.Resources.CurrentAmmo;

            uIGameplayController.UI_ItemNotification.OnShowInterface += pauseManager.Pause;
            uIGameplayController.UI_ItemNotification.OnHideInteface += pauseManager.UnPause;

            uIGameplayController.UI_ItemNotification.OnAllowControls += _playerBehaviour.Inputs.AllowControls;
            uIGameplayController.UI_ItemNotification.OnBlockControls += _playerBehaviour.Inputs.BlockControls;

            uIGameplayController.UI_PauseGame.OnShowInterface += pauseManager.Pause;
            uIGameplayController.UI_PauseGame.OnShowInterface += _playerBehaviour.Inputs.BlockInputsWhenPauseMenu;

            uIGameplayController.UI_PauseGame.OnHideInteface += pauseManager.UnPause;
            uIGameplayController.UI_PauseGame.OnHideInteface += _playerBehaviour.Inputs.AllowInputsWhenPauseMenu;

            uIGameplayController.UI_Inventory.OnShowInventoryEnd += _changeCameraRendering.OnlyRenderingUI;
            uIGameplayController.UI_Inventory.OnHideInventoryStarts += _changeCameraRendering.BackToDefaultRendering;

            uISaveAndLoad.OnCallWindow += _playerBehaviour.Inputs.BlockControls;
            uISaveAndLoad.OnCursorState += _playerBehaviour.HideCursor;
            uISaveAndLoad.OnUnCallWindow += _playerBehaviour.Inputs.AllowControls;

            for(int i = 0; i < uISaveAndLoad.SaveSlots.Length; i++)
            {
                uISaveAndLoad.SaveSlots[i].OnSaveFile += saveManager.SaveGame;
                uISaveAndLoad.SaveSlots[i].OnLoadFile += loaderManager.Load;

                uISaveAndLoad.SaveSlots[i].OnSavingState += saveManager.GetSavingState;
                uISaveAndLoad.SaveSlots[i].OnGetSlotInfo += saveManager.GetSlotInformation;
                uISaveAndLoad.SaveSlots[i].OnGetSlotScreenshoot += saveManager.GetSlotScreenshoot;
            }
        }

        private void ScenarioLoaderEnableEvents()
        {
            scenarioLoaderManager.OnStartTravel += pauseManager.PauseWithoutShowingCursor;
            scenarioLoaderManager.OnStartTravel += _playerBehaviour.Inputs.BlockControls;
            scenarioLoaderManager.OnStartTravel += uIFadeController.FadeIn;

            scenarioLoaderManager.OnFlipPlayer += _playerBehaviour.FlipPlayer;

            scenarioLoaderManager.OnEndTravel += pauseManager.UnPause;
            scenarioLoaderManager.OnEndTravel += _playerBehaviour.Inputs.AllowControls;
            scenarioLoaderManager.OnEndTravel += uIFadeController.FadeOut;
        }

        private void InventoryEnableEvents()
        {
            inventoryManager.OnModifyInventory += uIGameplayController.UI_Inventory.ModifyItems;
        }

        private void PauseEnableEvents()
        {
            pauseManager.OnPaused += _playerBehaviour.Inputs.BlockInputsWhenPaused;
            pauseManager.OnPausedWithoutShowingCursor += _playerBehaviour.Inputs.BlockInputsWhenPausedNoCursor;
            pauseManager.OnUnPaused += _playerBehaviour.Inputs.AllowInputsWhenUnPaused;
        }

        private void PlayerEnableEvents()
        {
            _playerBehaviour.Inputs.SubscribeInventory(uIGameplayController.UI_Inventory.CallInventory);
            _playerBehaviour.Inputs.SubscribePause(uIGameplayController.UI_PauseGame.CallPause);

            _playerBehaviour.Status.OnModifingHealth += uIGameplayController.UI_HurtAlertOverlay.CheckAlertOverlay;
            _playerBehaviour.Status.OnTakingDamage += _cameraShake.HittedShake;
            _playerBehaviour.Status.OnDeath += gameManager.GameOver;

            _playerBehaviour.Resources.OnRefreshWeaponUI += uIGameplayController.UI_RangedWeapon.RefreshWeaponInfo;
            _playerBehaviour.Resources.OnRefreshBottlesUI += uIGameplayController.UI_HealingBottles.RefreshHealingInfo;
        }

        private void OnDisable()
        {
            PlayerDisableEvents();

            PauseDisableEvents();

            InventoryDisableEvents();

            if (_unlockDoorsTriggers != null && _unlockDoorsTriggers.Length > 0)
            {
                foreach (UnlockDoorTrigger unlockDoor in _unlockDoorsTriggers)
                {
                    unlockDoor.OnCheckIfPlayerHasTheItem -= inventoryManager.CheckIfHasItem;
                    unlockDoor.OnRemoveItemFromInventory -= inventoryManager.RemoveKeyItem;
                    unlockDoor.OnUnlockedDoor += null;
                }
            }

            if (_puzzleInteractionTriggers != null && _puzzleInteractionTriggers.Length > 0)
            {
                foreach (PuzzleInteractionTrigger puzzleUnlock in _puzzleInteractionTriggers)
                {
                    puzzleUnlock.OnReceiveItemFromInventory -= inventoryManager.AddKeyItem;
                    puzzleUnlock.OnCheckIfPlayerHasTheItem -= inventoryManager.CheckIfHasItem;
                    puzzleUnlock.OnRemoveItemFromInventory -= inventoryManager.RemoveKeyItem;
                }
            }

            ChaserManagerDisableEvents();

            ScenarioLoaderDisableEvents();

            UIDisableEvents();

            GameManagerDisableEvents();

            SaveManagerDisableEvents();

            LoaderManagerDisableEvents();

            if(pursuerSearchingState != null)
            {
                pursuerSearchingState.OnChasingStarted -= musicManager.PlayerChasing;
            }
        }

        private void LoaderManagerDisableEvents()
        {
            loaderManager.OnStartLoadSaveGame -= _playerBehaviour.Inputs.BlockActions;
            loaderManager.OnLoadSaveGame -= scenarioLoaderManager.LoadScene;
        }

        private void SaveManagerDisableEvents()
        {
            saveManager.OnGetCurrentChapter -= gameManager.GetCurrentChapter;
            saveManager.OnGetCurrentLevel -= scenarioLoaderManager.GetCurrentScene;
            saveManager.OnGetCurrentRoom -= scenarioLoaderManager.GetCurrentRoom;

            saveManager.OnGetPlayerCurrentPos -= _playerBehaviour.GetCurrentPosistion;

            saveManager.OnGetPlayerCurrentHealth -= _playerBehaviour.Status.GetCurrentHealth;

            saveManager.OnGetPlayerCurrentHealthBottles -= _playerBehaviour.Resources.CurrentHealingBottles;
            saveManager.OnGetPlayerCurrentBullets -= _playerBehaviour.Resources.CurrentBullets;
            saveManager.OnGetPlayerCurrentMunition -= _playerBehaviour.Resources.CurrentAmmo;

            saveManager.OnGetPlayerMeleeStatus -= _playerBehaviour.Equipment.HasMeleeWeapon;
            saveManager.OnGetPlayerRangedStatus -= _playerBehaviour.Equipment.HasRangedWeapon;

            saveManager.OnGetInventoryItems -= inventoryManager.GetInventoryItems;

            saveManager.OnGetChaserStatus -= chaserManager.GetChaserStatus;

            saveManager.OnGetChapterEventsTriggereds -= chapterEventsManager.GetAllEndedEvents;
        }

        private void ChaserManagerDisableEvents()
        {
            chaserManager.OnChaserEmitSounds -= musicManager.BlockMusic;
            chaserManager.OnEmitSound -= globalSoundEffects.PlayerAudioNoLoop;
            chaserManager.OnStopEmittingSound -= globalSoundEffects.StopAudioLoop;
            chaserManager.OnChaserStopEmitingSounds -= musicManager.UnBlockMusic;
            chaserManager.OnSpawnNoRules -= globalSoundEffects.PlayAudioOneShootWithDelay;
            chaserManager.OnGetPlayerLastSpawn -= scenarioLoaderManager.ReturnPlayerLastSpawn;

            chaserManager.OnChasingEnd -= musicManager.StopChasing;

            if (chasersStateMachine != null)
            {
                chaserManager.OnChasingEnd -= chasersStateMachine.ChasingStateOver;
            }
        }

        private void GameManagerDisableEvents()
        {
            gameManager.OnLoadRoom -= scenarioLoaderManager.LoadRoom;
            gameManager.OnLoadPlayerHealth -= _playerBehaviour.Status.LoadedPlayerHealth;
            gameManager.OnLoadPlayerResources -= _playerBehaviour.Resources.LoadPlayerResources;
            gameManager.OnLoadPlayerEquipment -= _playerBehaviour.Equipment.LoadPlayerEquipment;
            gameManager.OnLoadPlayerInventory -= inventoryManager.LoadPlayerInventory;
            gameManager.OnLoadChapterEndedEvents -= chapterEventsManager.LoadEndedEvents;

            gameManager.OnGameLoadedEnd -= uIAreaNotification.DisableNotificationOnStart;

            gameManager.EnablePlayerControlls -= _playerBehaviour.Inputs.AllowControls;

            gameManager.OnGameStarted -= uIFadeController.CustomFadeOut;
            gameManager.OnGameLoaded -= uIFadeController.CustomFadeOut;

            gameManager.OnGameWinEnded -= scenarioLoaderManager.LoadScene;

            gameManager.OnGameOverStart -= gameOverVolumeProfile.Show;
            gameManager.OnGameOverEnd -= scenarioLoaderManager.GameOverLoadScene;
        }
        
        private void UIDisableEvents()
        {
            uIGameplayController.OnPlaySFX -= globalSoundEffects.PlayAudioOneShoot;

            uIGameplayController.UI_HurtAlertOverlay.OnEnterCriticalHealth -= _playerSoundEffects.PlayAudioLoop;
            uIGameplayController.UI_HurtAlertOverlay.OnLeftCriticalHealth -= _playerSoundEffects.StopAudioLoop;

            uIGameplayController.UI_Inventory.OnCallingInventory -= pauseManager.Pause;
            uIGameplayController.UI_Inventory.OnCallingInventory -= _playerBehaviour.Inputs.BlockInputsWhenInventory;

            uIGameplayController.UI_Inventory.OnUnCallingInventory -= pauseManager.UnPause;
            uIGameplayController.UI_Inventory.OnAllowingMenus -= _playerBehaviour.Inputs.AllowInputsWhenInventory;

            uIGameplayController.UI_Inventory.OnCheckMeleeWeaponState -= _playerBehaviour.Equipment.HasMeleeWeapon;
            uIGameplayController.UI_Inventory.OnCheckRangedWeaponState -= _playerBehaviour.Equipment.HasRangedWeapon;

            uIGameplayController.UI_Inventory.OnCheckHealingBottlesState -= _playerBehaviour.Resources.CurrentHealingBottles;
            uIGameplayController.UI_Inventory.OnCheckWeaponBulletsState -= _playerBehaviour.Resources.CurrentBullets;
            uIGameplayController.UI_Inventory.OnCheckWeaponAmmoState -= _playerBehaviour.Resources.CurrentAmmo;

            uIGameplayController.UI_ItemNotification.OnShowInterface -= pauseManager.Pause;
            uIGameplayController.UI_ItemNotification.OnHideInteface -= pauseManager.UnPause;

            uIGameplayController.UI_ItemNotification.OnAllowControls -= _playerBehaviour.Inputs.AllowControls;
            uIGameplayController.UI_ItemNotification.OnBlockControls -= _playerBehaviour.Inputs.BlockControls;

            uIGameplayController.UI_PauseGame.OnShowInterface -= pauseManager.Pause;
            uIGameplayController.UI_PauseGame.OnShowInterface -= _playerBehaviour.Inputs.BlockInputsWhenPauseMenu;

            uIGameplayController.UI_PauseGame.OnHideInteface -= pauseManager.UnPause;
            uIGameplayController.UI_PauseGame.OnHideInteface -= _playerBehaviour.Inputs.AllowInputsWhenPauseMenu;

            uIGameplayController.UI_Inventory.OnShowInventoryEnd -= _changeCameraRendering.OnlyRenderingUI;
            uIGameplayController.UI_Inventory.OnHideInventoryStarts -= _changeCameraRendering.BackToDefaultRendering;

            uISaveAndLoad.OnCallWindow -= _playerBehaviour.Inputs.BlockControls;
            uISaveAndLoad.OnCursorState -= _playerBehaviour.HideCursor;
            uISaveAndLoad.OnUnCallWindow -= _playerBehaviour.Inputs.AllowControls;

            for(int i = 0; i < uISaveAndLoad.SaveSlots.Length; i++)
            {
                uISaveAndLoad.SaveSlots[i].OnSaveFile -= saveManager.SaveGame;
                uISaveAndLoad.SaveSlots[i].OnLoadFile -= loaderManager.Load;

                uISaveAndLoad.SaveSlots[i].OnSavingState -= saveManager.GetSavingState;
                uISaveAndLoad.SaveSlots[i].OnGetSlotInfo -= saveManager.GetSlotInformation;
                uISaveAndLoad.SaveSlots[i].OnGetSlotScreenshoot -= saveManager.GetSlotScreenshoot;
            }
        }

        private void ScenarioLoaderDisableEvents()
        {
            scenarioLoaderManager.OnStartTravel -= pauseManager.PauseWithoutShowingCursor;
            scenarioLoaderManager.OnStartTravel -= _playerBehaviour.Inputs.BlockControls;
            scenarioLoaderManager.OnStartTravel -= uIFadeController.FadeIn;

            scenarioLoaderManager.OnFlipPlayer -= _playerBehaviour.FlipPlayer;

            scenarioLoaderManager.OnEndTravel -= pauseManager.UnPause;
            scenarioLoaderManager.OnEndTravel -= _playerBehaviour.Inputs.AllowControls;
            scenarioLoaderManager.OnEndTravel -= uIFadeController.FadeOut;
        }

        private void InventoryDisableEvents()
        {
            inventoryManager.OnModifyInventory -= uIGameplayController.UI_Inventory.ModifyItems;
        }

        private void PauseDisableEvents()
        {
            pauseManager.OnPaused -= _playerBehaviour.Inputs.BlockInputsWhenPaused;
            pauseManager.OnPausedWithoutShowingCursor -= _playerBehaviour.Inputs.BlockInputsWhenPausedNoCursor;
            pauseManager.OnUnPaused -= _playerBehaviour.Inputs.AllowInputsWhenUnPaused;
        }

        private void PlayerDisableEvents()
        {
            _playerBehaviour.Inputs.UnsubscribeInventory();
            _playerBehaviour.Inputs.UnsubscribePause();

            _playerBehaviour.Status.OnModifingHealth -= uIGameplayController.UI_HurtAlertOverlay.CheckAlertOverlay;
            _playerBehaviour.Status.OnTakingDamage -= _cameraShake.HittedShake;
            _playerBehaviour.Status.OnDeath -= gameManager.GameOver;

            _playerBehaviour.Resources.OnRefreshWeaponUI -= uIGameplayController.UI_RangedWeapon.RefreshWeaponInfo;
            _playerBehaviour.Resources.OnRefreshBottlesUI -= uIGameplayController.UI_HealingBottles.RefreshHealingInfo;
        }
    }
}
