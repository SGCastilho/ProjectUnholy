using Core.UI;
using Core.Player;
using UnityEngine;

namespace Core.Events
{
    public sealed class GameplayEvents : MonoBehaviour
    {
        [Header("UI Classes")]
        [SerializeField] private UIGameplayController uIGameplayController;

        private PlayerBehaviour playerBehaviour;

        private void Awake() 
        {
            playerBehaviour = FindObjectOfType<PlayerBehaviour>();
        }

        private void OnEnable() 
        {
            playerBehaviour.Status.OnModifingHealth += uIGameplayController.UI_HurtAlertOverlay.CheckAlertOverlay;

            playerBehaviour.Resources.OnRefreshUI += uIGameplayController.UI_RangedWeapon.RefreshWeaponInfo;
        }

        private void OnDisable() 
        {
            playerBehaviour.Status.OnModifingHealth -= uIGameplayController.UI_HurtAlertOverlay.CheckAlertOverlay;

            playerBehaviour.Resources.OnRefreshUI -= uIGameplayController.UI_RangedWeapon.RefreshWeaponInfo;
        }
    }
}
