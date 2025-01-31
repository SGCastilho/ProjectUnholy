using System.Collections.Generic;
using Core.ScriptableObjects;
using UnityEngine;

namespace Core.Managers
{
    public sealed class InventoryManager : MonoBehaviour
    {
        #region Events
        public delegate void ModifyInventory(bool increase, ref ItemData itemData);
        public ModifyInventory OnModifyInventory;
        #endregion

        private Dictionary<string, ItemData> _keyItemInventory = new Dictionary<string, ItemData>();

        public void AddKeyItem(ItemData keyItemData)
        {
            if(keyItemData == null || _keyItemInventory.ContainsKey(keyItemData.NameKey) || keyItemData.Type != ItemType.KEY_ITEM) return;

            _keyItemInventory.Add(keyItemData.NameKey, keyItemData);

            OnModifyInventory?.Invoke(true, ref keyItemData);
        }

        public void RemoveKeyItem(ItemData keyItemData)
        {
            if(keyItemData == null || !_keyItemInventory.ContainsKey(keyItemData.NameKey) || keyItemData.Type != ItemType.KEY_ITEM) return;

            _keyItemInventory.Remove(keyItemData.NameKey);

            OnModifyInventory?.Invoke(false, ref keyItemData);
        }

        public bool CheckIfHasItem(ItemData itemToCheck)
        {
            if(_keyItemInventory == null || _keyItemInventory.Count < 1)
            {
                return false;
            }

            if(_keyItemInventory.ContainsKey(itemToCheck.NameKey))
            {
                return true;
            }
            
            return false;
        }

        public string[] GetInventoryItems()
        {
            List<string> items = new List<string>(_keyItemInventory.Keys);

            return items.ToArray();
        }

        public void LoadPlayerInventory(string[] loadedInventory)
        {
            List<ItemData> existingItems = new List<ItemData>();

            var keyItemsToTranslate = Resources.LoadAll<ItemData>("ScriptableObjects/Items/Inventory/Keys");
            var puzzleItemsToTranslate = Resources.LoadAll<ItemData>("ScriptableObjects/Items/Inventory/Puzzles");
            var abstractItemsToTranslate = Resources.LoadAll<ItemData>("ScriptableObjects/Items/Abstract");

            existingItems.AddRange(keyItemsToTranslate);
            existingItems.AddRange(puzzleItemsToTranslate);
            existingItems.AddRange(abstractItemsToTranslate);

            foreach(ItemData loadedItem in existingItems)
            {
                for(int i = 0; i < loadedInventory.Length; i++)
                {
                    if(loadedItem.NameKey == loadedInventory[i])
                    {
                        AddKeyItem(loadedItem);
                        break;
                    }
                }
            }
        }
    }
}
