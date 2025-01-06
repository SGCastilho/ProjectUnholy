using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Managers
{
    public sealed class MainMenuManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string newGameSceneToLoad = "put your scene name here";

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
