using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerAnimation : MonoBehaviour
    {
        #region Constant Variables
        private const string MOVEMENT_KEY = "Movement";
        #endregion

        #region Encapsulation
        internal bool SprintAnimation 
        {
            set 
            {
                if(value == true)
                {
                    animator.speed = 1.4f;
                }
                else{
                    animator.speed = 1f;
                }
            } 
        }
        #endregion

        [Header("Behaviour")]
        [SerializeField] private PlayerBehaviour behaviour;

        [Header("Classes")]
        [SerializeField] private Animator animator;

        private void Update() 
        {
            animator.SetFloat(MOVEMENT_KEY, behaviour.Inputs.MovementAxisAbs);

            if(behaviour.Inputs.MovementAxisAbs <= 0 && behaviour.Movement.SprintEnabled)
            {
                SprintAnimation = false;
            }
            else if(behaviour.Inputs.MovementAxisAbs > 0 && behaviour.Movement.SprintEnabled)
            {
                SprintAnimation = true;
            }
        }
    }
}
