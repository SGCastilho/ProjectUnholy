using UnityEngine;

namespace Core.Enemies
{
    public sealed class PursuerAnimation : MonoBehaviour
    {
        #region Constants
        private const string ANIM_MOVEMENT_KEY = "isMoving";
        private const string ANIM_SEARCHING_KEY = "Searching";
        private const string ANIM_RUNNING_KEY = "Running";
        
        private const string ANIM_ATTACKING_KEY = "isAttacking";
        private const string ANIM_RUNNING_ATTACKING_KEY = "isRunningAttacking";
        private const string ANIM_ATTACKING_COMBO_KEY = "Combo";
        #endregion

        #region Encapsulation
        public bool IsMoving { set => animator.SetBool(ANIM_MOVEMENT_KEY, value); }
        public bool IsSearching { set => animator.SetBool(ANIM_SEARCHING_KEY, value); }
        public bool IsRunning { set => animator.SetBool(ANIM_RUNNING_KEY, value); }

        public bool IsAttacking { set => animator.SetBool(ANIM_ATTACKING_KEY, value); }
        public bool IsRunningAttacking { set => animator.SetBool(ANIM_RUNNING_ATTACKING_KEY, value); }
        public int CurrentCombo { set => animator.SetInteger(ANIM_ATTACKING_COMBO_KEY, value); }
        #endregion

        [Header("Classes")]
        [SerializeField] private Animator animator;
    }
}
