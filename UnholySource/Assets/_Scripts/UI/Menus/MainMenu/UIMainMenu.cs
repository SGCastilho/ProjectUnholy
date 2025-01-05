using UnityEngine;

namespace Core.UI
{
    public sealed class UIMainMenu : MonoBehaviour
    {
        #region Encapsulation
        public UIPressAnyButton UI_PressAnyButton { get => uIPressAnyButton; }
        #endregion

        [Header("Classes")]
        [SerializeField] private UIPressAnyButton uIPressAnyButton;
    }
}
