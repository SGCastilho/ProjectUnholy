using Core.StateMachines;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyRangedDeathState : State
    {
        [Header("State Machine")]
        [SerializeField] private StateMachine stateMachine;

        [Header("Classes")]
        [SerializeField] private EnemyRangedBehaviour enemyBehaviour;

        private bool _deathSequence;
        private bool _isDead;

        public override void StateAction()
        {
            if(!_deathSequence)
            {
                enemyBehaviour.Movement.SetCurrentSpeed(Character.CharacterSpeed.DEFAULT);
                enemyBehaviour.Movement.IsMoving = false;
                enemyBehaviour.Animation.IsDead = true;

                _deathSequence = true;
            }

            if(_isDead)
            {
                stateMachine.enabled = false;
            }
        }

        public void IsDead() => _isDead = true;
    }
}
