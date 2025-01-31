using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Managers
{
    [System.Serializable]
    public struct SceneLoaderSettings
    {
        #region Encapsulation
        public string SceneName { get => sceneName; }
        public GameObject EnemiesToEnable { get => enemiesToEnable; }
        public GameObject InteractablesToEnable { get => interactableToEnable; }

        public Transform LeftSpawn { get => predatorLeftSpawn; }
        public Transform RightSpawn { get => predatorRightSpawn; }
        #endregion

        [Header("Settings")]
        [SerializeField] private string sceneName;
        [SerializeField] private GameObject enemiesToEnable;
        [SerializeField] private GameObject interactableToEnable;

        [Space(10)]

        [SerializeField] private Transform predatorLeftSpawn;
        [SerializeField] private Transform predatorRightSpawn;
    }

    public sealed class ScenarioLoaderManager : MonoBehaviour
    {
        #region Events
        public delegate void StartTravel();
        public event StartTravel OnStartTravel;

        public delegate void EndTravel();
        public event EndTravel OnEndTravel;
        #endregion
        
        [Header("Classes")]
        [SerializeField] private ChaserManager chaserManager;

        [Header("Scenes To Load")]
        [SerializeField] private SceneLoaderSettings[] scenes;
        
        [Header("Settings")]
        [SerializeField] private bool loadFirstScene;

        [SerializeField] private bool loadSaveFileRoom;
        [SerializeField] private string sceneToLoad = "Put the scene here";
        [SerializeField] private Transform travelPosistion;
        
        [Header("Debug Tools")]
        [SerializeField] private bool loadDebugScene;

        private bool _loadingScene;
        private bool _isTraveling;
        private string _currentRoomScene;
        private string _currentLevelLoaded;
        private Vector3 _savedPlayerPosistion;

        private AsyncOperation _currentLoadingOperation;

        private Transform _playerTransform;
        private Transform _travelPointTransform;

        private void Awake()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

            _currentLevelLoaded = SceneManager.GetActiveScene().name;
        }

        private void OnEnable() 
        {
            if(scenes.Length <= 0) return;

            for(int i = 0; i < scenes.Length; i++)
            {
                if(scenes[i].EnemiesToEnable != null) { scenes[i].EnemiesToEnable.SetActive(false); }

                if(scenes[i].InteractablesToEnable != null) { scenes[i].InteractablesToEnable.SetActive(false); }
            }

            if(loadSaveFileRoom)
            {
                loadFirstScene = false;

                _playerTransform.gameObject.SetActive(false);
                
                if(!loadDebugScene)
                {
                    _playerTransform.position = _savedPlayerPosistion;
                }
                else
                {
                    _playerTransform.position = travelPosistion.position;
                }

                _playerTransform.gameObject.SetActive(true);

                LoadRoomAddictive(sceneToLoad);
            }

            if(loadFirstScene)
            {
                LoadRoomAddictive(scenes[0].SceneName);
            }
        }

        public void LoadRoomAddictive(string sceneName)
        {
            if(_loadingScene || scenes.Length <= 0) return;

            SceneLoaderSettings sceneToLoad = new SceneLoaderSettings();

            foreach(SceneLoaderSettings scene in scenes)
            {
                if(scene.SceneName == sceneName)
                {
                    sceneToLoad = scene;
                    break;
                }
            }

            _currentLoadingOperation = SceneManager.LoadSceneAsync(sceneToLoad.SceneName, LoadSceneMode.Additive);

            if(sceneToLoad.EnemiesToEnable != null) { sceneToLoad.EnemiesToEnable.SetActive(true); }

            if(sceneToLoad.InteractablesToEnable != null) { sceneToLoad.InteractablesToEnable.SetActive(true); }

            _currentRoomScene = sceneToLoad.SceneName;

            _loadingScene = true;

            if(_isTraveling)
            {
                _playerTransform.position = _travelPointTransform.position;

                _isTraveling = false;
            }

            StartCoroutine(LoadingProgress());
        }

        public void UnLoadRoomAddictive(string sceneName)
        {
            if(_loadingScene || scenes.Length <= 0) return;

            SceneLoaderSettings sceneToLoad = new SceneLoaderSettings();

            foreach(SceneLoaderSettings scene in scenes)
            {
                if(scene.SceneName == sceneName)
                {
                    sceneToLoad = scene;
                    break;
                }
            }

            _currentLoadingOperation = SceneManager.UnloadSceneAsync(sceneToLoad.SceneName);

            if(sceneToLoad.EnemiesToEnable != null) { sceneToLoad.EnemiesToEnable.SetActive(false); }

            if(sceneToLoad.InteractablesToEnable != null) { sceneToLoad.InteractablesToEnable.SetActive(false); }

            _loadingScene = true;

            StartCoroutine(LoadingProgress());
        }

        private IEnumerator LoadingProgress()
        {
            while(!_currentLoadingOperation.isDone)
            {
                yield return null;
            }

            _loadingScene = false;
        }

        public void TravelTo(Transform travelPoint)
        {
            _travelPointTransform = travelPoint;
            _isTraveling = true;
        }
        
        public async void TravelToScene(string sceneName)
        {
            OnStartTravel?.Invoke();

            UnLoadRoomAddictive(_currentRoomScene);

            await Task.Delay(400);

            LoadRoomAddictive(sceneName);

            await Task.Delay(1000);

            OnEndTravel?.Invoke();

            if(chaserManager.IsEnabled && !chaserManager.IsChasing)
            {
                chaserManager.ChaserGetCloser();
            }

            await Task.Delay(1000);

            if(chaserManager.IsChasing)
            {
                chaserManager.ChaserCountToHideAgain();

                if(chaserManager.FirstTimeSpawn)
                {
                    chaserManager.SpawnChaser(ReturnFarPredatorPoint());
                }
                else
                {
                    chaserManager.SpawnChaser(_travelPointTransform);
                }
            }
        }

        public async void ReloadCurrentScene()
        {
            await Task.Delay(4000);

            OnStartTravel?.Invoke();

            await Task.Delay(1000);

            SceneManager.LoadScene(_currentLevelLoaded);

            OnEndTravel?.Invoke();
        }

        public async void LoadScene(string sceneToLoad)
        {
            OnStartTravel?.Invoke();

            await Task.Delay(1000);

            SceneManager.LoadScene(sceneToLoad);

            OnEndTravel?.Invoke();
        }

        public void LoadRoom(string roomName, Vector3 playerPosition)
        {
            sceneToLoad = roomName;
            _savedPlayerPosistion = playerPosition;
            
            loadSaveFileRoom = true;
        }

        public Transform ReturnFarPredatorPoint()
        {
            SceneLoaderSettings currentScene = new SceneLoaderSettings();

            foreach(SceneLoaderSettings scene in scenes)
            {
                if(scene.SceneName == _currentRoomScene)
                {
                    currentScene = scene;
                    break;
                }
            }

            float leftSpawnDistance = 0f;
            float rightSpawnDistance = 0f;

            if(currentScene.LeftSpawn != null)
            {
                leftSpawnDistance = Mathf.Abs(Vector3.Distance(_playerTransform.position, currentScene.LeftSpawn.position));
            }

            if(currentScene.RightSpawn != null)
            {
                rightSpawnDistance = Mathf.Abs(Vector3.Distance(_playerTransform.position, currentScene.RightSpawn.position));
            }

            if(rightSpawnDistance > leftSpawnDistance)
            {
                return currentScene.RightSpawn;
            }
            else if(rightSpawnDistance < leftSpawnDistance)
            {
                return currentScene.LeftSpawn;
            }

            return null;
        }

        public Transform ReturnPlayerLastSpawn()
        {
            return _travelPointTransform;
        }

        public string GetCurrentRoom()
        {
            return _currentRoomScene;
        }

        public string GetCurrentScene()
        {
            return _currentLevelLoaded;
        }
    }
}
