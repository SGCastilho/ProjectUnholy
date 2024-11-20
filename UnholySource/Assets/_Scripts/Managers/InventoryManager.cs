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
            if(keyItemData == null || _keyItemInventory.ContainsKey(keyItemData.Key) || keyItemData.Type != ItemType.KEY_ITEM) return;

            _keyItemInventory.Add(keyItemData.Key, keyItemData);

            OnModifyInventory?.Invoke(true, ref keyItemData);
        }

        public void RemoveKeyItem(ItemData keyItemData)
        {
            if(keyItemData == null || !_keyItemInventory.ContainsKey(keyItemData.Key) || keyItemData.Type != ItemType.KEY_ITEM) return;

            _keyItemInventory.Remove(keyItemData.Key);

            OnModifyInventory?.Invoke(false, ref keyItemData);
        }

        public bool CheckIfHasItem(ItemData itemToCheck)
        {
            if(_keyItemInventory == null || _keyItemInventory.Count < 1)
            {
                return false;
            }

            if(_keyItemInventory.ContainsKey(itemToCheck.Key))
            {
                return true;
            }
            
            return false;
        }

        public void LoadKeyInventory(ref Dictionary<string, ItemData> inventoryToLoad)
        {
            if(inventoryToLoad == null) return;

            List<string> inventoryKeys = new List<string>();

            foreach(KeyValuePair<string, ItemData> inventory in inventoryToLoad)
            {
                inventoryKeys.Add(inventory.Key);
            }

            for(int i = 0; i < inventoryToLoad.Count; i++)
            {
                AddKeyItem(inventoryToLoad[inventoryKeys[i]]);
            }
        }
    }
}
