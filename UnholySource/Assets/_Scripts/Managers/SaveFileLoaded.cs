using UnityEngine;

namespace Core.Managers
{
    public sealed class SaveFileLoaded : MonoBehaviour
    {
        #region Singleton
        public static SaveFileLoaded Instance { get => instance; }
        #endregion

        #region Encapsulation
        public SaveFile File { get => _saveFileHandle; }
        #endregion

        private static SaveFileLoaded instance = null;

        private SaveFile _saveFileHandle;

        private void Awake() 
        {
            if(instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(gameObject);
        }

        public void SetupFile(SaveFile file)
        {
            _saveFileHandle = file;
        }
    }
}
