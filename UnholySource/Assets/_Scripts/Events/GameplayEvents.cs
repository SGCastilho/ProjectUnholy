using Core.UI;
using Core.Player;
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

        private CameraShake cameraShake;
        private ChangeCameraRendering changeCameraRendering;

        private void Awake() => CacheVariables();

        private void CacheVariables()
        {
            playerBehaviour = FindObjectOfType<PlayerBehaviour>();

            cameraShake = FindObjectOfType<CameraShake>();
            changeCameraRendering = FindObjectOfType<ChangeCameraRendering>();
        }

        private void OnEnable()
        {
            PlayerEnableEvents();

            PauseEnableEvents();

            InventoryEnableEvents();

            ScenarioLoaderEnableEvents();

            UIEnableEvents();
        }

        private void UIEnableEvents()
        {
            uIGameplayController.UI_Inventory.OnCallingInventory += pauseManager.Pause;
            uIGameplayController.UI_Inventory.OnUnCallingInventory += pauseManager.UnPause;
            uIGameplayController.UI_ItemNotification.OnShowInterface += pauseManager.Pause;
            uIGameplayController.UI_ItemNotification.OnHideInteface += pauseManager.UnPause;

            uIGameplayController.UI_ItemNotification.OnAllowControls += playerBehaviour.Inputs.AllowControls;
            uIGameplayController.UI_ItemNotification.OnBlockControls += playerBehaviour.Inputs.BlockControls;

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

            playerBehaviour.Status.OnModifingHealth += uIGameplayController.UI_HurtAlertOverlay.CheckAlertOverlay;
            playerBehaviour.Status.OnTakingDamage += cameraShake.HittedShake;

            playerBehaviour.Resources.OnRefreshUI += uIGameplayController.UI_RangedWeapon.RefreshWeaponInfo;
        }

        private void OnDisable()
        {
            PlayerDisableEvents();

            PauseDisableEvents();

            InventoryDisableEvents();

            ScenarioLoaderDisableEvents();

            UIDisableEvents();
        }

        private void UIDisableEvents()
        {
            uIGameplayController.UI_Inventory.OnCallingInventory -= pauseManager.Pause;
            uIGameplayController.UI_Inventory.OnUnCallingInventory -= pauseManager.UnPause;
            uIGameplayController.UI_ItemNotification.OnShowInterface -= pauseManager.Pause;
            uIGameplayController.UI_ItemNotification.OnHideInteface -= pauseManager.UnPause;

            uIGameplayController.UI_ItemNotification.OnAllowControls -= playerBehaviour.Inputs.AllowControls;
            uIGameplayController.UI_ItemNotification.OnBlockControls -= playerBehaviour.Inputs.BlockControls;

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

            playerBehaviour.Status.OnModifingHealth -= uIGameplayController.UI_HurtAlertOverlay.CheckAlertOverlay;
            playerBehaviour.Status.OnTakingDamage -= cameraShake.HittedShake;

            playerBehaviour.Resources.OnRefreshUI -= uIGameplayController.UI_RangedWeapon.RefreshWeaponInfo;
        }
    }
}
