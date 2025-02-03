using UnityEngine;
using UnityEngine.Events;

namespace Core.Utilities
{
    public sealed class EnemiesSensor : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool enemyFinded;
        [SerializeField] private float sensorTick = 0.6f;

        [Space(10)]

        [SerializeField] private float detectionDistance = 4f;
        [SerializeField] private LayerMask detectionLayerMask;

        #region Editor Variables
        #if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] private bool showGizmos;
        #endif
        #endregion

        [Header("Events")]
        [Space(10)]

        [SerializeField] private UnityEvent OnEnemyFounded;

        [Space(10)]

        [SerializeField] private UnityEvent OnEnemyLost;

        private Transform _transform;

        private float _currentSensorTick;

        private void Awake() 
        {
            _transform = GetComponent<Transform>();
        }

        private void OnDisable() 
        {
            enemyFinded = false;
            _currentSensorTick = 0f;

            Debug.Log("Disable");
        }

        private void Update() 
        {
            _currentSensorTick += Time.deltaTime;
            if(_currentSensorTick >= sensorTick)
            {
                DrawDetection();

                _currentSensorTick = 0f;
            }
        }

        private void DrawDetection()
        {
            if(Physics.CheckSphere(_transform.position, detectionDistance, detectionLayerMask))
            {
                if(enemyFinded) return;

                OnEnemyFounded?.Invoke();

                enemyFinded = true;
            }
            else
            {
                if(!enemyFinded) return;

                OnEnemyLost?.Invoke();

                enemyFinded = false;
            }
        }

        #region Editor Functions
        #if UNITY_EDITOR
        private void OnDrawGizmosSelected() 
        {
            if(showGizmos)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, detectionDistance);
            }
        }
        #endif
        #endregion
    }
}
