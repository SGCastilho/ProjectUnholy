using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UI
{
    public sealed class UIGameplayController : MonoBehaviour
    {
        #region Encapsulation
        public UIRangedWeapon UI_RangedWeapon { get => uIRangedWeapon; }
        public UIHealingBottles UI_HealingBottles { get => uIHealingBottles; }
        public UIHurtAlertOverlay UI_HurtAlertOverlay { get => uIHurtAlertOverlay; }

        public UIInventory UI_Inventory { get => uIInventory; }
        public UIPauseGame UI_PauseGame { get => uiPauseGame; }

        public UIItemNotificantion UI_ItemNotification { get => uIItemNotificantion; }
        #endregion

        [Header("Player UI Classes")]
        [SerializeField] private EventSystem eventSystem;

        [Space(10)]

        [SerializeField] private UIRangedWeapon uIRangedWeapon;
        [SerializeField] private UIHealingBottles uIHealingBottles;
        [SerializeField] private UIHurtAlertOverlay uIHurtAlertOverlay;

        [Space(10)]

        [SerializeField] private UIInventory uIInventory;
        [SerializeField] private UIPauseGame uiPauseGame;

        [Space(10)]

        [SerializeField] private UIItemNotificantion uIItemNotificantion;

        public void UnSelectButton()
        {
            eventSystem.SetSelectedGameObject(null);
        }
    }
}
