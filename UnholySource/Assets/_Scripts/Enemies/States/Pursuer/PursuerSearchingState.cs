using Core.StateMachines;
using UnityEngine;

namespace Core.Enemies
{
    public sealed class PursuerSearchingState : State
    {
        [Header("State Machine")]
        [SerializeField] private StateMachine stateMachine;

        [Header("Behaviour")]
        [SerializeField] private PursuerBehaviour behaviour;

        [Header("Settings")]
        [SerializeField] private State nextState;

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
        private bool _speedSetted;

        private void OnDisable() 
        {
            _speedSetted = false;
            
            searchingPlayer = null;
            _currentUpdateTick = 0f;
        }

        public override void StateAction()
        {
            if(!_speedSetted) 
            { 
                behaviour.Movement.SetCurrentSpeed(Character.CharacterSpeed.WALK); 
                behaviour.Movement.IsMoving = true;

                behaviour.Animation.IsSearching = true;
                behaviour.Animation.IsMoving = true;

                _speedSetted = true; 
            }

            _currentUpdateTick += Time.deltaTime;
            if(_currentUpdateTick >= updateTick)
            {
                SearchPlayer();

                behaviour.Movement.MoveRight = behaviour.GetPlayerSide();

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
                    behaviour.Animation.IsSearching = false;
                    behaviour.Movement.SetCurrentSpeed(Character.CharacterSpeed.DEFAULT);

                    stateMachine.ChangeState(ref nextState);

                    Debug.Log("Player Founded");

                    _speedSetted = false;

                    searchingPlayer = null;
                    _currentUpdateTick = 0f;
                    
                    break;
                }
            }
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
