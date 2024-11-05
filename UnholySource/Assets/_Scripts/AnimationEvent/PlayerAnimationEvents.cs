using Core.Player;
using Core.GameCamera;
using UnityEngine;

namespace Core.AnimationEvents
{
    public sealed class PlayerAnimationEvents : MonoBehaviour
    {
        [Header("Behaviour")]
        [SerializeField] private PlayerBehaviour behaviour;

        public void BlockActions()
        {
            behaviour.Inputs.BlockActions();
            behaviour.Inputs.BlockMovement();
        }

        public void EnableActions()
        {
            behaviour.Inputs.AllowActions();
            behaviour.Inputs.AllowMovement();
        }

        public void ApplyMeleeDamage()
        {
            AttackCameraShake();
        }

        private void AttackCameraShake()
        {
            if(CameraShake.Instance == null) return;

            CameraShake.Instance.AttackShake();
        }

        public void WhenReloadingEnds()
        {
            behaviour.Inputs.AllowActions();

            behaviour.Actions.IsReloading = false;
        }

        public void WhenHealingEnds()
        {
            behaviour.Status.ApplyHealthBottleHealing();

            behaviour.Actions.IsHealing = false;
            behaviour.Equipment.EquipHealingBottle(false);

            behaviour.Inputs.AllowActions();
        }
    }
}
