using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public sealed class UIInventory : MonoBehaviour
    {
        #region Encapsulation
        public delegate void CallingInventory();
        public event CallingInventory OnCallingInventory;

        public delegate void ShowInventoryEnd();
        public event ShowInventoryEnd OnShowInventoryEnd;

        public delegate void UnCallingInventory();
        public event UnCallingInventory OnUnCallingInventory;

        public delegate void HideInventoryStarts();
        public event HideInventoryStarts OnHideInventoryStarts;
        #endregion

        [Header("Classes")]
        [SerializeField] private CanvasGroup inventoryFadeCanvas;

        [Space(10)]

        [SerializeField] private GameObject inventoryGroup;
        [SerializeField] private GameObject noItemsGroup;

        [Space(6)]

        [SerializeField] private GameObject keyInventoryGroup;

        [Space(10)]

        [SerializeField] private TextMeshProUGUI itemNameTMP;
        [SerializeField] private TextMeshProUGUI itemDescriptionTMP;

        [Space(6)]

        [SerializeField] private Image itemImage;

        [Space(10)]

        [SerializeField] private Button nextItemButton;
        [SerializeField] private Button previousItemButton;

        [Header("Settings")]
        [SerializeField] private List<ItemData> keyInventoryList = new List<ItemData>();

        [Space(10)]

        [SerializeField] [Range(0.1f, 1f)] private float fadeDuration = 0.4f;

        private bool _inTransition;
        private bool _showingInventory;

        private int _maxItemToSelect;
        private int _currentItemSelected;

        private void OnEnable() 
        {
            if(keyInventoryList.Count <= 0)
            {
                noItemsGroup.SetActive(true);
                inventoryGroup.SetActive(false);
                keyInventoryGroup.SetActive(false);

                nextItemButton.gameObject.SetActive(true);
                previousItemButton.gameObject.SetActive(false);
            }

            _currentItemSelected = 0;

            if(inventoryFadeCanvas.alpha > 0f) { inventoryFadeCanvas.alpha = 0f; }
        }

        public void CallInventory()
        {
            if(_inTransition) return;

            _showingInventory = !_showingInventory;

            _inTransition = true;

            if(_showingInventory)
            {
                ShowFade();
            }
            else
            {
                HideFade();
            }
        }

        public void ModifyItems(bool increase, ref ItemData itemData)
        {
            if(increase)
            {
                if(!keyInventoryList.Contains(itemData))
                {
                    keyInventoryList.Add(itemData);
                }
            }
            else
            {
                if(keyInventoryList.Contains(itemData))
                {
                    keyInventoryList.Remove(itemData);
                }
            }

            if(keyInventoryList.Count <= 0)
            {
                _maxItemToSelect = keyInventoryList.Count;
            }
            else
            {
                _maxItemToSelect = keyInventoryList.Count-1;
            }

            RefreshInventory();
        }

        public void NextInventoryItem()
        {
            if(_currentItemSelected == _maxItemToSelect) return;

            _currentItemSelected++;

            if(_currentItemSelected >= _maxItemToSelect)
            {
                _currentItemSelected = _maxItemToSelect;
            }

            RefreshCurrentItem();
        }

        public void PreviousInventoryItem()
        {
            if(_currentItemSelected == 0) return;

            _currentItemSelected--;

            if(_currentItemSelected <= 0)
            {
                _currentItemSelected = 0;
            }

            RefreshCurrentItem();
        }

        private void ShowFade()
        {
            OnCallingInventory?.Invoke();

            inventoryFadeCanvas.DOKill();
            inventoryFadeCanvas.DOFade(1f,fadeDuration).OnComplete(
                () => 
                { 
                    inventoryFadeCanvas.blocksRaycasts = true; 
                    inventoryFadeCanvas.interactable = true; 
                }).SetUpdate(true).OnComplete(() => { inventoryGroup.SetActive(true); ShowInventory(); });
        }

        private void ShowInventory()
        {
            RefreshInventory();

            inventoryFadeCanvas.DOFade(0f, fadeDuration).SetUpdate(true).OnComplete(
                () => { _inTransition = false; OnShowInventoryEnd?.Invoke(); }).SetDelay(0.2f);
        }

        private void RefreshInventory()
        {
            _currentItemSelected = 0;
                           
            if(keyInventoryList.Count > 0)
            {
                itemNameTMP.text = keyInventoryList[0].Name;
                itemDescriptionTMP.text = keyInventoryList[0].Description;

                itemImage.sprite = keyInventoryList[0].Icon;

                if(noItemsGroup.gameObject.activeInHierarchy && !keyInventoryGroup.gameObject.activeInHierarchy)
                {
                    noItemsGroup.SetActive(false);
                    keyInventoryGroup.SetActive(true);
                }
            }
            else if(keyInventoryList.Count <= 0)
            {
                noItemsGroup.SetActive(true);
                keyInventoryGroup.SetActive(false);
            }

            ResetInventoryButtons();
        }

        private void ResetInventoryButtons()
        {
            if(_maxItemToSelect > 0)
            {
                nextItemButton.gameObject.SetActive(true);
                previousItemButton.gameObject.SetActive(false);
            }
            else if(_maxItemToSelect <= 0)
            {
                nextItemButton.gameObject.SetActive(false);
                previousItemButton.gameObject.SetActive(false);
            }
        }

        private void RefreshCurrentItem()
        {
            itemNameTMP.text = keyInventoryList[_currentItemSelected].Name;
            itemDescriptionTMP.text = keyInventoryList[_currentItemSelected].Description;

            itemImage.sprite = keyInventoryList[_currentItemSelected].Icon;

            RefreshInventoryButtons();
        }

        private void RefreshInventoryButtons()
        {
            if(_currentItemSelected < 1)
            {
                nextItemButton.gameObject.SetActive(true);
                previousItemButton.gameObject.SetActive(false);

                return;
            }

            if(_currentItemSelected == _maxItemToSelect)
            {
                nextItemButton.gameObject.SetActive(false);
                previousItemButton.gameObject.SetActive(true);

            }
            else if(_currentItemSelected > 0)
            {
                nextItemButton.gameObject.SetActive(true);
                previousItemButton.gameObject.SetActive(true);

            }
        }

        private void HideFade()
        {
            OnHideInventoryStarts?.Invoke();

            inventoryFadeCanvas.blocksRaycasts = false; 
            inventoryFadeCanvas.interactable = false;

            inventoryFadeCanvas.DOKill();
            inventoryFadeCanvas.DOFade(1f, fadeDuration).OnComplete(() => { inventoryGroup.SetActive(false); HideInventory(); })
                .SetUpdate(true);
        }

        private void HideInventory()
        {
            OnUnCallingInventory?.Invoke();

            inventoryFadeCanvas.DOKill();
            inventoryFadeCanvas.DOFade(0f, fadeDuration).OnComplete(() => { _inTransition = false; });
        }
    }
}
