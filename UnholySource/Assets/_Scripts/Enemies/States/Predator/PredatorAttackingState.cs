using Core.StateMachines;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class PredatorAttackingState : State
    {
        [Header("State Machine")]
        [SerializeField] private StateMachine stateMachine;

        [Space(10)]

        [SerializeField] private State nextState;

        [Header("Settings")]
        [SerializeField] [Range(1f, 4f)] private float recoveryTime;

        private float _currentRecoveryTime;

        private void OnDisable() 
        {
            _currentRecoveryTime = 0f;
        }

        public override void StateAction()
        {
            _currentRecoveryTime += Time.deltaTime;
            if(_currentRecoveryTime >= recoveryTime)
            {
                _currentRecoveryTime = 0f;
                stateMachine.ChangeState(ref nextState);
            }
        }
    }
}
