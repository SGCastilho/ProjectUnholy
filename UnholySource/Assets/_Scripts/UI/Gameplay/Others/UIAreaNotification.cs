using TMPro;
using DG.Tweening;
using Core.ScriptableObjects;
using UnityEngine;

namespace Core.UI
{
    public sealed class UIAreaNotification : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space(10)]

        [SerializeField] private TextMeshProUGUI areaNameTMP;

        [Header("Settings")]
        [SerializeField] private bool emitNotificationOnStart;
        [SerializeField] private TextData areaToNotify;

        [Space(10)]

        [SerializeField] [Range(0.1f , 1f)] private float fadeDuration = 0.2f;
        [SerializeField] [Range(4f, 12f)] private float screenDuration = 8f;

        private bool _showingNotification;
        private float _currentScreenDuration;
        
        private void Start() 
        {
            if(emitNotificationOnStart)
            {
                EmitWithDelayNotification(areaToNotify, 2f);
            }
        }

        private void Update() 
        {
            if(_showingNotification)
            {
                _currentScreenDuration += Time.deltaTime;
                if(_currentScreenDuration >= screenDuration)
                {
                    _showingNotification = false;
                    _currentScreenDuration = 0;

                    FadeOut();
                }
            }
        }

        public void EmitNotification(string areaName)
        {
            if(areaName == string.Empty) return;

            ResetVariables();

            areaNameTMP.text = areaName;

            _showingNotification = true;

            FadeIn();
        }

        public void EmitNotification(TextData areaName)
        {
            if(areaName == null) return;

            ResetVariables();

            areaNameTMP.text = areaName.Value;

            _showingNotification = true;

            FadeIn();
        }

        public void EmitWithDelayNotification(string areaName, float delay)
        {
            if(areaName == string.Empty) return;

            ResetVariables();

            areaNameTMP.text = areaName;

            _showingNotification = true;

            FadeIn(delay);
        }

        public void EmitWithDelayNotification(TextData areaName, float delay)
        {
            if(areaName == null) return;

            ResetVariables();

            areaNameTMP.text = areaName.Value;

            _showingNotification = true;

            FadeIn(delay);
        }

        private void FadeIn()
        {
            canvasGroup.alpha = 0f;

            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, fadeDuration).SetDelay(1f);
        }
        private void FadeIn(float delay)
        {
            canvasGroup.alpha = 0f;

            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, fadeDuration).SetDelay(delay);
        }

        private void FadeOut()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeDuration);
        }

        private void ResetVariables()
        {
            _showingNotification = false;
            _currentScreenDuration = 0;
        }

        public void DisableNotificationOnStart()
        {
            emitNotificationOnStart = false;
        }
    }
}
