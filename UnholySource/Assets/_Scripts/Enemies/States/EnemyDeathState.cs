using Core.StateMachines;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyDeathState : State
    {
        [Header("State Machine")]
        [SerializeField] private StateMachine stateMachine;

        [Header("Behaviour")]
        [SerializeField] private EnemyMeleeBehaviour behaviour;

        public override void StateAction()
        {
            behaviour.Animation.DeathAnimation = true;

            stateMachine.enabled = false;
        }
    }
}
