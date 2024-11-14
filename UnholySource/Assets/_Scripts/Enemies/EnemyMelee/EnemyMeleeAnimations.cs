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

        #region Encapsulations
        public bool IsAttacking { get => _isAttacking; set => _isAttacking = value; }

        internal bool ChasingAnimation { set => animator.SetBool(CHASING_ANIMATION_KEY, value); }
        internal bool DeathAnimation { set => animator.SetBool(DEATH_ANIMATION_KEY, value); }
        #endregion
        
        [Header("Behaviour")]
        [SerializeField] private EnemyMeleeBehaviour behaviour;
        
        [Header("Classes")]
        [SerializeField] private Animator animator;

        private void Start()
        {
            ChasingAnimation = false;
            DeathAnimation = false;

            _isAttacking = false;
        }

        private void OnEnable() 
        {
            if(behaviour.Status.IsDead)
            {
                DeathAnimation = true;
            }
        }

        private bool _isAttacking;

        public void CallTriggerAttack() => animator.SetTrigger(ATTACK_TRIGGER_ANIMATION_KEY);

        public void CallTriggerHitted() => animator.SetTrigger(HITTED_TRIGGER_ANIMATION_KEY);
    }
}
