using Core.StateMachines;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class PursuerAttackingState : State
    {
        [Header("State Machine")]
        [SerializeField] private StateMachine stateMachine;

        [Header("Behaviour")]
        [SerializeField] private PursuerBehaviour behaviour;

        [Header("Settings")]
        [SerializeField] private State nextState;

        [Space(10)]
        
        [SerializeField] [Tooltip("Combos starts counting from 0")] private int maxCombo = 1;
        [SerializeField] private int currentCombo;

        [SerializeField] [Range(0.1f, 8f)] private float attackRange = 4f;

        [SerializeField] [Range(0.1f, 0.6f)] private float updateTick = 0.2f;

        private float _currentUpdateTick;

        public override void StateAction()
        {
            _currentUpdateTick += Time.deltaTime;
            if(_currentUpdateTick >= updateTick)
            {
                CheckIfPlayerIsInRange();

                _currentUpdateTick = 0f;
            }
        }

        private void CheckIfPlayerIsInRange()
        {
            if (behaviour.Animation.IsAttacking) return;

            if (behaviour.GetPlayerDistance() > attackRange)
            {
                BackToChasingState();

                return;
            }

            StartAttacking();
        }

        private void StartAttacking()
        {
            behaviour.Movement.MoveRight = behaviour.GetPlayerSide();
            behaviour.Animation.CurrentCombo = currentCombo;
            behaviour.Animation.IsAttacking = true;

            currentCombo++;

            if (currentCombo > maxCombo)
            {
                currentCombo = 0;
            }
        }

        private void BackToChasingState()
        {
            behaviour.Movement.MoveRight = behaviour.GetPlayerSide();
            behaviour.Movement.IsMoving = true;
            behaviour.Animation.IsMoving = true;

            stateMachine.ChangeState(ref nextState);

            currentCombo = 0;
            _currentUpdateTick = 0f;
        }
    }
}
