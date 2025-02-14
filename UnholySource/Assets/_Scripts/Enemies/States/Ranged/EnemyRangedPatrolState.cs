using Core.StateMachines;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyRangedPatrolState : State
    {
        [Header("State Machine")]
        [SerializeField] private StateMachine stateMachine;

        [Space(10)]

        [SerializeField] private State nextState;

        [Header("Classes")]
        [SerializeField] private EnemyRangedBehaviour enemyBehaviour;

        [Header("Settings")]
        [SerializeField] private int selectedWaypoint;
        [SerializeField] private float distanceFromWaypoint;

        [Space(10)]

        [SerializeField] private Transform[] waypointsTransform;

        [Space(10)]

        [SerializeField] [Range(1f, 10f)] private float searchingAtWaypointTimer = 4f;

        [Space(10)]

        [SerializeField] private Transform frontSearchingTransform;
        [SerializeField] private Transform backSearchingTransform;

        [Space(5)]

        [SerializeField] [Range(1f, 4f)] private float frontSearchingLegth = 2f;
        [SerializeField] [Range(0.2f, 2f)] private float backSearchingLegth = 1f;
        [SerializeField] private LayerMask searchingLayerMask;

        [Space(5)]

        [SerializeField] private float searchingRaycastTick = 0.2f;

        private RaycastHit _frontRayCast;
        private RaycastHit _backRayCast;

        //Precisa ser resetado
        private bool _randomizeWaypoint;
        private bool _goingToWaypoint;
        private bool _searchingAtWaypoint;

        private float _currentSearchingAtWaypointTimer;
        
        private float _currentSearchingRaycastTick;
    
        #region Editor Variable
        #if UNITY_EDITOR
        [Space(20)]

        [SerializeField] private bool showGizmos;

        [Space(10)]

        [SerializeField] private Color waypointColor = Color.green;
        [SerializeField] private Color waypointLineColor = Color.red;

        [Space(10)]

        [SerializeField] private Color searchingRayColor = Color.yellow;
        #endif
        #endregion

        public override void StateAction()
        {
            if (!_randomizeWaypoint) 
            { 
                selectedWaypoint = Random.Range(0, 2);
                
                if(selectedWaypoint > 1) { selectedWaypoint = 1; }

                _randomizeWaypoint = true; 
            }

            TravelingBetweenWaypoints();

            _currentSearchingRaycastTick += Time.deltaTime;
            if (_currentSearchingRaycastTick >= searchingRaycastTick)
            {
                DrawSearchingRaycast();
                _currentSearchingRaycastTick = 0f;
            }

            SearchingAtWaypoint();
        }

        public override void ResetState()
        {
            enemyBehaviour.Animation.IsWalking = false;
            enemyBehaviour.Movement.IsMoving = false;

            _randomizeWaypoint = false;
            _goingToWaypoint = false;
            _searchingAtWaypoint = false;

            _currentSearchingAtWaypointTimer = 0f;
            _currentSearchingRaycastTick = 0f;
        }

        private void GoToNextState()
        {
            enemyBehaviour.Animation.IsWalking = false;
            enemyBehaviour.Movement.IsMoving = false;

            _randomizeWaypoint = false;
            _goingToWaypoint = false;
            _searchingAtWaypoint = false;

            _currentSearchingAtWaypointTimer = 0f;
            _currentSearchingRaycastTick = 0f;

            stateMachine.ChangeState(nextState);
        }

        private void DrawSearchingRaycast()
        {
            if (enemyBehaviour.Movement.MoveRight)
            {
                if (Physics.Raycast(frontSearchingTransform.position, Vector3.forward, out _frontRayCast, frontSearchingLegth, searchingLayerMask))
                {
                    if (_frontRayCast.collider.CompareTag("Player"))
                    {
                        GoToNextState();
                        Debug.Log("Achou Front");
                    }
                }

                if (Physics.Raycast(backSearchingTransform.position, Vector3.back, out _backRayCast, backSearchingLegth, searchingLayerMask))
                {
                    if (_backRayCast.collider.CompareTag("Player"))
                    {
                        GoToNextState();
                        Debug.Log("Achou Back");
                    }
                }
            }
            else
            {
                if (Physics.Raycast(backSearchingTransform.position, Vector3.back, out _backRayCast, frontSearchingLegth, searchingLayerMask))
                {
                    if (_backRayCast.collider.CompareTag("Player"))
                    {
                        GoToNextState();
                        Debug.Log("Achou Front");
                    }
                }

                if (Physics.Raycast(frontSearchingTransform.position, Vector3.forward, out _frontRayCast, backSearchingLegth, searchingLayerMask))
                {
                    if (_frontRayCast.collider.CompareTag("Player"))
                    {
                        GoToNextState();
                        Debug.Log("Achou Back");
                    }
                }
            }
        }

        private void TravelingBetweenWaypoints()
        {
            if (!_goingToWaypoint && !_searchingAtWaypoint)
            {
                distanceFromWaypoint = 0f;

                enemyBehaviour.Movement.MoveRight = enemyBehaviour.GetSideFrom(waypointsTransform[selectedWaypoint]);
                enemyBehaviour.Animation.IsWalking = true;
                enemyBehaviour.Movement.IsMoving = true;

                _goingToWaypoint = true;
            }

            if (_goingToWaypoint)
            {
                distanceFromWaypoint = enemyBehaviour.GetDistanceFrom(waypointsTransform[selectedWaypoint]);

                if (distanceFromWaypoint <= 0.2f)
                {
                    distanceFromWaypoint = 0f;

                    enemyBehaviour.Animation.IsWalking = false;
                    enemyBehaviour.Movement.IsMoving = false;

                    _goingToWaypoint = false;

                    _searchingAtWaypoint = true;
                }
            }
        }

        private void SearchingAtWaypoint()
        {
            if (_searchingAtWaypoint)
            {
                _currentSearchingAtWaypointTimer += Time.deltaTime;
                if (_currentSearchingAtWaypointTimer >= searchingAtWaypointTimer)
                {
                    _currentSearchingAtWaypointTimer = 0f;

                    NextWaypoint();
                }
            }
        }

        private void NextWaypoint()
        {
            selectedWaypoint++;
            if (selectedWaypoint >= waypointsTransform.Length)
            {
                selectedWaypoint = 0;
            }

            _searchingAtWaypoint = false;
        }

        #region Editor Function
        #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if(showGizmos)
            {
                Gizmos.color = waypointColor;
                Gizmos.DrawSphere(waypointsTransform[0].position, 0.2f);
                Gizmos.DrawSphere(waypointsTransform[1].position, 0.2f);

                Gizmos.color = waypointLineColor;
                Gizmos.DrawLine(waypointsTransform[0].position, waypointsTransform[1].position);

                Gizmos.color = searchingRayColor;
                Gizmos.DrawRay(frontSearchingTransform.position, frontSearchingLegth * Vector3.forward);
                Gizmos.DrawRay(backSearchingTransform.position, backSearchingLegth * Vector3.back);
            }
        }
        #endif
        #endregion
    }
}
