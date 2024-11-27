using UnityEngine;

namespace Core.Enemies
{
    public class PredatorAnimation : MonoBehaviour
    {
        #region Constants
        private const string CHASING_ANIMATION_KEY = "Chasing";
        private const string ATTACKING_ANIMATION_TRIGGER_KEY = "Attack";
        #endregion

        #region Encapsulation
        internal bool ChasingAnimation { set => animator.SetBool(CHASING_ANIMATION_KEY, value); }
        #endregion

        [Header("Classes")]
        [SerializeField] private Animator animator;

        internal void CallAttackingTrigger() => animator.SetTrigger(ATTACKING_ANIMATION_TRIGGER_KEY);
    }
}
