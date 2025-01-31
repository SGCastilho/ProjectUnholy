using Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Triggers
{
    public sealed class PuzzleInteractionTrigger : MonoBehaviour
    {
        #region Events
        public delegate void ReceiveItemFromInventory(ItemData itemToReceive);
        public event ReceiveItemFromInventory OnReceiveItemFromInventory;

        public delegate bool CheckIfPlayerHasTheItem(ItemData itemToCheck);
        public event CheckIfPlayerHasTheItem OnCheckIfPlayerHasTheItem;

        public delegate void RemoveItemFromInventory(ItemData itemToRemove);
        public event RemoveItemFromInventory OnRemoveItemFromInventory;
        #endregion

        [Header("Data")]
        [SerializeField] private ItemData[] itensToUnlock;

        [Space(10)]

        [SerializeField] private ItemData itemToReceive;

        [Header("Unity Events")]

        [Space(10)]
        [SerializeField] private UnityEvent OnPuzzleLocked;

        [Space(10)]
        [SerializeField] private UnityEvent OnPuzzleUnlocked;

        public void UnlockPuzzle()
        {
            int haveItem = 0;

            for(int i = 0; i < itensToUnlock.Length; i++)
            {
                if(OnCheckIfPlayerHasTheItem?.Invoke(itensToUnlock[i]) == true)
                {
                    haveItem++;
                }
            }

            if(haveItem >= itensToUnlock.Length)
            {
                for(int i = 0; i < itensToUnlock.Length; i++)
                {
                    OnRemoveItemFromInventory?.Invoke(itensToUnlock[i]);
                }

                OnReceiveItemFromInventory?.Invoke(itemToReceive);
                OnPuzzleUnlocked?.Invoke();
            }
            else
            {
                OnPuzzleLocked?.Invoke();
            }
        }
    }
}
