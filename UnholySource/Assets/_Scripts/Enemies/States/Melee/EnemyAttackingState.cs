using Core.StateMachines;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyAttackingState : State
    {
        [Header("State Machine")]
        [SerializeField] private StateMachine stateMachine;

        [Space(10)]

        [SerializeField] private State nextState;

        [Header("Classes")]
        [SerializeField] private EnemyMeleeBehaviour behaviour;

        [Header("Settings")]
        [SerializeField] [Range(2f, 8f)] private float attackRange = 4f;

        [Space(6)]

        [SerializeField] [Range(0.6f, 6f)] private float updateTick = 1.6f;

        private float _currentUpdateTick;

        private void OnDisable() => ResetState();

        public override void ResetState()
        {
            behaviour.Animation.IsAttacking = false;
            _currentUpdateTick = 0f;
        }
        
        public override void StateAction()
        {
            _currentUpdateTick += Time.deltaTime;
            if(_currentUpdateTick >= updateTick)
            {
                CheckingRangeToAttack();

                _currentUpdateTick = 0f;
            }
        }

        private void CheckingRangeToAttack()
        {
            if(behaviour.GetPlayerDistance() <= attackRange)
            {
                AttackPlayer();
            }
            else
            {
                ChangeState();
            }
        }

        private void AttackPlayer()
        {
            if(behaviour.Animation.IsAttacking) return;

            behaviour.Movement.FlipToPlayer(behaviour.GetPlayerSide());
            behaviour.Correction.Correct(behaviour.GetPlayerSide());

            if(behaviour.Correction.FindCollider)
            {
                behaviour.Animation.CallTriggerAttack();

                behaviour.Animation.IsAttacking = true;
            }
            else
            {
                ChangeState();
            }
        }

        private void ChangeState()
        {
            stateMachine.ChangeState(ref nextState);

            StartChasingState();

            ResetState();
        }

        private void StartChasingState()
        {
            behaviour.Movement.MoveRight = behaviour.GetPlayerSide();
            behaviour.Movement.IsMoving = true;

            behaviour.Animation.ChasingAnimation = true;
        }
    }
}
