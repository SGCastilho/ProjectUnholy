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
        [SerializeField] private CanvasGroup backgroundCanvasGroup;
        [SerializeField] private CanvasGroup windowNameCanvasGroup;

        [Space(6)]

        [SerializeField] private CanvasGroup itemNameCanvasGroup;
        [SerializeField] private CanvasGroup itemSpriteCanvasGroup;
        [SerializeField] private CanvasGroup closeButtonCanvasGroup;

        [Space(10)]

        [SerializeField] private TextMeshProUGUI itemNameTMP;
        [SerializeField] private Image itemImage;

        [Header("Settings")]
        [SerializeField] [Range(0.1f, 1f)] private float fadeDuration = 0.2f;
        [SerializeField] [Range(0.1f, 1f)] private float contentFadeDuration = 0.2f;

        [Space(10)]

        [SerializeField] private float windowMovement = 40f;
        [SerializeField] private float itemNameMovement = 340f;

        private RectTransform windowNameRect;
        private RectTransform itemNameRect;
        private RectTransform itemSpriteRect;
        private RectTransform closeButtonRect;

        private void Awake() => CacheVariables();

        private void CacheVariables()
        {
            windowNameRect = windowNameCanvasGroup.GetComponent<RectTransform>();
            itemNameRect = itemNameCanvasGroup.GetComponent<RectTransform>();
            itemSpriteRect = itemSpriteCanvasGroup.GetComponent<RectTransform>();
            closeButtonRect = closeButtonCanvasGroup.GetComponent<RectTransform>();
        }

        public void ShowNotification(ItemData itemToShow)
        {
            itemNameTMP.text = itemToShow.Name;
            itemImage.sprite = itemToShow.Icon;

            OnBlockControls?.Invoke();
            OnShowInterface?.Invoke();

            backgroundCanvasGroup.DOKill();
            backgroundCanvasGroup.DOFade(1f, fadeDuration).OnComplete(() => 
            {
                backgroundCanvasGroup.interactable = true; 
                backgroundCanvasGroup.blocksRaycasts = true;

                ShowWindowName();
            }).SetUpdate(true);
        }

        private void ShowWindowName()
        {
            windowNameCanvasGroup.alpha = 0f;
            windowNameRect.anchoredPosition = new Vector2(-windowMovement, windowNameRect.anchoredPosition.y);

            windowNameCanvasGroup.DOKill();
            windowNameRect.DOKill();

            windowNameCanvasGroup.DOFade(1f, fadeDuration).SetUpdate(true);
            windowNameRect.DOAnchorPosX(0f, fadeDuration).OnComplete(() => { ShowItemSprite(); }).SetUpdate(true);
        }

        private void ShowItemSprite()
        {
            itemSpriteCanvasGroup.alpha = 0f;
            itemSpriteRect.anchoredPosition = new Vector2(itemSpriteRect.anchoredPosition.x, -windowMovement);

            itemSpriteCanvasGroup.DOKill();
            itemSpriteRect.DOKill();

            itemSpriteCanvasGroup.DOFade(1f, contentFadeDuration).SetUpdate(true);
            itemSpriteRect.DOAnchorPosY(0f, contentFadeDuration).OnComplete(() => { ShowItemName(); }).SetUpdate(true);
        }

        private void ShowItemName()
        {
            itemNameCanvasGroup.alpha = 0f;
            itemNameRect.anchoredPosition = new Vector2(itemNameRect.anchoredPosition.x, itemNameMovement);

            itemNameCanvasGroup.DOKill();
            itemNameRect.DOKill();

            itemNameCanvasGroup.DOFade(1f, contentFadeDuration).SetUpdate(true);
            itemNameRect.DOAnchorPosY(300f, contentFadeDuration).OnComplete(() => { ShowCloseButton(); }).SetUpdate(true);
        }

        private void ShowCloseButton()
        {
            closeButtonCanvasGroup.alpha = 0f;
            closeButtonRect.anchoredPosition = new Vector2(closeButtonRect.anchoredPosition.x, -itemNameMovement);

            closeButtonCanvasGroup.DOKill();
            closeButtonRect.DOKill();

            closeButtonCanvasGroup.DOFade(1f, contentFadeDuration).SetUpdate(true);
            closeButtonRect.DOAnchorPosY(-320f, contentFadeDuration).SetUpdate(true);
        }

        public void HideNotification()
        {
            OnAllowControls?.Invoke();
            OnHideInteface?.Invoke();

            backgroundCanvasGroup.DOKill();
            backgroundCanvasGroup.DOFade(0f, fadeDuration).OnComplete(() => 
            {
                HideWindowName();
                HideItemSprite();
                HideItemName();
                HideCloseButton();

                backgroundCanvasGroup.interactable = false; 
                backgroundCanvasGroup.blocksRaycasts = false; 
            }).SetUpdate(true);
        }

        private void HideWindowName()
        {
            windowNameCanvasGroup.alpha = 0f;
            windowNameRect.anchoredPosition = new Vector2(-windowMovement, windowNameRect.anchoredPosition.y);
        }

        private void HideItemSprite()
        {
            itemSpriteCanvasGroup.alpha = 0f;
            itemSpriteRect.anchoredPosition = new Vector2(itemSpriteRect.anchoredPosition.x, -windowMovement);
        }

        private void HideItemName()
        {
            itemNameCanvasGroup.alpha = 0f;
            itemNameRect.anchoredPosition = new Vector2(itemNameRect.anchoredPosition.x, itemNameMovement);
        }

        private void HideCloseButton()
        {
            closeButtonCanvasGroup.alpha = 0f;
            closeButtonRect.anchoredPosition = new Vector2(closeButtonRect.anchoredPosition.x, -itemNameMovement);
        }
    }
}
