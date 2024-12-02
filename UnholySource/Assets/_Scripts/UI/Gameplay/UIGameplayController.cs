using UnityEngine;

namespace Core.UI
{
    public sealed class UIGameplayController : MonoBehaviour
    {
        #region Encapsulation
        public UIRangedWeapon UI_RangedWeapon { get => uIRangedWeapon; }
        public UIHealingBottles UI_HealingBottles { get => uIHealingBottles; }
        public UIHurtAlertOverlay UI_HurtAlertOverlay { get => uIHurtAlertOverlay; }

        public UIInventory UI_Inventory { get => uIInventory; }

        public UIItemNotificantion UI_ItemNotification { get => uIItemNotificantion; }
        #endregion

        [Header("Player UI Classes")]
        [SerializeField] private UIRangedWeapon uIRangedWeapon;
        [SerializeField] private UIHealingBottles uIHealingBottles;
        [SerializeField] private UIHurtAlertOverlay uIHurtAlertOverlay;

        [Space(10)]

        [SerializeField] private UIInventory uIInventory;

        [Space(10)]

        [SerializeField] private UIItemNotificantion uIItemNotificantion;
    }
}
