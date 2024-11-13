using Core.StateMachines;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemySearchingState : State
    {
        [Header("State Machine")]
        [SerializeField] private StateMachine stateMachine;

        [Space(10)]

        [SerializeField] private State nextState;

        [Header("Classes")]
        [SerializeField] private EnemyMeleeBehaviour behaviour;

        [Space(10)]

        [SerializeField] private Transform searchingTransform;

        [Header("Settings")]
        [SerializeField] private Vector3 searchingBoxSize;
        [SerializeField] private LayerMask searchingLayer;

        #region Editor Variable
        #if UNITY_EDITOR
        [Space(10)]

        [SerializeField] private bool showGizmos;
        [SerializeField] private Color searchingBoxColor = Color.red;
        #endif
        #endregion

        [Space(10)]

        [SerializeField] [Range(0.1f, 0.6f)] private float updateTick = 0.2f;

        private Collider[] searchingPlayer;

        private float _currentUpdateTick;

        private void OnDisable() => ResetState();

        private void ResetState()
        {
            searchingPlayer = null;
            _currentUpdateTick = 0f;
        }

        public override void StateAction()
        {
            _currentUpdateTick += Time.deltaTime;
            if(_currentUpdateTick >= updateTick)
            {
                SearchPlayer();

                _currentUpdateTick = 0f;
            }
        }

        private void SearchPlayer()
        {
            searchingPlayer = Physics.OverlapBox(searchingTransform.position, searchingBoxSize, searchingTransform.rotation, searchingLayer);

            if(searchingPlayer == null || searchingPlayer.Length <= 0) return;
            
            foreach(Collider player in searchingPlayer)
            {
                if(player.CompareTag("Player"))
                {
                    Debug.Log("Player Founded");

                    stateMachine.ChangeState(ref nextState);

                    StartChasingState();

                    ResetState();
                    
                    break;
                }
            }
        }

        private void StartChasingState()
        {
            behaviour.Movement.MoveRight = behaviour.GetPlayerSide();
            behaviour.Movement.IsMoving = true;

            behaviour.Animation.ChasingAnimation = true;
        }

        #region Editor Function
        #if UNITY_EDITOR
        private void OnDrawGizmos() 
        {
            if(showGizmos)
            {
                if(searchingTransform == null) return;

                Gizmos.color = searchingBoxColor;
                Gizmos.DrawWireCube(searchingTransform.position, searchingBoxSize);
            }    
        }
        #endif
        #endregion
    }
}
