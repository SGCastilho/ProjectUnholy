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

        public override void StateAction()
        {
            
        }
    }
}
