using UnityEngine;

namespace Core.ScriptableObjects
{
    public enum ItemType { KEY_ITEM, HEALING }

    [CreateAssetMenu(fileName = "Item Data", menuName = "Data/New Item Data")]
    public sealed class ItemData : ScriptableObject
    {
        #region Encapsulation
        public ItemType Type { get => itemType; }

        public int HealingAmount { get => healingAmount; }
        #endregion

        [Header("Settings")]
        [SerializeField] private ItemType itemType = ItemType.KEY_ITEM;

        [Space(20)]

        [SerializeField] private int healingAmount = 34;
    }
}
