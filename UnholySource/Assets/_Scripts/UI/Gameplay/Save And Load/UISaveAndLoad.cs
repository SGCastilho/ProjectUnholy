using TMPro;
using DG.Tweening;
using UnityEngine;

namespace Core.UI
{
    public sealed class UISaveAndLoad : MonoBehaviour
    {
        #region Events
        public delegate void CallWindow();
        public event CallWindow OnCallWindow;

        public delegate void CursorState(bool hideCursor);
        public event CursorState OnCursorState;

        public delegate void UnCallWindow();
        public event UnCallWindow OnUnCallWindow;
        #endregion

        #region Encapsulation
        public UISaveSlots[] SaveSlots { get => saveSlots; }
        #endregion

        [Header("Classes")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Space(10)]

        [SerializeField] private TextMeshProUGUI windowNameTMP;

        [Space(10)]

        [SerializeField] private UISaveSlots[] saveSlots;

        [Header("Settngs")]
        [SerializeField] [Range(0.1f, 1f)] private float fadeDuration = 0.4f;

        private bool _isSaving;

        void Awake()
        {
            for(int i = 0; i < saveSlots.Length; i++)
            {
                saveSlots[i].Index = i;
            }
        }

        public void ShowSaveWindow()
        {
            _isSaving = true;

            for(int i = 0; i < saveSlots.Length; i++)
            {
                saveSlots[i].IsSaving = true;
            }

            OnCallWindow?.Invoke();
            OnCursorState?.Invoke(false);

            windowNameTMP.text = "Save"; //Temporario, carregar scriptable object traduzido

            FadeIn();
        }

        public void ShowLoadWindow()
        {
            _isSaving = false;

            for(int i = 0; i < saveSlots.Length; i++)
            {
                saveSlots[i].IsSaving = false;
            }

            OnCallWindow?.Invoke();

            windowNameTMP.text = "Load"; //Temporario, carregar scriptable object traduzido

            FadeIn();
        }

        public void HideWindow()
        {
            if(_isSaving)
            {
                OnCursorState?.Invoke(true);
            }
            
            OnUnCallWindow?.Invoke();

            FadeOut();
        }

        private void FadeIn()
        {
            canvasGroup.gameObject.SetActive(true);

            canvasGroup.DOKill();
            canvasGroup.DOFade(1f, fadeDuration).OnComplete(() => { canvasGroup.interactable = true; canvasGroup.blocksRaycasts = true; })
                .SetUpdate(true);
        }

        private void FadeOut()
        {
            canvasGroup.interactable = true; 
            canvasGroup.blocksRaycasts = true;

            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, fadeDuration).OnComplete(() => 
            {
                canvasGroup.gameObject.SetActive(false);
            }).SetUpdate(true);
        }
    }
}
