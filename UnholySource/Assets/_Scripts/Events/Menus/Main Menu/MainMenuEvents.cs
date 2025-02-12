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

        [SerializeField] private UISaveAndLoad uISaveAndLoad;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private LoaderManager loaderManager;

        private void OnEnable() 
        {
            mainMenuManager.OnStartPlaying += uIMainMenu.FadeOut;

            loaderManager.OnLoadSaveGame += SceneManager.LoadScene;
            loaderManager.OnFadeOut += uIMainMenu.LoadFadeOut;

            for(int i = 0; i < uISaveAndLoad.SaveSlots.Length; i++)
            {
                uISaveAndLoad.SaveSlots[i].OnLoadFile += loaderManager.LoadWithFade;

                uISaveAndLoad.SaveSlots[i].OnSavingState += saveManager.GetSavingState;
                uISaveAndLoad.SaveSlots[i].OnGetSlotInfo += saveManager.GetSlotInformation;
                uISaveAndLoad.SaveSlots[i].OnGetSlotScreenshoot += saveManager.GetSlotScreenshoot;
            }
        }

        private void OnDisable() 
        {
            mainMenuManager.OnStartPlaying -= uIMainMenu.FadeOut;

            loaderManager.OnLoadSaveGame -= SceneManager.LoadScene;
            loaderManager.OnFadeOut -= uIMainMenu.LoadFadeOut;

            for(int i = 0; i < uISaveAndLoad.SaveSlots.Length; i++)
            {
                uISaveAndLoad.SaveSlots[i].OnLoadFile -= loaderManager.LoadWithFade;

                uISaveAndLoad.SaveSlots[i].OnSavingState -= saveManager.GetSavingState;
                uISaveAndLoad.SaveSlots[i].OnGetSlotInfo -= saveManager.GetSlotInformation;
                uISaveAndLoad.SaveSlots[i].OnGetSlotScreenshoot -= saveManager.GetSlotScreenshoot;
            }
        }
    }
}
