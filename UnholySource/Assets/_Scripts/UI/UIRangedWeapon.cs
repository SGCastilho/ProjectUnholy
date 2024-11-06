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

        [Space(6)]

        [SerializeField] [Range(1f, 8f)] private float overlayDurationInScreen = 6f;

        private float _overlayCurrentTime;

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
            canvasGroup.DOFade(1f, fadeDuration);
        }

        public void HideOverlay()
        {
            if(canvasGroup.alpha <= 0f) return;

            canvasGroup.DOKill();
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
