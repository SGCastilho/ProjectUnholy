using TMPro;
using DG.Tweening;
using Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public sealed class UIItemNotificantion : MonoBehaviour
    {
        #region Events
        public delegate void ShowInterface();
        public event ShowInterface OnShowInterface;

        public delegate void BlockControls();
        public event BlockControls OnBlockControls;

        public delegate void AllowControls();
        public event AllowControls OnAllowControls;

        public delegate void HideInteface();
        public event HideInteface OnHideInteface;
        #endregion

        [Header("Classes")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space(10)]

        [SerializeField] private TextMeshProUGUI itemNameTMP;
        [SerializeField] private Image itemImage;

        [Header("Settings")]
        [SerializeField] [Range(0.1f, 1f)] private float fadeDuration = 0.2f;

        public void ShowNotification(ItemData itemToShow)
        {
            itemNameTMP.text = itemToShow.Name;
            itemImage.sprite = itemToShow.Icon;

            OnBlockControls?.Invoke();
            OnShowInterface?.Invoke();

            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, fadeDuration).OnComplete(() => 
            {
                canvasGroup.interactable = true; 
                canvasGroup.blocksRaycasts = true;  
            }).SetUpdate(true);
        }

        public void HideNotification()
        {
            OnAllowControls?.Invoke();
            OnHideInteface?.Invoke();

            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeDuration).OnComplete(() => 
            {
                canvasGroup.interactable = false; 
                canvasGroup.blocksRaycasts = false;  
            }).SetUpdate(true);
        }
    }
}
