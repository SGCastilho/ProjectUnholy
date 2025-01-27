using Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Triggers
{
    public sealed class UnlockDoorTrigger : MonoBehaviour
    {
        #region Events
        public delegate void UnlockedDoor(ref Sprite newSprite);
        public event UnlockedDoor OnUnlockedDoor;

        public delegate bool CheckIfPlayerHasTheItem(ItemData itemToCheck);
        public event CheckIfPlayerHasTheItem OnCheckIfPlayerHasTheItem;

        public delegate void RemoveItemFromInventory(ItemData itemToRemove);
        public event RemoveItemFromInventory OnRemoveItemFromInventory;
        #endregion

        #region Encapsulation
        public bool IsUnlocked 
        { 
            get => doorUnlocked;  
            set 
            {
                doorUnlocked = value;
                if(value == true) { OnUnlockedDoor?.Invoke(ref unlockedSprite); }
            }
        }
        #endregion

        [Header("Data")]
        [SerializeField] private ItemData keyToUnlock;

        [Header("Settings")]
        [SerializeField] private bool doorUnlocked;
        [SerializeField] private Sprite unlockedSprite;

        [Header("Unity Events")]

        [Space(10)]

        [SerializeField] private UnityEvent OnDoorUnlocked;

        [Space(10)]

        [SerializeField] private UnityEvent OnHasBeenUnlocked;

        [Space(10)]

        [SerializeField] private UnityEvent OnDoorLocked;

        public void UnlockDoor()
        {
            if(doorUnlocked)
            {
                OnDoorUnlocked?.Invoke();
            }
            else
            {
                if(OnCheckIfPlayerHasTheItem == null) return;

                bool hasTheItem = OnCheckIfPlayerHasTheItem.Invoke(keyToUnlock);

                if(hasTheItem)
                {
                    doorUnlocked = true;

                    OnUnlockedDoor?.Invoke(ref unlockedSprite);
                    OnRemoveItemFromInventory?.Invoke(keyToUnlock);
                    OnHasBeenUnlocked?.Invoke();
                }
                else
                {
                    OnDoorLocked?.Invoke();
                }
            }
        }
    }
}
