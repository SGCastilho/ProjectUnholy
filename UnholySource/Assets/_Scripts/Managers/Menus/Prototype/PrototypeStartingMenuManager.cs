using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Core.Managers
{
    public sealed class PrototypeStartingMenuManager : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private CanvasGroup slide1Canvas;
        [SerializeField] private CanvasGroup slide2Canvas;

        [Header("Settings")]
        [SerializeField] private string sceneToLoad = "sceneNameHERE";

        [Space(10)]

        [SerializeField] private float timeToNextSlide = 20f;

        private bool _slideFinish;
        private float _currentTimeToNextSlide;

        private void Awake() 
        {
            Application.targetFrameRate = 60;
        }

        private void Start() 
        {
            slide1Canvas.DOFade(1f, 2f).SetDelay(2f);
        }

        private void Update() 
        {
            if(_slideFinish) return;

            _currentTimeToNextSlide += Time.deltaTime;
            if(_currentTimeToNextSlide > timeToNextSlide)
            {
                _slideFinish = true;
                _currentTimeToNextSlide = 0f;

                NextSlide();
            }
        }

        private void NextSlide()
        {
            slide1Canvas.DOFade(0f, 2f).OnComplete(() => { slide2Canvas.DOFade(1f, 2f).SetDelay(2f); });
        }

        public void LoadScene()
        {
            slide2Canvas.blocksRaycasts = false;
            slide2Canvas.interactable = false;

            slide2Canvas.DOKill();
            slide2Canvas.DOFade(0f, 2f).OnComplete(() => 
            {
                SceneManager.LoadScene(sceneToLoad);
            });
        }
    }
}
