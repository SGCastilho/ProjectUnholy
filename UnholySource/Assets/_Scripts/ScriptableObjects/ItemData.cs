using UnityEngine;

namespace Core.ScriptableObjects
{
    public enum ItemType { KEY_ITEM, HEALING }

    [CreateAssetMenu(fileName = "Item Data", menuName = "Data/New Item Data")]
    public sealed class ItemData : ScriptableObject
    {
        #region Encapsulation
        public ItemType Type { get => itemType; internal set => itemType = value; }

        public string NameKey { get => itemKey; internal set => itemKey = value; }
        public string DescriptionKey { get => itemKeyDescription; internal set => itemKeyDescription = value; }

        public string Name { get => itemName; set => itemName = value; }
        public string Description { get => itemDescription; set => itemDescription = value; }

        public Sprite Icon { get => itemIcon; internal set => itemIcon = value; }

        public int HealingAmount { get => healingAmount; internal set => healingAmount = value; }
        #endregion

        [Header("Settings")]
        [SerializeField] private ItemType itemType = ItemType.KEY_ITEM;

        [Space(20)]

        [SerializeField] private string itemKey = "item_key";
        [SerializeField] private string itemKeyDescription = "item_description_key";

        [Space(10)]

        [SerializeField] private string itemName = "Item Name";
        [SerializeField] private Sprite itemIcon;
        [SerializeField] [Multiline(4)] private string itemDescription = "Put your item description here.";

        [Space(10)]

        [SerializeField] private int healingAmount = 34;

        #region Editor Variable
        #if UNITY_EDITOR
        [Space(20)]

        [SerializeField] [Multiline(4)] internal string devNotes = "Put your dev notes here.";
        #endif
        #endregion
    }
}
