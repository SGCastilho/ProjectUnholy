using UnityEngine;

namespace Core.Enemies
{
    public sealed class EnemyMeleeAnimations : MonoBehaviour
    {
        #region Constants
        private const string CHASING_ANIMATION_KEY = "Chasing";
        private const string DEATH_ANIMATION_KEY = "Dead";

        private const string ATTACK_TRIGGER_ANIMATION_KEY = "Attack";
        private const string HITTED_TRIGGER_ANIMATION_KEY = "Hitted";
        #endregion

        internal bool ChasingAnimation { set => animator.SetBool(CHASING_ANIMATION_KEY, value); }

        [Header("Classes")]
        [SerializeField] private Animator animator;
    }
}
