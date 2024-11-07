using Core.UI;
using Core.Player;
using Core.Managers;
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

        private void Awake() 
        {
            playerBehaviour = FindObjectOfType<PlayerBehaviour>();
        }

        private void OnEnable() 
        {
            playerBehaviour.Inputs.SubscribeInventory(uIGameplayController.UI_Inventory.CallInventory);

            playerBehaviour.Status.OnModifingHealth += uIGameplayController.UI_HurtAlertOverlay.CheckAlertOverlay;

            playerBehaviour.Resources.OnRefreshUI += uIGameplayController.UI_RangedWeapon.RefreshWeaponInfo;

            pauseManager.OnPaused += playerBehaviour.Inputs.BlockInputsWhenPaused;
            pauseManager.OnUnPaused += playerBehaviour.Inputs.AllowInputsWhenUnPaused;

            uIGameplayController.UI_Inventory.OnCallingInventory += pauseManager.Pause;
            uIGameplayController.UI_Inventory.OnUnCallingInventory += pauseManager.UnPause;
        }

        private void OnDisable() 
        {
            playerBehaviour.Inputs.UnsubscribeInventory();

            playerBehaviour.Status.OnModifingHealth -= uIGameplayController.UI_HurtAlertOverlay.CheckAlertOverlay;

            playerBehaviour.Resources.OnRefreshUI -= uIGameplayController.UI_RangedWeapon.RefreshWeaponInfo;

            pauseManager.OnPaused -= playerBehaviour.Inputs.BlockInputsWhenPaused;
            pauseManager.OnUnPaused -= playerBehaviour.Inputs.AllowInputsWhenUnPaused;

            uIGameplayController.UI_Inventory.OnCallingInventory -= pauseManager.Pause;
            uIGameplayController.UI_Inventory.OnUnCallingInventory -= pauseManager.UnPause;
        }
    }
}
