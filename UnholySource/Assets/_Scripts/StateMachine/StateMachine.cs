using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.StateMachines
{
    public class StateMachine : MonoBehaviour
    {
        #region Editor Variables
        #if UNITY_EDITOR
        internal State DefaultState { get => defaultState; set => defaultState = value; }
        internal State CurrentState { get => _currentState; }
        #endif
        #endregion

        [Header("States")]
        [SerializeField] private State defaultState;

        private State _currentState;

        public virtual void OnEnable() 
        {
            if(defaultState == null) enabled = false;

            _currentState = defaultState;
        }

        public virtual void Update() 
        {
            if(_currentState == null) ResetStateMachine();

            _currentState.StateAction();
        }

        public void ChangeState(ref State nextState)
        {
            if(nextState == null) return;

            _currentState = nextState;
        }

        public void ResetStateMachine()
        {
            _currentState = defaultState;
        }
    }

    #region Custom Editor
    #if UNITY_EDITOR
    [CustomEditor(typeof(StateMachine))]
    class StateMachineEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var stateMachine = (StateMachine) target;
            if(stateMachine == null) return;
            
            EditorGUILayout.LabelField("States settings", EditorStyles.boldLabel);

            stateMachine.DefaultState = EditorGUILayout.ObjectField("Default State", stateMachine.DefaultState, typeof(State), true) as State;

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("State Machine info", EditorStyles.boldLabel);

            if(stateMachine.DefaultState != null)
            {
                EditorGUILayout.LabelField($"Starting State: {stateMachine.DefaultState.gameObject.name}");
            }
            else{ EditorGUILayout.LabelField($"No Default State setted, the State Machine will not work."); }

            if(stateMachine.CurrentState != null)
            {
                EditorGUILayout.LabelField($"Current State: {stateMachine.CurrentState.gameObject.name}");
            }
        }
    }
    #endif
    #endregion
}
