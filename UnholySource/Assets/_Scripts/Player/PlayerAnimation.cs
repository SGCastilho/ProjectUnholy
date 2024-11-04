using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerAnimation : MonoBehaviour
    {
        #region Constant Variables
        private const string MOVEMENT_KEY = "Movement";

        private const string WEAPON_MELEE_EQUIPPED_KEY = "MeleeEquipped";
        private const string WEAPON_RANGED_EQUIPPED_KEY = "RangedEquipped";

        private const string WEAPON_RANGED_AIMING_KEY = "Aiming";

        private const string TRIGGER_MELEE_ATTACK_KEY = "MeleeAttack";
        private const string TRIGGER_AIMING_ATTACK_KEY = "Shoot";
        private const string TRIGGER_RANGED_REALODING_KEY = "Reloading";
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

        internal bool MeleeEquippedAnimation { set => animator.SetBool(WEAPON_MELEE_EQUIPPED_KEY, value); }
        internal bool RangedEquippedAnimation { set => animator.SetBool(WEAPON_RANGED_EQUIPPED_KEY, value); }

        internal bool RangedAimingAnimation { set => animator.SetBool(WEAPON_RANGED_AIMING_KEY, value); }
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

        internal void CallAttackTrigger() => animator.SetTrigger(TRIGGER_MELEE_ATTACK_KEY);
        
        internal void CallShootTrigger() => animator.SetTrigger(TRIGGER_AIMING_ATTACK_KEY);

        internal void CallReloadingTrigger() => animator.SetTrigger(TRIGGER_RANGED_REALODING_KEY);
    }
}
