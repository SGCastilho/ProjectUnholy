using Core.StateMachines;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyHittedState : State
    {
        [Header("State Machine")]
        [SerializeField] private StateMachine stateMachine;

        [Header("Behaviour")]
        [SerializeField] private EnemyMeleeBehaviour behaviour;

        [Header("Settings")]
        [SerializeField] [Range(1f, 4f)] private float staggerTime = 1f;

        [SerializeField] private bool inStagger;

        private float _currentStaggerTime;

        public override void ResetState()
        {
            _currentStaggerTime = 0f;
        }

        public override void StateAction()
        {
            if(inStagger)
            {
                _currentStaggerTime += Time.deltaTime;
                if(_currentStaggerTime >= staggerTime)
                {
                    _currentStaggerTime = 0;

                    inStagger = false;

                    stateMachine.ChangeState(stateMachine.LastState);
                }
            }
            else
            {
                stateMachine.LastState.ResetState();

                behaviour.Movement.IsMoving = false;
                behaviour.Animation.ChasingAnimation = false;

                behaviour.Animation.CallTriggerHitted();

                inStagger = true;
            }
        }
    }
}
