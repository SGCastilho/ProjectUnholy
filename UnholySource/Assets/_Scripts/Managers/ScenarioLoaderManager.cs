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
        [SerializeField] private PredatorManager predatorManager;
        
        [Header("Scenes To Load")]
        [SerializeField] private SceneLoaderSettings[] scenes;
        
        [Header("Settings")]
        [SerializeField] private bool loadFirstScene;

        #region Editor Variables
        #if UNITY_EDITOR

        [Header("Debug Tools")]
        [SerializeField] private bool loadDebugScene;
        [SerializeField] private string sceneToDebug = "Put the scene here";
        [SerializeField] private Transform travelPosistion;
        #endif
        #endregion

        private bool _loadingScene;
        private bool _isTraveling;
        private string _currentLoadedScene;

        private AsyncOperation _currentLoadingOperation;

        private Transform _playerTransform;
        private Transform _travelPointTransform;

        private void Awake()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        private void OnEnable() 
        {
            if(scenes.Length <= 0) return;

            for(int i = 0; i < scenes.Length; i++)
            {
                if(scenes[i].EnemiesToEnable != null) { scenes[i].EnemiesToEnable.SetActive(false); }

                if(scenes[i].InteractablesToEnable != null) { scenes[i].InteractablesToEnable.SetActive(false); }
            }

            #if UNITY_EDITOR
            if(loadDebugScene)
            {
                loadFirstScene = false;

                _playerTransform.gameObject.SetActive(false);
                _playerTransform.position = travelPosistion.position;
                _playerTransform.gameObject.SetActive(true);

                LoadSceneAddictive(sceneToDebug);
            }
            #endif

            if(loadFirstScene)
            {
                LoadSceneAddictive(scenes[0].SceneName);
            }
        }

        public void LoadSceneAddictive(string sceneName)
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

            _currentLoadedScene = sceneToLoad.SceneName;

            _loadingScene = true;

            if(_isTraveling)
            {
                _playerTransform.position = _travelPointTransform.position;

                _isTraveling = false;
            }

            StartCoroutine(LoadingProgress());
        }

        public void UnLoadSceneAddictive(string sceneName)
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

            UnLoadSceneAddictive(_currentLoadedScene);

            await Task.Delay(400);

            LoadSceneAddictive(sceneName);

            await Task.Delay(1000);

            OnEndTravel?.Invoke();

            await Task.Delay(1000);

            if(predatorManager.IsChasing)
            {
                if(predatorManager.CountdownFinish)
                {
                    predatorManager.EndChasing();
                }
                else
                {
                    predatorManager.SpawnPredator(ref _travelPointTransform);
                }
            }
        }

        public Transform ReturnFarPredatorPoint()
        {
            SceneLoaderSettings currentScene = new SceneLoaderSettings();

            foreach(SceneLoaderSettings scene in scenes)
            {
                if(scene.SceneName == _currentLoadedScene)
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

                Debug.Log($"Left: {leftSpawnDistance}");
            }

            if(currentScene.RightSpawn != null)
            {
                rightSpawnDistance = Mathf.Abs(Vector3.Distance(_playerTransform.position, currentScene.RightSpawn.position));

                Debug.Log($"Right: {rightSpawnDistance}");
            }

            if(rightSpawnDistance > leftSpawnDistance)
            {
                Debug.Log("Return right spawn");

                return currentScene.RightSpawn;
            }
            else if(rightSpawnDistance < leftSpawnDistance)
            {
                Debug.Log("Return left spawn");

                return currentScene.LeftSpawn;
            }

            return null;
        }
    }
}
