using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Managers
{
    public sealed class MainMenuManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string newGameSceneToLoad = "put your scene name here";
        
        private void Start() 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void LoadNewGame()
        {
            SceneManager.LoadScene(newGameSceneToLoad);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
