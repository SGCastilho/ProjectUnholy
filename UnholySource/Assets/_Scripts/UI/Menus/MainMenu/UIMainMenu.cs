using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public sealed class UIMainMenu : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private CanvasGroup logoCanvasGroup;
        [SerializeField] private CanvasGroup buttonsCanvasGroup;
        [SerializeField] private CanvasGroup fadeCanvasGroup;

        [Space(10)]

        [SerializeField] private Transform cameraTransform;

        [Space(10)]

        [SerializeField] private CanvasGroup creditsCanvasGroup;
        [SerializeField] private RectTransform creditsRollTransform;

        [Space(10)]

        [SerializeField] private Button loadButton;

        [Header("Settings")]
        [SerializeField] [Range(0.1f, 1f)] private float fadeDuration = 0.4f;
        [SerializeField] [Range(0.1f, 1f)] private float newGameFadeDuration = 1.4f;
        [SerializeField] private Vector3 cameraMovement;

        [Space(10)]

        [SerializeField] private int creditsEndPos = 3194;

        [Space(5)]

        [SerializeField] private float creditsRollSpeed = 20f;

        private Vector3 _creditsStartPosistion;

        private void OnEnable() 
        {
            _creditsStartPosistion = new Vector3(creditsRollTransform.anchoredPosition.x, creditsRollTransform.anchoredPosition.y, 
                creditsRollTransform.position.z);
            
            creditsCanvasGroup.alpha = 0f;
        }

        public void FadeIn()
        {
            logoCanvasGroup.DOKill();
            logoCanvasGroup.DOFade(1f, fadeDuration).OnComplete(() => 
            {
                buttonsCanvasGroup.DOKill();
                buttonsCanvasGroup.DOFade(1f, fadeDuration).OnComplete(() => 
                { 
                    buttonsCanvasGroup.interactable = true; 
                    buttonsCanvasGroup.blocksRaycasts = true;
                });
            });
        }

        public void FadeOut()
        {
            buttonsCanvasGroup.interactable = false; 
            buttonsCanvasGroup.blocksRaycasts = false;
        
            logoCanvasGroup.DOKill();
            buttonsCanvasGroup.DOKill();
            
            logoCanvasGroup.DOFade(0f, fadeDuration);
            buttonsCanvasGroup.DOFade(0f, fadeDuration);
        }

        public void FadeOut(Action ExecuteOnComplete)
        {
            buttonsCanvasGroup.interactable = false; 
            buttonsCanvasGroup.blocksRaycasts = false;
            fadeCanvasGroup.gameObject.SetActive(true);
            
            fadeCanvasGroup.DOKill();
            logoCanvasGroup.DOKill();
            buttonsCanvasGroup.DOKill();

            logoCanvasGroup.DOFade(0f, newGameFadeDuration);
            buttonsCanvasGroup.DOFade(0f, newGameFadeDuration).OnComplete(() => 
            {
                cameraTransform.DOMove(cameraMovement, fadeDuration).SetEase(Ease.InSine);   
                fadeCanvasGroup.DOFade(1f, fadeDuration).OnComplete(() => { ExecuteOnComplete?.Invoke(); });  
            });
        }

        public void LoadFadeOut()
        {
            buttonsCanvasGroup.interactable = false; 
            buttonsCanvasGroup.blocksRaycasts = false;
            fadeCanvasGroup.gameObject.SetActive(true);

            fadeCanvasGroup.DOKill();
            logoCanvasGroup.DOKill();
            buttonsCanvasGroup.DOKill();
            
            logoCanvasGroup.DOFade(0f, fadeDuration);
            buttonsCanvasGroup.DOFade(0f, newGameFadeDuration).OnComplete(() => 
            {
                cameraTransform.DOMove(cameraMovement, fadeDuration).SetEase(Ease.InSine);   
                fadeCanvasGroup.DOFade(1f, fadeDuration); 
            });
        }

        public void CreditsFadeIn()
        {
            creditsCanvasGroup.DOKill();
            creditsCanvasGroup.DOFade(1f, fadeDuration).OnComplete(() => { StartCreditsRoll(); });
        }

        private void StartCreditsRoll()
        {
            creditsRollTransform.DOKill();
            creditsRollTransform.DOLocalMoveY(creditsEndPos, creditsRollSpeed).SetEase(Ease.Linear).OnComplete(() => 
            {
                creditsRollTransform.DOKill();

                creditsRollTransform.gameObject.SetActive(false); 
                creditsRollTransform.localPosition = _creditsStartPosistion;
                creditsRollTransform.gameObject.SetActive(true);

                StartCreditsRoll();
            });
        }

        public void CreditFadeOut()
        {
            creditsCanvasGroup.DOKill();
            creditsCanvasGroup.DOFade(0f, fadeDuration).OnComplete(() => 
            {
                creditsRollTransform.DOKill(); 
                creditsRollTransform.localPosition = _creditsStartPosistion;
            });
        }

        public void EnableLoadButton(bool enable)
        {
            loadButton.interactable = enable;
        }
    }
}
