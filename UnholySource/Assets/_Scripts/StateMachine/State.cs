using UnityEngine;

namespace Core.StateMachines
{
    public abstract class State : MonoBehaviour
    {
        public abstract void StateAction ();

        public virtual void ResetState ()
        {
            
        }
    }
}
