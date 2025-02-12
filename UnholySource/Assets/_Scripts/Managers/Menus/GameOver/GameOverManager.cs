using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Managers
{
    public sealed class GameOverManager : MonoBehaviour
    {
        #region Events
        public delegate void OptionSelected(Action ExecuteOnComplete);
        public event OptionSelected OnOptionSelected;
        #endregion

        [Header("Classes")]
        [SerializeField] private LoaderManager loaderManager;

        [Header("Settings")]
        [SerializeField] private string mainMenuScene;
        [SerializeField] private string gameStartingScene;

        private void Start() 
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void Restart()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            OnOptionSelected?.Invoke(LoadGameStartingScene);
        }

        public void Load()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            OnOptionSelected?.Invoke(LoadPreviousSave);
        }

        public void BackToMainMenu()
        {
            OnOptionSelected?.Invoke(LoadMainMenu);
        }

        private void LoadGameStartingScene()
        {
            SceneManager.LoadScene(gameStartingScene);
        }

        private void LoadPreviousSave()
        {
            Debug.Log("Open save");
        }

        private void LoadMainMenu()
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}
