using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Managers
{
    public sealed class MainMenuManager : MonoBehaviour
    {
        #region Events
        public delegate void CheckIfHasSaveFile(bool enable);
        public event CheckIfHasSaveFile OnCheckIfHasSaveFile;

        public delegate void StartPlaying(Action ExecuteOnComplete);
        public event StartPlaying OnStartPlaying;
        #endregion

        [Header("Classes")]
        [SerializeField] private LoaderManager loaderManager;

        [Header("Settings")]
        [SerializeField] private string newGameSceneToLoad = "put your scene name here";
        
        private void OnEnable() 
        {
            OnCheckIfHasSaveFile?.Invoke(loaderManager.SaveFileExists());
        }

        private void Start() 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void LoadNewGame()
        {
            OnStartPlaying?.Invoke(NewGameScene);
        }

        private void NewGameScene()
        {
            SceneManager.LoadScene(newGameSceneToLoad);
        }

        public void LoadSaveFile()
        {
            OnStartPlaying?.Invoke(LoadSaveScene);
        }

        private void LoadSaveScene()
        {
            loaderManager.Load();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
