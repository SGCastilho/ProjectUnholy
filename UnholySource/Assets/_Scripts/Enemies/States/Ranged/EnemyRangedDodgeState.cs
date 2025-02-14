using Core.StateMachines;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyRangedDodgeState : State
    {
        [Header("State Machine")]
        [SerializeField] private StateMachine stateMachine;

        [Space(10)]

        [SerializeField] private State nextState;

        [Header("Classes")]
        [SerializeField] private EnemyRangedBehaviour enemyBehaviour;

        [Header("Settings")]
        [SerializeField] [Range(0.1f, 4f)] private float dodgingTime = 2f;

        [Space(10)]
        
        [SerializeField] private Transform frontWallSearchingTransform;
        [SerializeField] private Transform backWallSearchingTransform;

        [Space(5)]

        [SerializeField] [Range(0.2f, 2f)] private float wallSearchingLegth = 1f;
        [SerializeField] private LayerMask wallSearchingLayerMask;

        [Space(5)]

        [SerializeField] private float searchingRaycastTick = 0.4f;

        private RaycastHit _frontRayCast;
        private RaycastHit _backRayCast;

        private bool _runningFromPlayer; //Precisa ser resetado
        private float _currentDodingTime;

        private float _currentSearchingRaycastTick;

        public override void StateAction()
        {
            RunningFromPlayer();

            _currentSearchingRaycastTick += Time.deltaTime;
            if (_currentSearchingRaycastTick >= searchingRaycastTick)
            {
                DrawSearchingRaycast();
                _currentSearchingRaycastTick = 0f;
            }

            RunningTimer();
        }

        void OnDisable() => ResetState();

        public override void ResetState()
        {
            enemyBehaviour.Animation.IsRunning = false;
            enemyBehaviour.Movement.IsMoving = false;
            enemyBehaviour.Movement.SetCurrentSpeed(Character.CharacterSpeed.DEFAULT);

            _currentDodingTime = 0f;
            _currentSearchingRaycastTick = 0f;
            
            _runningFromPlayer = false;
        }

        private void DrawSearchingRaycast()
        {
            if (enemyBehaviour.Movement.MoveRight)
            {
                if (Physics.Raycast(frontWallSearchingTransform.position, Vector3.forward, out _frontRayCast, wallSearchingLegth, wallSearchingLayerMask))
                {
                    enemyBehaviour.Movement.MoveRight = !enemyBehaviour.Movement.MoveRight;
                    Debug.Log("Front Wall");
                }

                if (Physics.Raycast(backWallSearchingTransform.position, Vector3.back, out _backRayCast, wallSearchingLegth, wallSearchingLayerMask))
                {
                    enemyBehaviour.Movement.MoveRight = !enemyBehaviour.Movement.MoveRight;
                    Debug.Log("Back Wall");
                }
            }
            else
            {
                if (Physics.Raycast(frontWallSearchingTransform.position, Vector3.back, out _backRayCast, wallSearchingLegth, wallSearchingLayerMask))
                {
                    enemyBehaviour.Movement.MoveRight = !enemyBehaviour.Movement.MoveRight;
                    Debug.Log("Front Wall");
                }

                if (Physics.Raycast(backWallSearchingTransform.position, Vector3.forward, out _frontRayCast, wallSearchingLegth, wallSearchingLayerMask))
                {
                    enemyBehaviour.Movement.MoveRight = !enemyBehaviour.Movement.MoveRight;
                    Debug.Log("Back Wall");
                }
            }
        }

        private void RunningTimer()
        {
            if (_runningFromPlayer)
            {
                _currentDodingTime += Time.deltaTime;
                if (_currentDodingTime >= dodgingTime)
                {
                    GoToNextState();
                }
            }
        }

        private void GoToNextState()
        {
            enemyBehaviour.Animation.IsRunning = false;
            enemyBehaviour.Movement.IsMoving = false;
            enemyBehaviour.Movement.SetCurrentSpeed(Character.CharacterSpeed.DEFAULT);

            enemyBehaviour.Movement.FlipToPlayer(enemyBehaviour.GetPlayerSide());

            _currentDodingTime = 0f;
            _currentSearchingRaycastTick = 0f;
            
            _runningFromPlayer = false;

            stateMachine.ChangeState(nextState);

            Debug.Log("Ready to Shoot");
        }

        private void RunningFromPlayer()
        {
            if (!_runningFromPlayer)
            {
                if(Physics.Raycast(backWallSearchingTransform.position, Vector3.back, out _backRayCast, wallSearchingLegth, wallSearchingLayerMask))
                {
                    enemyBehaviour.Movement.MoveRight = enemyBehaviour.GetPlayerSide();
                }
                else
                {
                    enemyBehaviour.Movement.MoveRight = !enemyBehaviour.GetPlayerSide();
                }

                enemyBehaviour.Movement.SetCurrentSpeed(Character.CharacterSpeed.RUN);
                enemyBehaviour.Animation.IsRunning = true;
                enemyBehaviour.Movement.IsMoving = true;

                _runningFromPlayer = true;
            }
        }
    }
}
