using Core.StateMachines;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyRangedCombatState : State
    {
        [Header("State Machine")]
        [SerializeField] private StateMachine stateMachine;

        [Space(10)]

        [SerializeField] private State dodgeState;
        [SerializeField] private State shootState;

        [Header("Classes")]
        [SerializeField] private EnemyRangedBehaviour enemyBehaviour;

        [Header("Settings")]
        [SerializeField] private float goToDodgeStateRange = 2f;

        private float _playerDistance;

        #region Editor Variable
        #if UNITY_EDITOR
        [SerializeField] private bool showGizmos;

        [Space(10)]

        [SerializeField] private Color dodgeStateColor = Color.yellow;
        #endif
        #endregion

        public override void StateAction()
        {
            _playerDistance = enemyBehaviour.GetPlayerDistance();

            if(_playerDistance < goToDodgeStateRange)
            {
                _playerDistance = 0f;

                stateMachine.ChangeState(dodgeState);

                Debug.Log("Dodging");
            }
            else if(_playerDistance > goToDodgeStateRange)
            {
                _playerDistance = 0f;

                stateMachine.ChangeState(shootState);

                Debug.Log("Shooting");
            }
        }

        #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if(showGizmos)
            {
                Gizmos.color = dodgeStateColor;
                Gizmos.DrawWireSphere(enemyBehaviour.transform.position, goToDodgeStateRange);
            }
        }
        #endif
    }
}
