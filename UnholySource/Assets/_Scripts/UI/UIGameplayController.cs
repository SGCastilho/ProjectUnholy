using UnityEngine;

namespace Core.UI
{
    public sealed class UIGameplayController : MonoBehaviour
    {
        #region Encapsulation
        public UIRangedWeapon UI_RangedWeapon { get => uIRangedWeapon; }
        public UIHurtAlertOverlay UI_HurtAlertOverlay { get => uIHurtAlertOverlay; }

        public UIInventory UI_Inventory { get => uIInventory; }
        #endregion

        [Header("Player UI Classes")]
        [SerializeField] private UIRangedWeapon uIRangedWeapon;
        [SerializeField] private UIHurtAlertOverlay uIHurtAlertOverlay;

        [Space(10)]

        [SerializeField] private UIInventory uIInventory;
    }
}
