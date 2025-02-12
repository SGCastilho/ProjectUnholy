using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Managers
{
    public sealed class MainMenuManager : MonoBehaviour
    {
        #region Events
        public delegate void StartPlaying(Action ExecuteOnComplete);
        public event StartPlaying OnStartPlaying;
        #endregion

        [Header("Classes")]
        [SerializeField] private LoaderManager loaderManager;

        [Header("Settings")]
        [SerializeField] private string newGameSceneToLoad = "put your scene name here";

        private void Start() 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void LoadNewGame()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            OnStartPlaying?.Invoke(NewGameScene);
        }

        private void NewGameScene()
        {
            SceneManager.LoadScene(newGameSceneToLoad);
        }

        public void LoadSaveFile()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            OnStartPlaying?.Invoke(LoadSaveScene);
        }

        private void LoadSaveScene()
        {
            
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
