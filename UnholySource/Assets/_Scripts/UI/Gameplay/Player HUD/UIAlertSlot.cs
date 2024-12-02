using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public sealed class UIAlertSlot : MonoBehaviour
    {
        #region Events
        public delegate void NotificationEnd(GameObject alert);
        public event NotificationEnd OnNotificationEnd;
        #endregion

        [Header("Classes")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space(10)]

        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemNameTMP;

        [Header("Settings")]
        [SerializeField] [Range(0.1f, 1f)] private float fadeDuration = 0.1f;
        [SerializeField] private float horizontalMovement = 44f;

        [Space(6)]

        [SerializeField] [Range(1f, 8f)] private float overlayDurationInScreen = 6f;

        private RectTransform _painelTransform;
        private float _overlayCurrentTime;

        public void RefreshSlot(Sprite itemSprite, string itemName)
        {
            if(itemSprite == null || itemName == string.Empty) return;

            itemImage.sprite = itemSprite;
            itemNameTMP.text = itemName;
        }

        private void OnEnable() 
        {
            _painelTransform = GetComponent<RectTransform>();
        }
        
        private void Update() 
        {
            if(_overlayCurrentTime > 0f)
            {
                _overlayCurrentTime -= Time.deltaTime;
                if(_overlayCurrentTime <= 0f)
                {
                    HideNotification();

                    _overlayCurrentTime = 0;
                }
            }
        }
        
        public void ShowNotification()
        {
            _overlayCurrentTime = overlayDurationInScreen;

            if(canvasGroup.alpha >= 1f) return;

            canvasGroup.DOKill();
            _painelTransform.anchoredPosition = new Vector2(horizontalMovement, -440f);

            _painelTransform.DOAnchorPosX(600f, fadeDuration);
            canvasGroup.DOFade(1f, fadeDuration);
        }

        public void HideNotification()
        {
            if(canvasGroup.alpha <= 0f) return;

            canvasGroup.DOKill();

            _painelTransform.DOAnchorPosX(horizontalMovement, fadeDuration);
            canvasGroup.DOFade(0f, fadeDuration).OnComplete(() => { OnNotificationEnd?.Invoke(this.gameObject); });
        }
    }
}
