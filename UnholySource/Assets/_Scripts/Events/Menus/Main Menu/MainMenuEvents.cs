using Core.UI;
using Core.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Events
{
    public sealed class MainMenuEvents : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private UIMainMenu uIMainMenu;
        [SerializeField] private MainMenuManager mainMenuManager;

        [Space(10)]

        [SerializeField] private LoaderManager loaderManager;

        private void OnEnable() 
        {
            mainMenuManager.OnCheckIfHasSaveFile += uIMainMenu.EnableLoadButton;

            mainMenuManager.OnStartPlaying += uIMainMenu.FadeOut;

            loaderManager.OnLoadSaveGame += SceneManager.LoadScene;
        }

        private void OnDisable() 
        {
            mainMenuManager.OnCheckIfHasSaveFile -= uIMainMenu.EnableLoadButton;

            mainMenuManager.OnStartPlaying -= uIMainMenu.FadeOut;

            loaderManager.OnLoadSaveGame -= SceneManager.LoadScene;
        }
    }
}
