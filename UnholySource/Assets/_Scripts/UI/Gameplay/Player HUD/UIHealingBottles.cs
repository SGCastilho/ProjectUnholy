using TMPro;
using DG.Tweening;
using Core.ScriptableObjects;
using UnityEngine;

namespace Core.UI
{
    public sealed class UIHealingBottles : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private ItemData healingBottleData;

        [Header("Classes")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space(6)]

        [SerializeField] private TextMeshProUGUI healingInfoTMP;

        [Header("Settings")]
        [SerializeField] [Range(0.1f, 1f)] private float fadeDuration = 0.1f;
        [SerializeField] private float horizontalMovement = 44f;

        [Space(6)]

        [SerializeField] [Range(1f, 8f)] private float overlayDurationInScreen = 6f;
        
        private RectTransform _painelTransform;
        private float _overlayCurrentTime;

        private void Awake() 
        {
            _painelTransform = canvasGroup.GetComponent<RectTransform>();
        }

        private void Update() 
        {
            if(_overlayCurrentTime > 0f)
            {
                _overlayCurrentTime -= Time.deltaTime;
                if(_overlayCurrentTime <= 0f)
                {
                    HideOverlay();

                    _overlayCurrentTime = 0;
                }
            }
        }

        public void ShowOverlay()
        {
            _overlayCurrentTime = overlayDurationInScreen;

            if(canvasGroup.alpha >= 1f) return;

            canvasGroup.DOKill();
            _painelTransform.anchoredPosition = new Vector2(horizontalMovement, _painelTransform.anchoredPosition.y);

            _painelTransform.DOAnchorPosX(0f, fadeDuration);
            canvasGroup.DOFade(1f, fadeDuration);
        }

        public void HideOverlay()
        {
            if(canvasGroup.alpha <= 0f) return;

            canvasGroup.DOKill();

            _painelTransform.DOAnchorPosX(horizontalMovement, fadeDuration);
            canvasGroup.DOFade(0f, fadeDuration);
        }

        public void RefreshHealingInfo(ref int bottlesAmount)
        {
            string bottles = string.Empty;

            if(bottlesAmount <= 9)
            {
                bottles = $"0{bottlesAmount}";
            }
            else if(bottlesAmount > 9)
            {
                bottles = bottlesAmount.ToString();
            }

            healingInfoTMP.text = $"{healingBottleData.Name} : {bottles}";

            ShowOverlay();
        }
    }
}
