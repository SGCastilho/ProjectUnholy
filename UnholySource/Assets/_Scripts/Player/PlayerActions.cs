using UnityEngine;

namespace Core.Player
{
    public sealed class PlayerActions : MonoBehaviour
    {
        #region Encapsulation
        public bool IsHealing { set => isHealing = value; }
        public bool IsReloading { get => isReloading; set => isReloading = value; }

        internal bool IsAiming { get => isAiming; set => isAiming = value; }
        #endregion

        [Header("Behaviour")]
        [SerializeField] private PlayerBehaviour behaviour;

        [Header("Classes")]
        [SerializeField] private Transform raycastTransform;

        [Header("Settings")]
        [SerializeField] private bool isAiming;
        [SerializeField] private bool isHealing;
        [SerializeField] private bool isReloading;

        [Space(10)]

        [SerializeField] private LayerMask raycastLayer;
        [SerializeField] private float raycastLegth = 4f;

        [Space(10)]

        [SerializeField] [Range(0.1f, 1.6f)] private float meleeAttackCouldown = 1f;
        [SerializeField] [Range(0.1f, 1.6f)] private float rangedAttackCouldown = 0.4f;

        #region Editor Variable
        #if UNITY_EDITOR
        [Space(20)]

        [SerializeField] private bool showGizmos;
        [SerializeField] private bool showBackwards;

        [Space(6)]

        [SerializeField] private Color raycastLineColor;
        #endif
        #endregion

        private Ray _rayDetection;
        private RaycastHit _raycastHitDetection;

        private bool _isAttacking;

        private float _attackCouldown;
        private float _currentAttackCouldown;

        private void Update() 
        {
            if(_isAttacking)
            {
                _currentAttackCouldown += Time.deltaTime;
                if(_currentAttackCouldown >= _attackCouldown)
                {
                    _currentAttackCouldown = 0f;

                    _isAttacking = false;
                }
            }
        }

        internal void Attack()
        {
            if(_isAttacking || isHealing) return;

            if(isAiming && behaviour.Equipment.RangedEnabled)
            {
                if(behaviour.Resources.Bullets <= 0) return;

                behaviour.Animation.CallShootTrigger();

                behaviour.Resources.ModifyBullets(false, 1);

                if(!behaviour.Movement.IsFlipped)
                {
                    _rayDetection = new Ray(raycastTransform.position, Vector3.forward);
                    _raycastHitDetection = new RaycastHit();

                    if(Physics.Raycast(_rayDetection, out _raycastHitDetection, raycastLegth, raycastLayer))
                    {
                        //FAZER O INIMIGO TOMAR DANO
                    }
                }
                else
                {
                    _rayDetection = new Ray(raycastTransform.position, Vector3.back);
                    _raycastHitDetection = new RaycastHit();

                    if(Physics.Raycast(_rayDetection, out _raycastHitDetection, raycastLegth, raycastLayer))
                    {
                        //FAZER O INIMIGO TOMAR DANO
                    }
                }

                _attackCouldown = rangedAttackCouldown;

                _isAttacking = true;
            }
            else if(!isAiming && behaviour.Equipment.MeleeEnabled)
            {
                if(!behaviour.Equipment.MeleeEquipped)
                {
                    behaviour.Equipment.EquipMeleeWeapon();
                }

                behaviour.Correction.Correct(!behaviour.Movement.IsFlipped);

                behaviour.Animation.CallAttackTrigger();

                _attackCouldown = meleeAttackCouldown;

                _isAttacking = true;
            }
        }

        internal void AimingSetup()
        {
            if(!behaviour.Equipment.RangedEnabled || _isAttacking) return;

            if(!behaviour.Equipment.RangedEquipped)
            {
                behaviour.Equipment.EquipRangedWeapon();
            }

            isAiming = !isAiming;

            if(isAiming)
            {
                behaviour.Inputs.BlockActionsWhenAiming();
                behaviour.Movement.ChangeToAimMovementSpeed();

                behaviour.Animation.RangedAimingAnimation = isAiming;
            }
            else{
                behaviour.Inputs.AllowActionsBeforeAiming();
                behaviour.Movement.ResetMovementSpeed();

                behaviour.Animation.RangedAimingAnimation = isAiming;
            }
        }

        public void CancelAiming()
        {
            isAiming = false;
            behaviour.Animation.RangedAimingAnimation = false;

            behaviour.Movement.ResetMovementSpeed();
        }

        internal void Healing()
        {
            if(behaviour.Resources.HealthBottles <= 0 || isHealing || _isAttacking) return;

            behaviour.Inputs.BlockActions();

            isHealing = true;

            behaviour.Resources.ModifyHealingBottles(false, 1);

            behaviour.Equipment.EquipHealingBottle(true);
            behaviour.Animation.CallHealingTrigger();
        }

        #region Editor Functions
        #if UNITY_EDITOR
        private void OnDrawGizmos() 
        {
            if(showGizmos)
            {
                if(showBackwards)
                {
                    Gizmos.color = raycastLineColor;
                    Gizmos.DrawLine(raycastTransform.position, raycastTransform.position + raycastLegth * Vector3.back);
                }
                else
                {
                    Gizmos.color = raycastLineColor;
                    Gizmos.DrawLine(raycastTransform.position, raycastTransform.position + raycastLegth * Vector3.forward);
                }
            }
        }   
        #endif
        #endregion
    }
}
