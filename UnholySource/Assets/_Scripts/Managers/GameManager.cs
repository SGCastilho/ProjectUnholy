using System;
using UnityEngine;

namespace Core.Managers
{
    public sealed class GameManager : MonoBehaviour
    {
        #region Events
        public delegate void GameStarted(float fadeDuration, float delay, Action OnFadeEnded);
        public event GameStarted OnGameStarted;

        public delegate void GameLoaded(float fadeDuration, float delay, Action OnFadeEnded);
        public event GameLoaded OnGameLoaded;
        #endregion

        #region Actions
        public Action EnablePlayerControlls;
        #endregion

        [Header("Settings")]
        [SerializeField] private bool gameLoaded;

        [Space(10)]

        [SerializeField] [Range(2f, 10f)] private float chapterStartedFadeOut = 4f;
        [SerializeField] [Range(1f, 6f)] private float chapterStartedFadeOutDelay = 1f;

        [Space(5)]

        [SerializeField] [Range(1f, 4f)] private float chapterLoadedFadeOut = 1f;
        [SerializeField] [Range(1f, 6f)] private float chapterLoadedFadeOutDelay = 1f;

        private ChapterEventsManager _chapterEventsManager;

        private void Awake() => CacheVariables();

        private void CacheVariables()
        {
            _chapterEventsManager = FindObjectOfType<ChapterEventsManager>();
        }

        private void OnEnable() 
        {
            CheckForSaveFileLoader();

            GameStart();
        }

        private void CheckForSaveFileLoader()
        {
            //Checka se existe umas instancia do save file loader, e pega todas suas informações para aplica-los na cena
            //Se o mesmo não existir, toda essa função sera ignorada e o jogo será iniciado

            //DEBUG
            _chapterEventsManager.ExecuteEndedEvents();
        }

        public void GameStart()
        {
            if(!gameLoaded)
            {
                OnGameStarted?.Invoke(chapterStartedFadeOut, chapterStartedFadeOutDelay, EnablePlayerControlls);
            }
            else
            {
                OnGameLoaded?.Invoke(chapterLoadedFadeOut, chapterLoadedFadeOutDelay, EnablePlayerControlls);
            }
        }

        public void GameOver()
        {
            Debug.Log("Game Over");
        }
    }
}
