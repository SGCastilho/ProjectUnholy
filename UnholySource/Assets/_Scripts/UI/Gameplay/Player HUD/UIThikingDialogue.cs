using TMPro;
using DG.Tweening;
using UnityEngine;

namespace Core.UI
{
    public sealed class UIThikingDialogue : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space(10)]

        [SerializeField] private TextMeshProUGUI messageTMP;

        [Header("Settings")]
        [SerializeField] private bool emitingMessage;
        [SerializeField] [Range(0.1f, 1f)] private float fadeDuration = 0.2f;

        [Space(10)]

        [SerializeField] [Range(1f , 4f)] private float messageDuration = 1.6f;

        private float _messageDelay;
        private float _currenteMessageTimer;
        private float _currentMessageDuration;

        private string _lastMessage;

        private void OnEnable() 
        {
            _currenteMessageTimer = messageDuration;
        }

        //TROCAR PARA UM SCRIPTABLE OBJECT
        public void EmitMessage(string message)
        {
            if(message == null || message == string.Empty || _lastMessage == message) return;

            if(emitingMessage)
            {
                ResetMessage(message);
            }
            else
            {
                _lastMessage = message;

                messageTMP.text = message;

                FadeIn();
            }
        }

        public void SetDelay(float delay)
        {
            _messageDelay = delay;
        }

        public void SetDuration(float duration)
        {
            _currenteMessageTimer = duration;
        }

        private void Update() 
        {
            if(emitingMessage)
            {
                _currentMessageDuration += Time.deltaTime;
                if(_currentMessageDuration >= _currenteMessageTimer)
                {
                    emitingMessage = false;
                    _currentMessageDuration = 0f;
                    
                    FadeOut();
                }
            }
        }

        private void FadeIn()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, fadeDuration).OnComplete(() => { emitingMessage = true; }).SetDelay(_messageDelay);
        }

        private void FadeOut()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeDuration).OnComplete(() => 
            {                 
                _lastMessage = string.Empty;
                messageTMP.text = string.Empty;

                _messageDelay = 0f;
                _currenteMessageTimer = messageDuration;
            });
        }

        private void ResetMessage(string newMessage)
        {
            emitingMessage = false;

            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeDuration).OnComplete(() => 
            { 
                messageTMP.text = newMessage;

                emitingMessage = true;

                _currenteMessageTimer = messageDuration;
                _currentMessageDuration = 0f;
                _messageDelay = 0f;

                FadeIn();
            });
        }
    }
}
