using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public sealed class UIInventory : MonoBehaviour
    {
        #region Encapsulation
        public delegate void CallingInventory();
        public event CallingInventory OnCallingInventory;

        public delegate void UnCallingInventory();
        public event UnCallingInventory OnUnCallingInventory;
        #endregion

        [Header("Classes")]
        [SerializeField] private CanvasGroup inventoryCanvas;

        [Header("Settings")]
        [SerializeField] [Range(0.1f, 1f)] private float fadeDuration = 0.4f;

        private bool _inTransition;
        private bool _showingInventory;

        public void CallInventory()
        {
            if(_inTransition) return;

            _showingInventory = !_showingInventory;

            _inTransition = true;

            if(_showingInventory)
            {
                ShowInventory();
            }
            else
            {
                HideInventory();
            }
        }

        private void ShowInventory()
        {
            OnCallingInventory?.Invoke();

            inventoryCanvas.DOKill();
            inventoryCanvas.DOFade(1f,fadeDuration).OnComplete(
                () => { inventoryCanvas.blocksRaycasts = true; inventoryCanvas.interactable = true; _inTransition = false; }).SetUpdate(true);
        }

        private void HideInventory()
        {
            OnUnCallingInventory?.Invoke();

            inventoryCanvas.blocksRaycasts = false; 
            inventoryCanvas.interactable = false;

            inventoryCanvas.DOKill();
            inventoryCanvas.DOFade(0f, fadeDuration).OnComplete(() => { _inTransition = false; });
        }
    }
}
