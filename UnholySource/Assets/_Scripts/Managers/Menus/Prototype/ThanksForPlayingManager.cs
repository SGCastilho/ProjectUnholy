using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Managers
{
    public sealed class ThanksForPlayingManager : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Settings")]
        [SerializeField] private float timeToQuit = 30f;

        private float _currentTimeToQuit;

        private void Start() 
        {
            canvasGroup.DOFade(1f, 2f).SetDelay(1f);
        }

        private void Update() 
        {
            if(_currentTimeToQuit > 999f) return;

            _currentTimeToQuit += Time.deltaTime;
            if(_currentTimeToQuit >= timeToQuit)
            {
                _currentTimeToQuit = 999f;

                canvasGroup.DOFade(0f, 2f).OnComplete(() => { SceneManager.LoadScene("Menu_Main"); });
            }
        }
    }
}
