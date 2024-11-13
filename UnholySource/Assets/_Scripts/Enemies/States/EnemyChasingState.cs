using Core.StateMachines;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyChasingState : State
    {
        [Header("State Machine")]
        [SerializeField] private StateMachine stateMachine;

        [Space(10)]

        [SerializeField] private State nextState;

        [Header("Classes")]
        [SerializeField] private EnemyMeleeBehaviour behaviour;

        [Header("Settings")]
        [SerializeField] [Range(2f, 8f)] private float attackRange = 4f;

        #region Editor Variable
        #if UNITY_EDITOR
        [Space(10)]

        [SerializeField] private bool showGizmos;
        [SerializeField] private Color gizmosColor = Color.yellow;
        #endif
        #endregion

        [SerializeField] [Range(0.1f, 0.6f)] private float updateTick = 0.2f;

        private float _currentUpdateTick;

        private void OnDisable() => ResetState();

        private void ResetState()
        {
            _currentUpdateTick = 0f;

            //DEBUG
            behaviour.Animation.ChasingAnimation = false;
        }

        public override void StateAction()
        {
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

                ResetState();

                stateMachine.ChangeState(ref nextState);
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
