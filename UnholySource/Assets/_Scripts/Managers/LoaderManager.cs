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
        #endregion

        [Header("Classes")]
        [SerializeField] private GameObject saveFileLoadedPrefab;

        private string _savePath = Application.dataPath + "/Saved/saveFile.save";

        public void Load()
        {
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

        public bool SaveFileExists()
        {
            return File.Exists(_savePath);
        }
    }
}
