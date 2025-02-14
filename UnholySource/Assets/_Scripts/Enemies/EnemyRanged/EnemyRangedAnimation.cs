using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyRangedAnimation : MonoBehaviour
    {
        #region Constants
        private const string WALKING_ANIM_KEY = "IsWalking";
        private const string RUNNING_ANIM_KEY = "IsRunning";
        private const string SHOOTING_ANIM_KEY = "IsShooting";

        private const string DEATH_ANIM_KEY = "IsDead";
        #endregion

        #region Encapsulation
        internal bool IsWalking { set { animator.SetBool(WALKING_ANIM_KEY, value); } }
        internal bool IsRunning { set { animator.SetBool(RUNNING_ANIM_KEY, value); } }
        internal bool IsShooting { set { animator.SetBool(SHOOTING_ANIM_KEY, value); } }

        internal bool IsDead { set { animator.SetBool(DEATH_ANIM_KEY, value); } }
        #endregion

        [Header("Classes")]
        [SerializeField] private Animator animator;
    }
}
