using System;
using UnityEngine;

namespace Core.Managers
{
    public sealed class GameManager : MonoBehaviour
    {
        #region Events
        public delegate void LoadRoom(string roomName, Vector3 playerPosition);
        public event LoadRoom OnLoadRoom;

        public delegate void LoadPlayerHealth(int loadedHealth);
        public event LoadPlayerHealth OnLoadPlayerHealth;

        public delegate void LoadPlayerResources(int healthBottles, int bullets, int ammo);
        public event LoadPlayerResources OnLoadPlayerResources;

        public delegate void LoadPlayerEquipment(bool meleeOn, bool rangedOn);
        public event LoadPlayerEquipment OnLoadPlayerEquipment;

        public delegate void LoadPlayerInventory(string[] loadedInventory);
        public event LoadPlayerInventory OnLoadPlayerInventory;

        public delegate void LoadChaserStatus(bool isEnabled);
        public event LoadChaserStatus OnLoadChaserStatus;

        public delegate void LoadChapterEndedEvents(string[] endedEvents);
        public event LoadChapterEndedEvents OnLoadChapterEndedEvents;

        public delegate void GameStarted(float fadeDuration, float delay, Action OnFadeEnded);
        public event GameStarted OnGameStarted;

        public delegate void GameLoaded(float fadeDuration, float delay, Action OnFadeEnded);
        public event GameLoaded OnGameLoaded;
        #endregion

        #region Actions
        public Action EnablePlayerControlls;
        #endregion

        [Header("Settings")]
        [SerializeField] private string currentChapter = "Chapter 1";

        [Space(10)]

        [SerializeField] private bool gameLoaded;

        [Space(10)]

        [SerializeField] [Range(2f, 10f)] private float chapterStartedFadeOut = 4f;
        [SerializeField] [Range(1f, 6f)] private float chapterStartedFadeOutDelay = 1f;

        [Space(5)]

        [SerializeField] [Range(1f, 4f)] private float chapterLoadedFadeOut = 1f;
        [SerializeField] [Range(1f, 6f)] private float chapterLoadedFadeOutDelay = 1f;

        private SaveFileLoaded _saveFileLoaded;
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
            _saveFileLoaded = FindObjectOfType<SaveFileLoaded>();

            if(_saveFileLoaded != null)
            {
                gameLoaded = true;

                Time.timeScale = 0;

                SaveFile loadedSave = _saveFileLoaded.File;
                
                OnLoadRoom?.Invoke(loadedSave.currentLoadedRoom, loadedSave.currentPlayerLocation);

                OnLoadPlayerHealth?.Invoke(loadedSave.currentPlayerHealth);

                OnLoadPlayerResources?.Invoke(loadedSave.currentPlayerHealthBottles, loadedSave.currentPlayerBullets, 
                    loadedSave.currentPlayerMunition);

                OnLoadPlayerEquipment?.Invoke(loadedSave.currentWeaponMeleeStatus, loadedSave.currentWeaponRangedStatus);

                OnLoadPlayerInventory?.Invoke(loadedSave.currentInventoryItems);

                OnLoadChaserStatus?.Invoke(loadedSave.currentCharserEnabled);

                OnLoadChapterEndedEvents?.Invoke(loadedSave.currentTriggeredScenarioEvents);

                Time.timeScale = 1;
            }
            else
            {
                gameLoaded = false;
            }
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

        public string GetCurrentChapter()
        {
            return currentChapter;
        }
    }
}
