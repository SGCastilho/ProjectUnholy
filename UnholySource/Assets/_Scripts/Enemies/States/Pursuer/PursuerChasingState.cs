using Core.StateMachines;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class PursuerChasingState : State
    {
        [Header("State Machine")]
        [SerializeField] private StateMachine stateMachine;

        [Header("Behaviour")]
        [SerializeField] private PursuerBehaviour behaviour;

        [Header("Settings")]
        [SerializeField] private State nextState;

        [Header("Settings")]
        [SerializeField] private bool _isRunning;
        [SerializeField] private bool _timeSetted;
        [SerializeField] [Range(1f, 6f)] private float minTimeToStartRunning = 2f;
        [SerializeField] [Range(6f, 10f)] private float maxTimeToStartRunning = 8f;

        [Space(10)]

        [SerializeField] [Range(0.1f, 8f)] private float attackRange = 4f;

        #region Editor Variable
        #if UNITY_EDITOR
        [Space(10)]

        [SerializeField] private bool showGizmos;
        [SerializeField] private Color gizmosColor = Color.yellow;
        #endif
        #endregion

        [SerializeField] [Range(0.1f, 0.6f)] private float updateTick = 0.2f;

        private float _currentUpdateTick;

        private float _timeToStartRunning;
        private float _currentTimeToStartRunning;

        private void OnDisable() => ResetState();

        public override void ResetState()
        {
            _isRunning = false;
            _timeSetted = false;
            _currentUpdateTick = 0f;
        }

        public override void StateAction()
        {
            if(!_timeSetted)
            {
                _timeToStartRunning = Random.Range(minTimeToStartRunning, maxTimeToStartRunning);

                _timeSetted = true;
            }

            if(!_isRunning)
            {
                _currentTimeToStartRunning += Time.deltaTime;
                if(_currentTimeToStartRunning >= _timeToStartRunning)
                {
                    behaviour.Movement.SetCurrentSpeed(Character.CharacterSpeed.RUN);
                    behaviour.Animation.IsRunning = true;

                    _isRunning = true;
                    _currentTimeToStartRunning = 0f;
                }
            }

            _currentUpdateTick += Time.deltaTime;
            if(_currentUpdateTick >= updateTick)
            {
                CheckingRangeToAttack();
                CheckingPlayerSide();

                _currentUpdateTick = 0f;
            }
        }

        private void CheckingPlayerSide()
        {
            behaviour.Movement.MoveRight = behaviour.GetPlayerSide();
        }

        private void CheckingRangeToAttack()
        {
            if(behaviour.GetPlayerDistance() <= attackRange)
            {
                behaviour.Movement.IsMoving = false;

                _currentUpdateTick = 0f;

                if(_isRunning)
                {
                    behaviour.Animation.IsRunningAttacking = true;

                    _isRunning = false;
                    _timeSetted = false;

                    behaviour.Movement.SetCurrentSpeed(Character.CharacterSpeed.DEFAULT);
                }

                stateMachine.ChangeState(ref nextState);

                behaviour.Animation.IsMoving = false;

                _currentTimeToStartRunning = 0f;
            }
            else
            {
                if(!behaviour.Movement.IsMoving)
                {
                    behaviour.Movement.MoveRight = behaviour.GetPlayerSide();
                    behaviour.Movement.IsMoving = true;
                }
            }
        }

        #region Editor Function
        #if UNITY_EDITOR
        private void OnDrawGizmos() 
        {
            if(showGizmos)
            {
                Gizmos.color = gizmosColor;
                Gizmos.DrawWireSphere(transform.position, attackRange);
            }
        }
        #endif
        #endregion
    }
}
