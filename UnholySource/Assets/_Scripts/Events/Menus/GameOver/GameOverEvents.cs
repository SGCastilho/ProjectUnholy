using Core.Managers;
using Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Events
{
    public sealed class GameOverEvents : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private UIGameOver uIGameOver;
        [SerializeField] private GameOverManager gameOverManager;

        [Space(10)]

        [SerializeField] private LoaderManager loaderManager;

        private void OnEnable() 
        {
            gameOverManager.OnOptionSelected += uIGameOver.FadeOut;
            loaderManager.OnLoadSaveGame += SceneManager.LoadScene;
        }

        private void OnDisable() 
        {
            gameOverManager.OnOptionSelected -= uIGameOver.FadeOut;
            loaderManager.OnLoadSaveGame -= SceneManager.LoadScene;
        }
    }
}
