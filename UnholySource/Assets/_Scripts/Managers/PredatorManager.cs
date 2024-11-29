using UnityEngine;
using UnityEngine.Events;

namespace Core.Managers
{
    public sealed class PredatorManager : MonoBehaviour
    {
        #region Encapsulation
        internal bool IsChasing { get => isChasing; }
        internal bool CountdownFinish { get => countDownFinish; }
        #endregion

        [Header("Classes")]
        [SerializeField] private ScenarioLoaderManager scenarioLoaderManager;

        [Header("Settings")]
        [SerializeField] private GameObject predatorInstance;
        [SerializeField] private AudioClip predatorAudioClip;

        [Space(10)]

        [SerializeField] private bool predatorEnabled;

        [Space(6)]

        [SerializeField] private bool isChasing;
        [SerializeField] private bool chasingAgain;
        [SerializeField] private bool countDownFinish;
        [SerializeField] private bool checkingChasingRoll;

        [Space(10)]

        [SerializeField] [Range(10, 100)] private int chasingContinueChange = 20;

        [Space(10)]

        [SerializeField] [Range(10f, 20f)] private float minChasingCountdown = 12f;
        [SerializeField] [Range(10f, 20f)] private float maxChasingCountdown = 18f;

        [Space(6)]

        [SerializeField] [Range(10f, 20f)] private float chasingTime = 14f;

        [Header("Unity Events")]

        [Space(10)]

        [SerializeField] private UnityEvent OnStartChasing;

        [Space(10)]

        [SerializeField] private UnityEvent OnExitChasing;
        
        [Space(10)]

        [SerializeField] private UnityEvent OnFinishChasing;

        private int _continueChasingRoll;

        private float _chasingCountdown;
        private float _continueRollCountdown;

        private float _currentChasingTime;
        private float _currentChasingCountdown;
        private float _currentContinueRollCountdown;

        private Transform _predatorSpawnPosistion;

        private void Awake() 
        {
            _continueRollCountdown = predatorAudioClip.length;
        }

        private void Start() 
        {
            if(predatorInstance == null) { enabled = false; }

            predatorInstance.SetActive(false);

            RandomizeChasing();
        }

        private void Update() 
        {
            if(!predatorEnabled) return;

            if(!isChasing && !checkingChasingRoll)
            {
                _currentChasingCountdown += Time.deltaTime;
                if(_currentChasingCountdown >= _chasingCountdown)
                {
                    StartChasing();

                    _currentChasingCountdown = 0f;
                }
            }

            if(isChasing && !countDownFinish)
            {
                _currentChasingTime += Time.deltaTime;
                if(_currentChasingTime >= chasingTime)
                {
                    countDownFinish = true;
                    _currentChasingTime = 0f;
                }
            }

            if(checkingChasingRoll)
            {
                _currentContinueRollCountdown += Time.unscaledDeltaTime;
                if(_currentContinueRollCountdown >= _continueRollCountdown)
                {
                    _currentContinueRollCountdown = 0f;

                    OnFinishChasing?.Invoke();
                    
                    if(_continueChasingRoll <= chasingContinueChange)
                    {
                        StartChasing();

                        chasingAgain = true;
                    }

                    checkingChasingRoll = false;
                }
            }
        }

        public void StartChasing()
        {
            if(!predatorEnabled) return;

            SpawnPredator();

            OnStartChasing?.Invoke();
        }

        public void SpawnPredator()
        {
            isChasing = true;

            predatorInstance.SetActive(false);

            _predatorSpawnPosistion = scenarioLoaderManager.ReturnFarPredatorPoint();

            if(_predatorSpawnPosistion == null) return;

            predatorInstance.transform.position = _predatorSpawnPosistion.position;

            predatorInstance.SetActive(true);
        }

        public void SpawnPredator(ref Transform predatorSpawn)
        {
            isChasing = true;

            predatorInstance.SetActive(false);

            if(predatorSpawn == null) return;

            predatorInstance.transform.position = predatorSpawn.position;

            predatorInstance.SetActive(true);
        }

        public void EndChasing()
        {
            if(!predatorEnabled) return;

            RandomizeChasing();

            if(chasingAgain)
            {
                predatorInstance.SetActive(false);

                isChasing = false;
                chasingAgain = false;
            }
            else
            {
                OnExitChasing?.Invoke();

                predatorInstance.SetActive(false);

                _continueChasingRoll = Random.Range(0, 100);

                Debug.Log($"Chasing roll: {_continueChasingRoll}");

                checkingChasingRoll = true;

                isChasing = false;
            }
        }

        public void EnablePredator()
        {
            _currentChasingTime = 0f;
            _currentChasingCountdown = 0f;
            _currentContinueRollCountdown = 0f;

            RandomizeChasing();

            predatorEnabled = true;
        }
        
        public void DisablePredator()
        {
            _currentChasingTime = 0f;
            _currentChasingCountdown = 0f;
            _currentContinueRollCountdown = 0f;

            predatorEnabled = false;
        }

        private void RandomizeChasing()
        {
            _chasingCountdown = Random.Range(minChasingCountdown, maxChasingCountdown);
            _currentChasingCountdown = 0f;
        }
    }
}
