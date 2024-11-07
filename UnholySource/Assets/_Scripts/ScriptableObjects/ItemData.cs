using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.ScriptableObjects
{
    public enum ItemType { KEY_ITEM, HEALING }

    [CreateAssetMenu(fileName = "Item Data", menuName = "Data/New Item Data")]
    public sealed class ItemData : ScriptableObject
    {
        #region Encapsulation
        public ItemType Type { get => itemType; internal set => itemType = value; }

        public string Key { get => itemKey; internal set => itemKey = value; }
        public string Name { get => itemName; internal set => itemName = value; }
        public string Description { get => itemDescription; internal set => itemDescription = value; }

        public Sprite Icon { get => itemIcon; internal set => itemIcon = value; }

        public int HealingAmount { get => healingAmount; internal set => healingAmount = value; }
        #endregion

        [Header("Settings")]
        [SerializeField] private ItemType itemType = ItemType.KEY_ITEM;

        [Space(20)]

        [SerializeField] private string itemKey = "item_key";
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

    #region Custom Editor
    #if UNITY_EDITOR
    [CustomEditor(typeof(ItemData))]
    class ItemDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var itemData = (ItemData)target;
            if(itemData == null) return;

            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

            itemData.Type = (ItemType)EditorGUILayout.EnumPopup("Item Type", itemData.Type);

            EditorGUILayout.Space(20);

            if(itemData.Type == ItemType.KEY_ITEM)
            {
                itemData.Key = EditorGUILayout.TextField("Item Key", itemData.Key);
                itemData.Name = EditorGUILayout.TextField("Item Name", itemData.Name);
                itemData.Icon = EditorGUILayout.ObjectField("Item Sprite", itemData.Icon, typeof(Sprite), false) as Sprite;

                EditorGUILayout.LabelField("Item Description");
                itemData.Description = EditorGUILayout.TextArea(itemData.Description, GUILayout.MaxHeight(60));
            }

            if(itemData.Type == ItemType.HEALING)
            {
                itemData.HealingAmount = EditorGUILayout.IntField("Healing Amount", itemData.HealingAmount);
            }

            EditorGUILayout.Space(20);

            EditorGUILayout.LabelField("Dev Notes", EditorStyles.boldLabel);
            itemData.devNotes = EditorGUILayout.TextArea(itemData.devNotes, GUILayout.MaxHeight(60));
        }
    }
    #endif
    #endregion
}
