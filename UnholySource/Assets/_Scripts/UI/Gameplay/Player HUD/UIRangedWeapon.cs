using TMPro;
using DG.Tweening;
using UnityEngine;

namespace Core.UI
{
    public sealed class UIRangedWeapon : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space(6)]

        [SerializeField] private TextMeshProUGUI weaponInfoTMP;

        [Header("Settings")]
        [SerializeField] [Range(0.1f, 1f)] private float fadeDuration = 0.4f;
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
            _painelTransform.anchoredPosition = horizontalMovement * Vector2.right;

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

        public void RefreshWeaponInfo(ref int bullets, ref int munition)
        {
            string bulletsText = "";
            string munitionText = "";

            if(bullets > 9)
            {
                bulletsText = bullets.ToString();
            }
            else
            {
                bulletsText = $"0{bullets}";
            }

            if(munition > 99)
            {
                munitionText = munition.ToString();
            }
            else if(munition > 9)
            {
                munitionText = $"0{munition}";
            }
            else 
            {
                munitionText = $"00{munition}";
            }

            weaponInfoTMP.text = $"{bulletsText} | {munitionText}";

            ShowOverlay();
        }
    }
}
