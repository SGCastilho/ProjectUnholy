using TMPro;
using Core.ScriptableObjects;
using DG.Tweening;
using UnityEngine;
using System.Collections;

namespace Core.UI
{
    public sealed class UIDialogue : MonoBehaviour
    {
        #region Events
        public delegate void EnterDialogue();
        public event EnterDialogue OnEnterDialogue;

        public delegate void ExitDialogue();
        public event ExitDialogue OnExitWindow;
        #endregion

        [Header("Classes")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space(10)]

        [SerializeField] private TextMeshProUGUI characterNameTMP;
        [SerializeField] private TextMeshProUGUI dialogueTMP;

        [Header("Settings")]
        [SerializeField] private bool inDialogue;
        [SerializeField] [Range(0.1f , 0.2f)] private float fadeDuration = 0.2f;

        private DialogueData _currentDialogueData;
        
        private Coroutine _dialogueCouroutine;

        private int _currentDialogue;
        private int _currentDialogueTree;

        private bool _inDialogueCouroutine;

        void Start() => dialogueTMP.text = string.Empty;

        public void CallDialogue(DialogueData dialogueData)
        {
            if(inDialogue) return;

            _currentDialogueData = dialogueData;

            characterNameTMP.text = _currentDialogueData.DialogueTree[0].Name;

            inDialogue = true;

            OnEnterDialogue?.Invoke();

            FadeIn();
        }

        public void NextDialogue()
        {
            if(!inDialogue) return;

            if(_inDialogueCouroutine) 
            { 
                StopCoroutine(_dialogueCouroutine);

                _inDialogueCouroutine = false;
                
                dialogueTMP.text = _currentDialogueData.DialogueTree[_currentDialogueTree].Dialogues[_currentDialogue];

                return;
            }

            _currentDialogue++;

            if(_currentDialogue >= _currentDialogueData.DialogueTree[_currentDialogueTree].Dialogues.Length)
            {
                _currentDialogue = 0;
                _currentDialogueTree++;

                if(_currentDialogueTree >= _currentDialogueData.DialogueTree.Length)
                {
                    UnCallDialogue();
                }
                else
                {
                    characterNameTMP.text = _currentDialogueData.DialogueTree[_currentDialogueTree].Name;
                    _dialogueCouroutine = _dialogueCouroutine = StartCoroutine(DialogueParcing(_currentDialogueData
                        .DialogueTree[_currentDialogueTree].Dialogues[_currentDialogue].ToCharArray()));
                }
            }
            else
            {
                _dialogueCouroutine = StartCoroutine(DialogueParcing(_currentDialogueData.DialogueTree[_currentDialogueTree]
                    .Dialogues[_currentDialogue].ToCharArray()));
            }
        }

        IEnumerator DialogueParcing(char[] dialogueChars)
        {
            _inDialogueCouroutine = true;

            int characters = 0;
            dialogueTMP.text = string.Empty;

            while(characters < dialogueChars.Length)
            {
                dialogueTMP.text += dialogueChars[characters];
                characters++;

                yield return null;
            }

            _inDialogueCouroutine = false;
        }

        public void UnCallDialogue()
        {
            _currentDialogue = 0;
            _currentDialogueTree = 0;

            OnExitWindow?.Invoke();

            FadeOut();
        }

        private void FadeIn()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, fadeDuration).OnComplete(() => 
            {
                _dialogueCouroutine = StartCoroutine(DialogueParcing(_currentDialogueData.DialogueTree[0].Dialogues[0].ToCharArray()));
            });
        }

        private void FadeOut()
        {
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeDuration).OnComplete(() => { inDialogue = false; dialogueTMP.text = string.Empty; });
        }
    }
}
