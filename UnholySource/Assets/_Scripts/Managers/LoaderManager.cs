using System.Collections;
using System.IO;
using UnityEngine;

namespace Core.Managers
{
    public sealed class LoaderManager : MonoBehaviour
    {
        #region Events
        public delegate void StartLoadSaveGame();
        public event StartLoadSaveGame OnStartLoadSaveGame;

        public delegate void LoadSaveGame(string sceneToLoad);
        public event LoadSaveGame OnLoadSaveGame;

        public delegate void FadeOut();
        public event FadeOut OnFadeOut;
        #endregion

        [Header("Classes")]
        [SerializeField] private GameObject saveFileLoadedPrefab;

        public void Load(int slotIndex)
        {
            string _savePath = Application.dataPath + $"/Saved/saveFile{slotIndex}.save";

            if(File.Exists(_savePath))
            {
                PlaytimeManager.Instance.DestroyInstance();

                OnStartLoadSaveGame?.Invoke();

                SaveFileLoaded instance = Instantiate(saveFileLoadedPrefab).GetComponent<SaveFileLoaded>();

                string jsonFile = string.Empty;
                SaveFile saveFile = new SaveFile();

                jsonFile = File.ReadAllText(_savePath);
                saveFile = JsonUtility.FromJson<SaveFile>(jsonFile);

                instance.SetupFile(saveFile);

                OnLoadSaveGame?.Invoke(saveFile.currentScene);
            }
        }

        public void LoadWithFade(int slotIndex)
        {
            OnFadeOut?.Invoke();

            StartCoroutine(LoadWithFadeCouroutine(slotIndex));
        }

        private IEnumerator LoadWithFadeCouroutine(int slotIndex)
        {
            yield return new WaitForSeconds(1.4f);

            Load(slotIndex);
        }
    }
}
