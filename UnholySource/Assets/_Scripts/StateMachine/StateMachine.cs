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

        #region Encapsulation
        public State LastState { get => _lastState; }

        public bool ChasingState { get => _chasingState; set => _chasingState = value; }
        #endregion

        [Header("States")]
        [SerializeField] private State defaultState;

        [Space(10)]

        [SerializeField] internal State[] lastStateIgnore;

        private State _currentState;
        private State _lastState;

        private bool _chasingState;
        private bool _skipLastState;

        public virtual void OnEnable() 
        {
            if(defaultState == null) enabled = false;

            ResetStateMachine();
        }

        public virtual void Update() 
        {
            if(_currentState == null) ResetStateMachine();

            _currentState.StateAction();
        }

        public void ChangeState(ref State nextState)
        {
            if(nextState == null) return;

            _skipLastState = false;

            if(_currentState != null)
            {
                if(lastStateIgnore.Length > 0)
                {
                    for(int i = 0; i < lastStateIgnore.Length; i++)
                    {
                        if(_currentState == lastStateIgnore[i])
                        {
                            _skipLastState = true;

                            Debug.Log("Last state skipped");

                            break;
                        }
                    }
                }

                if(!_skipLastState) { _lastState = _currentState; }
            }

            _currentState = nextState;
        }

        public void ChangeState(State nextState)
        {
            if(nextState == null) return;

            _skipLastState = false;

            if(_currentState != null)
            {
                if(lastStateIgnore.Length > 0)
                {
                    for(int i = 0; i < lastStateIgnore.Length; i++)
                    {
                        if(_currentState == lastStateIgnore[i])
                        {
                            _skipLastState = true;

                            break;
                        }
                    }
                }

                if(!_skipLastState) { _lastState = _currentState; }
            }

            _currentState = nextState;
        }

        public void ResetStateMachine()
        {
            _currentState = defaultState;
            _currentState.ResetState();
        }

        public void ChasingStateOver() => _chasingState = false;
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

            SerializedObject obj = new SerializedObject(target);
            SerializedProperty stateIgnorePro = obj.FindProperty("lastStateIgnore");

            EditorGUILayout.PropertyField(stateIgnorePro, true);
            obj.ApplyModifiedProperties();

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

            if(stateMachine.LastState != null)
            {
                EditorGUILayout.LabelField($"Last State: {stateMachine.LastState.gameObject.name}");
            }
        }
    }
    #endif
    #endregion
}
