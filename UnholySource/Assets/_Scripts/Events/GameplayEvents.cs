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

        [Header("UI Classes")]
        [SerializeField] private UIGameplayController uIGameplayController;

        private PlayerBehaviour playerBehaviour;

        private CameraShake cameraShake;
        private ChangeCameraRendering changeCameraRendering;

        private void Awake() 
        {
            playerBehaviour = FindObjectOfType<PlayerBehaviour>();

            cameraShake = FindObjectOfType<CameraShake>();
            changeCameraRendering = FindObjectOfType<ChangeCameraRendering>();
        }

        private void OnEnable() 
        {
            playerBehaviour.Inputs.SubscribeInventory(uIGameplayController.UI_Inventory.CallInventory);

            playerBehaviour.Status.OnModifingHealth += uIGameplayController.UI_HurtAlertOverlay.CheckAlertOverlay;
            playerBehaviour.Status.OnTakingDamage += cameraShake.HittedShake;

            playerBehaviour.Resources.OnRefreshUI += uIGameplayController.UI_RangedWeapon.RefreshWeaponInfo;

            pauseManager.OnPaused += playerBehaviour.Inputs.BlockInputsWhenPaused;
            pauseManager.OnUnPaused += playerBehaviour.Inputs.AllowInputsWhenUnPaused;

            inventoryManager.OnModifyInventory += uIGameplayController.UI_Inventory.ModifyItems;

            uIGameplayController.UI_Inventory.OnCallingInventory += pauseManager.Pause;
            uIGameplayController.UI_Inventory.OnUnCallingInventory += pauseManager.UnPause;
            uIGameplayController.UI_ItemNotification.OnShowInterface += pauseManager.Pause;
            uIGameplayController.UI_ItemNotification.OnHideInteface += pauseManager.UnPause;

            uIGameplayController.UI_ItemNotification.OnAllowControls += playerBehaviour.Inputs.AllowControls;
            uIGameplayController.UI_ItemNotification.OnBlockControls += playerBehaviour.Inputs.BlockControls;

            uIGameplayController.UI_Inventory.OnShowInventoryEnd += changeCameraRendering.OnlyRenderingUI;
            uIGameplayController.UI_Inventory.OnHideInventoryStarts += changeCameraRendering.BackToDefaultRendering;
        }

        private void OnDisable() 
        {
            playerBehaviour.Inputs.UnsubscribeInventory();

            playerBehaviour.Status.OnModifingHealth -= uIGameplayController.UI_HurtAlertOverlay.CheckAlertOverlay;
            playerBehaviour.Status.OnTakingDamage -= cameraShake.HittedShake;

            playerBehaviour.Resources.OnRefreshUI -= uIGameplayController.UI_RangedWeapon.RefreshWeaponInfo;

            pauseManager.OnPaused -= playerBehaviour.Inputs.BlockInputsWhenPaused;
            pauseManager.OnUnPaused -= playerBehaviour.Inputs.AllowInputsWhenUnPaused;

            inventoryManager.OnModifyInventory -= uIGameplayController.UI_Inventory.ModifyItems;

            uIGameplayController.UI_Inventory.OnCallingInventory -= pauseManager.Pause;
            uIGameplayController.UI_Inventory.OnUnCallingInventory -= pauseManager.UnPause;
            uIGameplayController.UI_ItemNotification.OnShowInterface -= pauseManager.Pause;
            uIGameplayController.UI_ItemNotification.OnHideInteface -= pauseManager.UnPause;

            uIGameplayController.UI_ItemNotification.OnAllowControls -= playerBehaviour.Inputs.AllowControls;
            uIGameplayController.UI_ItemNotification.OnBlockControls -= playerBehaviour.Inputs.BlockControls;

            uIGameplayController.UI_Inventory.OnShowInventoryEnd -= changeCameraRendering.OnlyRenderingUI;
            uIGameplayController.UI_Inventory.OnHideInventoryStarts -= changeCameraRendering.BackToDefaultRendering;
        }
    }
}
